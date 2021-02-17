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


        public Audio(string relativePath)
        {
            Media = new MediaPlayer();
            Media.Open(new Uri(relativePath, UriKind.Relative));
            Name = relativePath.Substring(relativePath.LastIndexOf('\\') + 1).Replace(".mp3", "");
            SourceUrl = relativePath; 

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
    }
}
