using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CustomSoundtrack
{
    [Serializable]
    [DataContract(Namespace = "")]
    public class Settings
    {
        [CollectionDataContract(Namespace = "", ItemName = "Song")]
        private class Songs : List<SongInfo> { }

        [DataMember(Order = 0, IsRequired = false)]
        public string MusicPath { get; private set; }

        [DataMember(Order = 1)]
        public decimal Volume { get; private set; } = 0.5M;

        [DataMember(Order = 2)]
        public bool Loop { get; private set; } = false;

        [DataMember(Order = 3)]
        public bool Pool { get; private set; } = false;

        [DataMember(Name = "Music", Order = 4)]
        private Songs _songInfo;

        [IgnoreDataMember] public DirectoryInfo PluginPath { get; private set; }
        [IgnoreDataMember] public IReadOnlyList<SongInfo> SongInfo => _songInfo;

        public Settings() { }

        public Settings(string path) : this(new DirectoryInfo(path)) { }

        public Settings(DirectoryInfo path)
        {
            PluginPath = path;
        }

        /// <param name="path">Folder with settings.xml file</param>
        public static Settings LoadFromXml(string path)
        {
            using (Stream fs = File.OpenRead(Path.Combine(path, "settings.xml")))
            using (var rd = XmlReader.Create(fs))
            {
                var settings = (Settings)new DataContractSerializer(typeof(Settings)).ReadObject(rd);
                settings.PluginPath = new DirectoryInfo(path);

                return settings;
            }
        }

        /// <param name="path">Folder with settings.json file</param>
        public static Settings LoadFromJson(string path)
        {
            var serializer = new JsonSerializer();

            using (var fs = File.OpenRead(Path.Combine(path, "settings.json")))
            using (var sr = new StreamReader(fs))
            using (var jsr = new JsonTextReader(sr))
            {
                var settings = serializer.Deserialize<Settings>(jsr);
                settings.PluginPath = new DirectoryInfo(path);

                return settings;
            }
        }

        /// <param name="path">Folder with settings.json or settings.xml file</param>
        public static Settings TryLoad(string path)
        {
            Settings settings;
            try
            {
                settings = LoadFromXml(path);
            }
            catch (Exception ex)
            {
                settings = LoadFromJson(path);
            }

            if(settings.MusicPath == default)
            {
                settings.MusicPath = path;
            }

            foreach(var info in settings._songInfo)
            {
                info.File = new FileInfo(Path.Combine(settings.MusicPath, info.File.Name));
            }

            return settings;
        }
    }
}
