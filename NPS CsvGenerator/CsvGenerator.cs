using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NPS_CsvGenerator
{
    public class CsvGenerator
    {

        readonly Dictionary<string, string> _schema;
        StreamWriter _stream;
        //IMimsCmiXmlFileToCSVRecord _csvService = new MimsCmiXmlFileToCSVRecord();
        readonly string _separator;
        readonly string _enclose;

        private CsvGenerator()
        {

        }

        public CsvGenerator(string fileName, Dictionary<string, string> schema, string separator, string enclose, string newLine = "\n")
        {
            _separator = separator;
            _enclose = enclose;
            _schema = schema;
            _stream = new StreamWriter(fileName);
            _stream.NewLine = newLine;
        }

        public void AddHeader()
        {
            var sep = "";
            foreach (var sch in _schema)
            {
                _stream.Write($"{sep}{_enclose}{sch.Key}{_enclose}");
                sep = _separator;
                //csvOutput.WriteLine($"{enclose}CMIcode{enclose}{separator}{enclose}CMIxml{enclose}{separator}{enclose}CMIxmlescaped{enclose}");
            }
            _stream.WriteLine();
        }

        public void AddRecord(string[] data)
        {
            var sep = "";
            for (int i = 0; i < data.Length; i++)
            {
                var keyValue = _schema.ElementAt(i);
                switch (keyValue.Value)
                {
                    case "int":
                        _stream.Write($"{sep}{data[i]}");
                        break;
                    case "string":
                        _stream.Write($"{sep}{_enclose}{ApplyEncloseToCSVString(data[i], _enclose)}{_enclose}");
                        break;
                }
                sep = _separator;
            }
            _stream.WriteLine();
        }
        public void End()
        {
            _stream.Close();
        }

        public string ApplyEncloseToCSVString(string content, string enclose)
        {
            if (enclose == "\"" || enclose == "'")
            {
                content.Replace(enclose, enclose + enclose);
            }
            return content;
        }

    }
}
