using System;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace Models
{
    public sealed class Audio
    {
        /// <summary>
        /// Название аудио.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Длительность аудио в секундах.
        /// </summary>
        public double Duration { get; private set; }

        /// <summary>
        /// Путь к аудио файлу.
        /// </summary>
        public string SourceUrl { get; private set; }

        /// <summary>
        /// Объект MediaPlayer.
        /// </summary>
        public MediaPlayer Media { get; private set; }


        public Audio(string path)
        {
            Media = new MediaPlayer();
            Media.Open(new Uri(path, UriKind.Absolute));
            Name = Path.GetFileNameWithoutExtension(Media.Source.AbsoluteUri).Replace("%20", " ");
            SourceUrl = Media.Source.AbsoluteUri;

            GetDuration();
        }

        /// <summary>
        /// Метод получающий длительность аудио в секундах.
        /// </summary>
        private void GetDuration()
        {
            do
            {
                Thread.Sleep(150);

                if (Media.NaturalDuration.HasTimeSpan)
                {
                    Duration = Media.NaturalDuration.TimeSpan.TotalSeconds;
                    break;
                }

            } while (true);
        }

        //private void Broadcast(byte[] data)
        //{
        //    var r = new Mp3FileReader(WavToMP3(data));

        //    while ((frame = r.ReadNextFrame()) != null)
        //    {
        //        foreach (Consumer c in WebCast.Clients) c.Audio(frame.RawData);

        //        Console.Title = frame.FrameLength.ToString();
        //    }
        //}

        //private MemoryStream WavToMP3(byte[] wavFile)
        //{
        //    using (var retMs = new MemoryStream())
        //    using (var ms = new MemoryStream(wavFile))
        //    using (var rdr = new RawSourceWaveStream(ms, new WaveFormat(44100, 16, 1)))
        //    using (var wtr = new LameMP3FileWriter(retMs, rdr.WaveFormat, 128))
        //    {
        //        rdr.CopyTo(wtr);
        //        wtr.Flush();
        //        return new MemoryStream(retMs.ToArray());
        //    }
        //}

    }
}
