using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InsertToSql
{
    public class RawDataParser
    {
        private readonly TextReader reader;
        private IReadOnlyList<string>? currentSplittedLine;

        public RawDataParser(TextReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            this.reader = reader;
        }

        public string this[int i]
        {
            get => currentSplittedLine?[i] ?? throw new RawDataParserException();
        }

        public int Count()
        {
            return currentSplittedLine is not null ? currentSplittedLine.Count : throw new RawDataParserException();
        }

        public IReadOnlyList<string> GetLineData()
        {
            return currentSplittedLine is not null ? currentSplittedLine : throw new RawDataParserException();
        }

        public bool Read()
        {
            if (reader.ReadLine() is not string readLine)
            {
                return false;
            }

            var splitted = readLine.Split("\t");
            currentSplittedLine = splitted.ToList();
            return true;
        }
    }
}
