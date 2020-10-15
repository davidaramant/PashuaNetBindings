using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Humanizer;

namespace DataModelGenerator
{
    class Control
    {
        public string Name { get; set; }
        public List<Property> Properties { get; } = new List<Property>();

        public string ClassName => Name + "Control";
        public string FileName => ClassName + ".cs";
        public string PashuaName => Name.ToLower();
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
                if(string.IsNullOrWhiteSpace(line))
                    continue;

                var cols = line.Split('\t').Select(col => col.Trim()).ToArray();

                if (cols[0] != string.Empty && cols.Skip(1).All(c => c == string.Empty))
                {
                    controls.Add(new Control { Name = cols[0].Pascalize() });
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

                var needsSystem = control.Properties.Any(p => p.DataType.StartsWith("DateTime") || p.DataType.StartsWith("TimeSpan"));
                var needsCollections = control.Properties.Any(p => p.DataType.StartsWith("IEnumerable"));
                if (needsSystem)
                {
                    file.Line("using System;");
                }
                if (needsCollections)
                {
                    file.Line("using System.Collections.Generic;");
                }

                if (needsSystem && needsCollections)
                {
                    file.Line();
                }

                file.Line("namespace Pashua").OpenParen();

                file.Line($"public sealed class {control.ClassName}").OpenParen();

                foreach (var property in control.Properties)
                {
                    file.Line($"public {property.DataType} {property.Name.Pascalize()} {{ get; set; }}");
                }

                // Close class and namespace
                file.CloseParen().CloseParen();
            }
        }
    }
}
