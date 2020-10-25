using System;
using System.Collections.Generic;
using System.IO;

namespace DataModelGenerator
{
    public sealed class IndentedWriter : IDisposable
    {
        private readonly StreamWriter _writer;
        public IndentedWriter(StreamWriter writer) => _writer = writer;

        public int IndentionLevel { get; private set; }
        public int IndentionCount => IndentionLevel * 4;
        public string CurrentIndent => new string(' ', IndentionCount);

        public IndentedWriter IncreaseIndent()
        {
            IndentionLevel++;
            return this;
        }

        public IndentedWriter DecreaseIndent()
        {
            if (IndentionLevel == 0)
                throw new InvalidOperationException();
            IndentionLevel--;
            return this;
        }

        public IndentedWriter OpenParen() => Line("{").IncreaseIndent();
        public IndentedWriter CloseParen()=> DecreaseIndent().Line("}");

        public IndentedWriter Line(string line)
        {
            _writer.WriteLine(CurrentIndent + line);
            return this;
        }

        public IndentedWriter Line()
        {
            _writer.WriteLine();
            return this;
        }

        public IndentedWriter Documentation(string summary, string remarks = null)
        {
            Line("/// <summary>");
            foreach (var sumLine in BreakUpDocumentationLine(summary, IndentionCount))
            {
                Line("/// " + sumLine);
            }
            Line("/// </summary>");

            if (!string.IsNullOrWhiteSpace(remarks))
            {
                Line("/// <remarks>");
                foreach (var remarksLine in BreakUpDocumentationLine(remarks, IndentionCount))
                {
                    Line("/// " + remarksLine);
                }
                Line("/// </remarks>");

            }
            
            return this;
        }

        private static IEnumerable<string> BreakUpDocumentationLine(string longLine, int indent)
        {
            int maxLength = 120 - indent - 1 - 4;
            while (longLine.Length > maxLength)
            {
                var breakPoint = longLine.LastIndexOf(' ', maxLength);
                yield return longLine.Substring(0, breakPoint);
                longLine = longLine.Substring(breakPoint + 1);
            }

            yield return longLine;
        }

        public void Dispose()
        {
            if (IndentionLevel != 0)
            {
                throw new InvalidOperationException("Indention level is screwed up.");
            }
        }
    }
}