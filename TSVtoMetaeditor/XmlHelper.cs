using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TSVtoMetaeditor.Json;

namespace TSVtoMetaeditor.Xml
{
    public static class XmlHelper
    {
        public static bool Replace = true;
        public static void WriteToFile(
            string startPath, 
            string imgPath,
            List<MarkupInfo> jsonMarkup,
            string outputDir)
        {
            var savePath = Path.Combine(
                    outputDir,
                    Path.ChangeExtension(imgPath, ".xml"));

            if (File.Exists(savePath) && !Replace)
            {
                Console.WriteLine($"Skiped -noreplace: {savePath}");
                return;
            }

            var serializer = new XmlSerializer(typeof(Image));
            var realImg = System.Drawing.Image.FromFile(Path.Combine(startPath, imgPath));
            // Create directory
            new FileInfo(savePath).Directory.Create();

            using (TextWriter writer = new StreamWriter(savePath))
            {
                var img = new Image();

                var plates = new List<Plate>();

                foreach (var markup in jsonMarkup)
                {
                    var plate = new Plate();
                    if (markup.Data.Count > 4)
                    {
                        Console.WriteLine($"Skiped: {imgPath}, points: {markup.Data.Count}");
                        continue;
                    }
                    foreach (var data in markup.Data)
                    {
                        plate.Region.Add(new Point
                        {
                            X = (int)(data.X * realImg.Width),
                            Y = (int)(data.Y * realImg.Height)
                        });
                    }
                    plates.Add(plate);
                }
                img.Plates = plates;
                serializer.Serialize(writer, img);
            }

        }
    }
}
