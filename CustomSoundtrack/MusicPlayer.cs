using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomSoundtrack
{
    internal class MusicPlayer
    {
        private AsioOut _out;
        private AudioFileReader

        public MusicPlayer()
        {
            _out = new AsioOut();
        }

        internal void Pause()
        {
            
        }

        internal void Play()
        {
            
        }

        internal void Play(FileInfo file)
        {
            switch(file.Extension)
            {
                case ".mp3":
                    break;
                case ".wav":
                    break;
            }
            _out.Init()
        }
    }
}
