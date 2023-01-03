using HarmonyLib;
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CustomSoundtrack
{
    [Serializable]
    public class SongInfo : IXmlSerializable {
        [XmlAttribute] public FileInfo File;
        [XmlAttribute] public string[] Scenes;
        [XmlAttribute] public bool Boss = false;
        [XmlAttribute] public decimal Volume = 1;

        public SongInfo()
        {
            var rnd = new Random();
            Boss = rnd.Next(0, 10) == 5;
            Volume = rnd.Next(30, 100) / 100;
        }

        public SongInfo(FileInfo file) : this()
        {
            File = file;
        }

        public SongInfo(string file) : this(new FileInfo(file)) { }

        public XmlSchema GetSchema() => (null);

        public void ReadXml(XmlReader reader)
        {
            File = new FileInfo(reader.GetAttribute("Name"));
            Scenes = reader.GetAttribute("Scenes").Split(new char[] { ',' });
            Boss = bool.Parse(reader.GetAttribute("Boss"));
            Volume = decimal.Parse(reader.GetAttribute("Volume"));
            reader.ReadToNextSibling("Song");
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", File.Name);
            writer.WriteAttributeString("Scenes", Scenes.Join(delimiter: ","));
            writer.WriteAttributeString("Boss", Boss.ToString());
            writer.WriteAttributeString("Volume", Volume.ToString());
        }
    }
}
