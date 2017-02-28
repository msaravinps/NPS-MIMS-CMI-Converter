using NPS_CsvGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NPS_MIMS_CMI_Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmiXmlPath = @"Z:\MSaravi\MIMS-Sample\CMI\CMIxml20170100\CMIxml";
            var xmiXmlExtension = "*.xml";
            var schema = new Dictionary<string, string> {
                {"CMIcode","int" },
                {"CMIxml","string" },
                {"CMIxmlescaped","string" }
            };
            var _CsvGenerator = new CsvGenerator(@"Z:\MSaravi\MIMS-Sample\CMI\output.csv", schema, "\t", "~");
            var fileNames = Directory.EnumerateFiles(cmiXmlPath, xmiXmlExtension).ToList();
            var progress = 0.0;
            var total = (double)fileNames.Count;
            _CsvGenerator.AddHeader();
            foreach (string fileName in fileNames)
            {
                string xmlFileContents = File.ReadAllText(fileName);
                var cmiCode = GetCmiCodeFromFileName(fileName);
                var escapedXmlFileContents = System.Security.SecurityElement.Escape(xmlFileContents);
                _CsvGenerator.AddRecord(new string[] { cmiCode, xmlFileContents, escapedXmlFileContents });
                Console.WriteLine(cmiCode + " finished, %" + Math.Round(progress * 100 / total, 1).ToString());
                progress++;
            }
            _CsvGenerator.End();
        }

        public static string GetCmiCodeFromFileName(string fileName)
        {
            var lastPart = fileName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList().Last();
            var code = lastPart.ToLower().Replace("cm", "").Replace(".xml", "");
            return Convert.ToInt64(code).ToString();
        }

    }
}
