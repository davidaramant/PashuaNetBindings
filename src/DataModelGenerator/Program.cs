using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;

namespace DataModelGenerator
{
    sealed class Control
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public List<Property> Properties { get; } = new List<Property>();

        public string ClassName => Name;
        public string FileName => ClassName + ".Generated.cs";

        public bool IsWindow => Name.ToLowerInvariant() == "window";

        public string PashuaName => IsWindow ? "*" : Name.ToLower();
    }

    enum PropertyType
    {
        String,
        NullableDouble,
        NullableInt,
        Int,
        Bool,
        Enum,
        NullableTimeSpan,
        NullableDateTime,
        StringCollection,
    }

    sealed class Property
    {
        public string Name { get; set; }
        public string ActualName { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public bool Required { get; set; }
        public string Default { get; set; }
        public string DataType { get; set; }

        public bool HasDefault => Default != "-";
        public string DefaultValue =>
            HasDefault ?
                (DataType == "string" ? $"\"{Default}\"" : Default)
                : "null";


        public string PashuaName
        {
            get
            {
                var name = string.IsNullOrEmpty(ActualName) ? Name : ActualName;
                return name.ToLowerInvariant();
            }
        }

        public PropertyType Type
        {
            get
            {
                return DataType switch
                {
                    "string" => PropertyType.String,
                    "double?" => PropertyType.NullableDouble,
                    "int?" => PropertyType.NullableInt,
                    "int" => PropertyType.Int,
                    "bool" => PropertyType.Bool,
                    "DateTime?" => PropertyType.NullableDateTime,
                    "TimeSpan?" => PropertyType.NullableTimeSpan,
                    "IEnumerable<string>" => PropertyType.StringCollection,
                    _ => PropertyType.Enum,
                };
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var controls = ReadControlInfo();

            GenerateDataModel(controls);
        }

        static List<Control> ReadControlInfo()
        {
            var controls = new List<Control>();

            // Skip the header row
            foreach (var line in File.ReadLines("Pashua Documentation.tsv").Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var cols = line.Split('\t').Select(col => col.Trim()).ToArray();

                if (cols[0] != string.Empty && cols.Skip(4).All(c => c == string.Empty))
                {
                    controls.Add(new Control
                    {
                        Name = cols[0].Pascalize(),
                        Summary = cols[2],
                        Remarks = cols[3],
                    });
                }
                else
                {
                    controls.Last().Properties.Add(new Property
                    {
                        Name = cols[0].Pascalize(),
                        ActualName = cols[1],
                        Summary = cols[2],
                        Remarks = cols[3],
                        Required = cols[4] == "Yes",
                        Default = cols[5],
                        DataType = cols[6],
                    });
                }
            }

            return controls;
        }

        static void GenerateDataModel(List<Control> controls)
        {
            string outputPath = Path.Combine("..", "..", "..", "..", "PashuaNetBindings", "Controls");

            if (!Directory.Exists(outputPath))
            {
                throw new Exception("Screwed up directory");
            }

            foreach (var control in controls)
            {
                var filePath = Path.Combine(outputPath, control.FileName);

                using var fs = File.Open(filePath, FileMode.Create);
                using var writer = new StreamWriter(fs);
                using var file = new IndentedWriter(writer);

                WriteUsings(file, control);

                file.Line("namespace Pashua")
                    .OpenParen()
                    .Documentation(control.Summary, control.Remarks)
                    .Line($"public sealed partial class {control.ClassName} : IPashuaControl")
                    .OpenParen();

                WriteProperties(file, control);
                WriteWriteToMethod(file, control);
                file.Line();
                WriteValidationMethods(file, control);
                
                file.CloseParen().CloseParen(); // Close class and namespace
            }
        }

        private static void WriteWriteToMethod(IndentedWriter file, Control control)
        {
            file
                .Documentation("Writes the control script to the given writer.")
                .Line(
                    "/// <exception cref=\"PashuaScriptException\">Thrown if the control was not configured correctly.</exception>")
                .Line("public void WriteTo(TextWriter writer)")
                .OpenParen()
                .Line("var errors = GetValidationIssues();")
                .Line("if(errors.Any())")
                .OpenParen()
                .Line("throw new PashuaScriptException(errors);")
                .CloseParen()
                .Line();


            if (!control.IsWindow)
            {
                file.Line($"writer.WriteLine($\"{{Id}}.type = {control.PashuaName}\");");
            }

            file.Line("WriteSpecialProperties(writer);");

            foreach (var property in control.Properties)
            {
                var id = control.IsWindow ? "*" : "{Id}";

                if (property.Type == PropertyType.StringCollection)
                {
                    file.Line($"foreach (var option in {property.Name})")
                        .OpenParen()
                        .Line($"writer.WriteLine($\"{id}.{property.PashuaName} = {{option}}\");")
                        .CloseParen();
                }
                else if (property.Name == "Tooltip")
                {
                    file.Line("if (!string.IsNullOrWhiteSpace(Tooltip))")
                        .OpenParen()
                        .Line($"writer.WriteLine($\"{id}.tooltip = {{Tooltip.Replace(\"\\n\", \"\\\\n\")}}\");")
                        .CloseParen();
                }
                else
                {
                    var checkForDefault = !property.Required;

                    if (checkForDefault)
                    {
                        if (property.Type == PropertyType.String && !property.HasDefault)
                        {
                            file.Line($"if (!string.IsNullOrWhiteSpace({property.Name}))");
                        }
                        else
                        {
                            file.Line($"if ({property.Name} != {property.DefaultValue})");
                        }

                        file.OpenParen();
                    }

                    var value = property.Type switch
                    {
                        PropertyType.Bool => $"({property.Name} ? 1 : 0)",
                        PropertyType.NullableDateTime => property.Name +"?.ToString(\"yyyy-mm-dd hh:mm\")",
                        PropertyType.NullableDouble => property.Name +":N2",
                        PropertyType.NullableTimeSpan =>  $"(int){property.Name}?.TotalSeconds",
                        PropertyType.Enum => $"SerializeEnum({property.Name})",
                        _ => property.Name,
                    };

                    file.Line($"writer.WriteLine($\"{id}.{property.PashuaName} = {{{value}}}\");");

                    if (checkForDefault)
                    {
                        file.CloseParen();
                    }

                }
            }

            file.CloseParen(); // Close WriteTo

            file.Line().Line("partial void WriteSpecialProperties(TextWriter writer);");
        }

        private static void WriteProperties(IndentedWriter file, Control control)
        {
            file.Documentation("The name of this element in the Pashua script.  Should not be needed outside of the framework.")
                .Line(control.IsWindow
                ? $"public string Id => \"*\";"
                : $"public string Id => \"{control.PashuaName}\" + GetHashCode();")
                .Line();

            foreach (var property in control.Properties)
            {
                file.Documentation(property.Summary, property.Remarks)
                    .Line($"public {property.DataType} {property.Name.Pascalize()} {{ get; set; }}" +
                          (property.HasDefault ? $" = {property.DefaultValue};" : ""))
                    .Line();
            }
        }

        private static void WriteUsings(IndentedWriter file, Control control)
        {
            var needsSystem =
                control.Properties.Any(p => p.DataType.StartsWith("DateTime") || p.DataType.StartsWith("TimeSpan"));
            if (needsSystem)
            {
                file.Line("using System;");
            }

            file.Line("using System.Collections.Generic;");
            file.Line("using System.IO;");
            file.Line("using System.Linq;");

            file.Line();
        }

        private static void WriteValidationMethods(IndentedWriter file, Control control)
        {
            file.Documentation("Returns all the validation errors with the control.")
                .Line("/// <returns>All the issues.</returns>")
                .Line("public IEnumerable<string> GetValidationIssues()")
                .OpenParen()
                .Line("var errors = new List<string>();");

            foreach (var positive in new[] {"X", "Y", "Rows", "Width", "Height", "MaxWidth", "MaxHeight"})
            {
                if (control.Properties.Any(p => p.Name == positive))
                {
                    file.Line($"if ({positive} < 0)")
                        .OpenParen()
                        .Line($"errors.Add(\"{control.Name} {positive} must be greater than or equal to 0.\");")
                        .CloseParen();
                }
            }

            if (control.Properties.Any(p => p.Name == "RelY"))
            {
                file.Line("if (RelY <= -20)")
                    .OpenParen()
                    .Line($"errors.Add(\"{control.Name} RelY must be greater than -20.\");")
                    .CloseParen();
            }

            if (control.Properties.Any(p => p.Name == "Label" && p.Required))
            {
                file.Line("if (string.IsNullOrWhiteSpace(Label))")
                    .OpenParen()
                    .Line($"errors.Add(\"{control.Name} Label must be set.\");")
                    .CloseParen();
            }

            if (control.Properties.Any(p => p.Name == "Options" ))
            {
                file.Line("if (Options == null || Options.Any(string.IsNullOrWhiteSpace))")
                    .OpenParen()
                    .Line($"errors.Add(\"{control.Name} Options must be set and have at least one value.  Empty strings are not valid options.\");")
                    .CloseParen();

                if (control.Properties.Any(p => p.Name == "Default"))
                {
                    file.Line("else if(!string.IsNullOrWhiteSpace(Default) && !Options.Contains(Default))")
                        .OpenParen()
                        .Line($"errors.Add(\"{control.Name} Default must be one of the Options.\");")
                        .CloseParen();
                }
            }

            file.Line("AdditionalValidation(errors);")
                .Line("return errors;")
                .CloseParen()
                .Line()
                .Line("partial void AdditionalValidation(List<string> errors);");
        }
    }
}
