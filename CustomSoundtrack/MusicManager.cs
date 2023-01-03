using CustomSoundtrack.Utils;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

namespace CustomSoundtrack
{
    internal class MusicManager
    {
        public IReadOnlyList<SongInfo> Songs;

        public MusicPlayer Player => _player;

        private SongInfo _currentlyPlaying;
        private Settings _settings;
        private MusicPlayer _player;

        private Random _random = new Random();

        private FadeInOutSampleProvider fader;

        public MusicManager(Settings settings)
        {
            _settings = settings;
            Songs = _settings.SongInfo != default && _settings.SongInfo.Count != 0 ? _settings.SongInfo :
                settings.PluginPath.GetFiles("*.mp3")
                .Concat(settings.PluginPath.GetFiles("*.wav"))
                .Select(e => new SongInfo(e)).ToList();

            _player = new MusicPlayer();
        }

        internal void PlayNext(bool isTeleporter = true)
        {
            var goodMusicChoices = Songs.Where(music => {
                var bossTest = isTeleporter == music.Boss;
                return music.File != null && bossTest && SceneManager.GetActiveScene().MostlyMatches(music.Scenes);
            }).ToList();

            SongInfo choice = null;

            goodMusicChoices = (List<SongInfo>)(goodMusicChoices.Count > 0 ? goodMusicChoices : Songs);

            do
            {
                choice = goodMusicChoices[_random.Next(goodMusicChoices.Count)];
            }
            while (choice == _currentlyPlaying);

            _player.Play(choice.File.FullName);
            return;
        }
    }
}
