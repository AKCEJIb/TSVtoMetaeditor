using System;
using System.Collections.Generic;
using System.Text;

namespace TSVtoMetaeditor.Json
{
    public class MarkupData
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
    public class MarkupInfo
    {
        public string Type { get; set; }
        public List<MarkupData> Data { get; set; } 
    }
}
