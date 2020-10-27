using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Pashua.ScriptExtensions;
using Xunit;
using Xunit.Abstractions;

namespace Pashua.Tests
{
    public class ScriptExtensionTests
    {
        private readonly ITestOutputHelper _output;

        public ScriptExtensionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldWriteScriptToGivenWriter()
        {
            using var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, leaveOpen: true))
            {
                var script = new List<IPashuaControl>
                {
                    new Window {Title = "Title"},
                    new Text {Default = "Some text"},
                    new Button {Label = "A button"},
                };
                
                script.WriteTo(writer);
            }

            stream.Position = 0;
            using var reader = new StreamReader(stream);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                _output.WriteLine(line);
            }
        }
    }
}
