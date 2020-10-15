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

        public string FileName => Name + ".cs";
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

            // Skip the header row an all blank lines
            foreach (var line in File.ReadLines("Pashua Documentation.tsv").Skip(1).Where(line => line != string.Empty))
            {
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

                writer.WriteLine($"//{control.Name}");
            }
        }
    }
}
