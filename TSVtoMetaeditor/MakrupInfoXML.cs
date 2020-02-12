using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TSVtoMetaeditor.Xml
{
    public class Point
    {
        [XmlAttribute]
        public int X { get; set; }
        [XmlAttribute]
        public int Y { get; set; }
    }

    public class Plate
    {
        public Number Number { get; set; } = new Number();
        public Stencil Stencil { get; set; } = new Stencil();
        public Layout Layout { get; set; } = new Layout();
        public Country Country { get; set; } = new Country();
        public Confidence Confidence { get; set; } = new Confidence();
        public Tags Tags { get; set; } = new Tags { Value = "HighQuality" };
        public List<Point> Region { get; set; } = new List<Point>();
    }
    public class Confidence
    {
        [XmlAttribute]
        public int Value { get; set; }
    }
    public class Country
    {
        [XmlAttribute]
        public string Value { get; set; } = "";
    }
    public class Layout
    {
        [XmlAttribute]
        public int Value { get; set; }
    }
    public class Number
    {
        [XmlAttribute]
        public string Value { get; set; } = "000";
    }
    public class Stencil
    {
        [XmlAttribute]
        public string Value { get; set; }
    }
    public class DongleId
    {
        [XmlAttribute]
        public string Value { get; set; } = "";
    }
    public class Description
    {
        [XmlAttribute]
        public string Value { get; set; } = "";
    }
    public class HumanChecked
    {
        [XmlAttribute]
        public bool Value { get; set; } = true;
    }
    public class Important
    {
        [XmlAttribute]
        public bool Value { get; set; } = false;
    }
    public class Tags
    {
        [XmlAttribute]
        public string Value { get; set; }
    }
    public class PixelAspectRatio
    {
        [XmlAttribute]
        public int Value { get; set; } = 1;
    }
    public class Image
    {
        [XmlAttribute]
        public string Version { get; set; } = "2.0";
        public DongleId DongleId { get; set; } = new DongleId();
        public HumanChecked HumanChecked { get; set; } = new HumanChecked();
        public Important Important { get; set; } = new Important();
        public PixelAspectRatio PixelAspectRatio { get; set; } = new PixelAspectRatio();
        public Description Description { get; set; } = new Description();
        public Tags Tags { get; set; } = new Tags { Value = "BrightnessLight" };
        public List<Plate> Plates { get; set; } = new List<Plate>();
    }
}
