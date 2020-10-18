using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Humanizer;

namespace DataModelGenerator
{
    class Control
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public List<Property> Properties { get; } = new List<Property>();

        public string ClassName => Name;
        public string FileName => ClassName + ".cs";

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

    class Property
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
            string outputPath = Path.Combine("..", "..", "..", "..", "PashuaNetBindings");

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

                WriteUsings(control, file);

                file.Line("namespace Pashua").OpenParen();

                WriteDocumentation(file, control.Summary, control.Remarks, 8);
                file.Line($"public sealed class {control.ClassName} : IPashuaControl").OpenParen();

                WriteProperties(control, file);
                WriteWriteToMethod(file, control);


                file.CloseParen().CloseParen(); // Close class and namespace
            }
        }

        private static void WriteWriteToMethod(IndentedWriter file, Control control)
        {
            file
                .Line("/// <summary>")
                .Line("/// Writes the control script to the given writer.")
                .Line("/// </summary>")
                .Line("/// <exception cref=\"PashuaControlSetupException\">Thrown if the control was not configured correctly.</exception>")
                .Line("public void WriteTo(StreamWriter writer)")
                .OpenParen();

            if (!control.IsWindow)
            {
                file.Line($"writer.WriteLine($\"{{Id}}.type = {control.PashuaName}\");");
            }

            foreach (var property in control.Properties)
            {
                var id = control.IsWindow ? "*" : "{Id}";

                // Very special case
                if (property.PashuaName == "fonttype")
                {
                    file.Line($"if ({property.Name} != {property.DefaultValue})")
                        .OpenParen()
                        .Line($"writer.WriteLine($\"{id}.{property.PashuaName} = fixed;\");")
                        .CloseParen();
                }
                else if (property.Type == PropertyType.StringCollection)
                {
                    file.Line($"foreach (var option in {property.Name})")
                        .OpenParen()
                        .Line($"writer.WriteLine($\"{id}.{property.PashuaName} = {{option}};\");")
                        .CloseParen();
                }
                else
                {
                    var checkForDefault = !property.Required;

                    if (checkForDefault)
                    {
                        file.Line($"if ({property.Name} != {property.DefaultValue})")
                            .OpenParen();
                    }

                    var value = property.Type switch
                    {
                        PropertyType.Bool => $"({property.Name} ? 1 : 0)",
                        PropertyType.NullableDateTime => property.Name +"?.ToString(\"yyyy-mm-dd hh:mm\")",
                        PropertyType.NullableDouble => property.Name +":N2",
                        PropertyType.NullableTimeSpan =>  $"(int){property.Name}?.TotalSeconds",
                        PropertyType.Enum => $"{property.Name}.ToString().ToLowerInvariant()",
                        _ => property.Name,
                    };

                    file.Line($"writer.WriteLine($\"{id}.{property.PashuaName} = {{{value}}};\");");

                    if (checkForDefault)
                    {
                        file.CloseParen();
                    }

                }
            }

            file.CloseParen(); // Close WriteTo
        }

        private static void WriteProperties(Control control, IndentedWriter file)
        {
            if (!control.IsWindow)
            {
                file.Line($"internal string Id => \"{control.PashuaName}\" + GetHashCode();").Line();
            }

            foreach (var property in control.Properties)
            {
                WriteDocumentation(file, property.Summary, property.Remarks, 12);
                file.Line($"public {property.DataType} {property.Name.Pascalize()} {{ get; set; }}" +
                          (property.HasDefault ? $" = {property.DefaultValue};" : ""));
                file.Line();
            }
        }

        private static void WriteUsings(Control control, IndentedWriter file)
        {
            var needsSystem =
                control.Properties.Any(p => p.DataType.StartsWith("DateTime") || p.DataType.StartsWith("TimeSpan"));
            var needsCollections = control.Properties.Any(p => p.DataType.StartsWith("IEnumerable"));
            if (needsSystem)
            {
                file.Line("using System;");
            }

            if (needsCollections)
            {
                file.Line("using System.Collections.Generic;");
            }

            file.Line("using System.IO;");

            file.Line();
        }

        private static void WriteDocumentation(IndentedWriter file, string summary, string remarks, int indent)
        {
            file.Line("/// <summary>");
            foreach (var sumLine in BreakUpDocumentationLine(summary, indent))
            {
                file.Line("/// " + sumLine);
            }
            file.Line("/// </summary>");
            if (!string.IsNullOrWhiteSpace(remarks))
            {
                file.Line("/// <remarks>");
                foreach (var remLine in BreakUpDocumentationLine(remarks, indent))
                {
                    file.Line("/// " + remLine);
                }
                file.Line("/// </remarks>");
            }
        }

        private static IEnumerable<string> BreakUpDocumentationLine(string longLine, int indent)
        {
            int maxLength = 120 - indent - 1;
            while (longLine.Length >= maxLength)
            {
                var breakPoint = longLine.LastIndexOf(' ', maxLength);
                yield return longLine.Substring(0, breakPoint);
                longLine = longLine.Substring(breakPoint + 1);
            }

            yield return longLine;
        }
    }
}
