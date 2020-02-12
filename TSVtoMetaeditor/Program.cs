using LinearTsvParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TSVtoMetaeditor.Json;
using TSVtoMetaeditor.Xml;

namespace TSVtoMetaeditor
{
    class Program
    {
        private const string HelpString = "Using: tsvtometaeditor.exe\n" +
            "[optional flags:\n" +
            "-noreplace - Disable replace .xml files in directory if one exists]\n" +
            "tsv_FILE_path - Path to .tsv file\n" +
            "start_DIR_path - Path to root images folder\n" +
            "output_DIR_path - Output directory path\n" +
            "image_path_COLUMNNAME - Name of column that contains path to img\n" +
            "json_output_COLUMNNAME - Name of column that contains json info\n\n" +
            @"Example 1: tsvtometaeditor.exe -noreplace C:\Users\admin\Desktop\Mallenom\checked_assignments_images_3k_05-02-2020.tsv C:\Users\admin\Desktop\Mallenom D:\result INPUT:image OUTPUT:result" +
            "\n" +
            @"Example 2: tsvtometaeditor.exe C:\Users\admin\Desktop\Mallenom\checked_assignments_images_3k_05-02-2020.tsv C:\Users\admin\Desktop\Mallenom C:\Users\admin\Desktop\Mallenom INPUT:image OUTPUT:result\n";
        public static void PrintUsage() => Console.WriteLine(HelpString);

        public static void RunConvert(
            string[] args,
            bool replace)
        {
            var tsvPath         = args[0];
            var startDir        = args[1];
            var outputPath      = args[2];
            var imgPathColumn   = args[3];
            var jsonColumn      = args[4];
            XmlHelper.Replace   = replace;

            if (!replace)
            {
                tsvPath         = args[1];
                startDir        = args[2];
                outputPath      = args[3];
                imgPathColumn   = args[4];
                jsonColumn      = args[5];
            }
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
                        var fields      = tsvReader.ReadLine();

                        var imgPath     = fields[imgPathColIdx];
                        var jsonData    = fields[jsonColIdx];
                        jsonData        = jsonData[1..^1].Replace("\"\"", "\"");

                        var jsonMarkupInfo = JsonConvert.DeserializeObject<List<MarkupInfo>>(jsonData);

                        XmlHelper.WriteToFile(startDir, imgPath, jsonMarkupInfo, outputPath);
                    }

                    Console.WriteLine("Converting Done!");
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 5)
                RunConvert(args, true);
            else if (args.Length == 6)
            {
                switch (args[0])
                {
                    case "-noreplace":
                        RunConvert(args, false);
                        break;
                    default:
                        PrintUsage();
                        break;
                }
            }
            else
                PrintUsage();

            //RunConvert(new string[] {
            //        @"C:\Users\admin\Desktop\Mallenom\checked_assignments_images_3k_05-02-2020.tsv",
            //        @"C:\Users\admin\Desktop\Mallenom",
            //        @"D:\result",
            //        "INPUT:image",
            //        "OUTPUT:result"
            //    });

            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }
    }
}
