using LinearTsvParser;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml;
using TSVtoMetaeditor.Json;
using TSVtoMetaeditor.Xml;

namespace TSVtoMetaeditor
{
    class Program
    {
        private const string HelpString = "Using: tsvtometaeditor.exe\n" +
            "tsv_FILE_path - Path to .tsv file\n" +
            "start_DIR_path - Path to root images folder\n" +
            "output_DIR_path - Output directory path\n" +
            "image_path_COLUMNNAME - Name of column that contains path to img\n" +
            "json_output_COLUMNNAME - Name of column that contains json info\n\n" +
            @"example: tsvtometaeditor.exe C:\Users\admin\Desktop\Mallenom\checked_assignments_images_3k_05-02-2020.tsv C:\Users\admin\Desktop\Mallenom D:\result INPUT:image OUTPUT:result";
        public static void PrintUsage() => Console.WriteLine(HelpString);

        public static void RunConvert(string[] args)
        {
            var tsvPath = args[0];
            var startDir = args[1];
            var outputPath = args[2];
            var imgPathColumn = args[3];
            var jsonColumn = args[4];

            using (var input = File.OpenRead(tsvPath))
            {
                using (var tsvReader = new TsvReader(input))
                {
                    // Read tsv header
                    var columns = tsvReader.ReadLine();

                    // Get indexes
                    var imgPathColIdx   = columns.IndexOf(imgPathColumn);
                    var jsonColIdx      = columns.IndexOf(jsonColumn);

                    if (imgPathColIdx == -1 || jsonColIdx == -1)
                    {
                        Console.WriteLine("ERROR: Could't find columns!");
                        return;
                    }

                    while (!tsvReader.EndOfStream)
                    {
                        var fields = tsvReader.ReadLine();

                        var imgPath = fields[imgPathColIdx];
                        var jsonData = fields[jsonColIdx];
                        jsonData = jsonData[1..^1].Replace("\"\"", "\"");

                        var jsonMarkupInfo = JsonConvert.DeserializeObject<List<MarkupInfo>>(jsonData);

                        XmlHelper.WriteToFile(startDir, imgPath, jsonMarkupInfo, outputPath);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 5)
                PrintUsage();
            else
                RunConvert(args);

            //RunConvert(new string[] {
            //        @"C:\Users\admin\Desktop\Mallenom\checked_assignments_images_3k_05-02-2020.tsv",
            //        @"C:\Users\admin\Desktop\Mallenom",
            //        @"D:\result",
            //        "INPUT:image",
            //        "OUTPUT:result"
            //    });

            Console.WriteLine("Done!");
            Console.ReadKey(true);
        }
    }
}
