using NPS_CsvGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NPS_MIMS_CMI_Converter
{
    class Program
    {
        public static object ConfigurationManager { get; private set; }

        static void Main(string[] args)
        {
            var cmiXmlPath = args[0];// @"Z:\MSaravi\MIMS-Sample\CMI\CMIxml20170100\CMIxml";
            var outputPath = args[1];//@"Z:\MSaravi\MIMS-Sample\CMI\output.csv"
            var xmiXmlExtension = @System.Configuration.ConfigurationManager.AppSettings["fileType"];// "*.xml";
            var separator = @System.Configuration.ConfigurationManager.AppSettings["separator"];// "*.xml";
            var enclose = @System.Configuration.ConfigurationManager.AppSettings["enclose"];// "*.xml";
            var newLine = @System.Configuration.ConfigurationManager.AppSettings["newLine"];// "*.xml";
            var schema = GetSchema();
            var _CsvGenerator = new CsvGenerator(outputPath, schema, separator, enclose, newLine);
            var fileNames = System.IO.Directory.EnumerateFiles(cmiXmlPath, xmiXmlExtension).ToList();
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
                if (progress > 3)
                    break;        
            }
            _CsvGenerator.End();
        }

        public static string GetCmiCodeFromFileName(string fileName)
        {
            var lastPart = fileName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList().Last();
            var code = lastPart.ToLower().Replace("cm", "").Replace(".xml", "");
            return Convert.ToInt64(code).ToString();
        }

        static Dictionary<string, string> GetSchema()
        {
            var keys = @System.Configuration.ConfigurationManager.AppSettings["schema:keys"].Split(new char[] { ',' }).Select(s => s.Trim()).ToArray();
            var types = @System.Configuration.ConfigurationManager.AppSettings["schema:types"].Split(new char[] { ',' }).Select(s => s.Trim()).ToArray();
            var schema = new Dictionary<string, string>();
            for (var i = 0; i < keys.Length; i++)
            {
                schema.Add(keys[i], types[i]);
            }
            return schema;
        }
    }
}
