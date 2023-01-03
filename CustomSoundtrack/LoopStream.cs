using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomSoundtrack
{
    internal class LoopStream : WaveStream, IDisposable
    {

        private WaveStream _sourceStream;
        private bool _enableLooping = true;

        public LoopStream(WaveStream sourceStream, bool shouldLoop)
        {
            _sourceStream = sourceStream;
            _enableLooping = shouldLoop;
        }

        public override WaveFormat WaveFormat
        {
            get { return _sourceStream.WaveFormat; }
        }

        public override long Position
        {
            get { return _sourceStream.Position; }
            set { _sourceStream.Position = value; }
        }

        public override long Length
        {
            get { return _sourceStream.Length; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            while (read < count)
            {
                int required = count - read;
                int readThisTime = _sourceStream.Read(buffer, offset + read, required);
                if (readThisTime < required || _sourceStream.Position >= _sourceStream.Length)
                {
                    if (!_enableLooping)
                    {
                        break;
                    }
                    _sourceStream.Position = 0;
                }
                read += readThisTime;
            }
            return read;
        }

        public new void Dispose()
        {
            base.Dispose();
            _sourceStream.Close();
        }
    }
}
