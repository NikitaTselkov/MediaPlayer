using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace Models
{
    public partial class AudioControl
    {
        #region Props

        /// <summary>
        /// Текущее аудио.
        /// </summary>
        private Audio CurrentAudio => playlist[currentIndex];

        /// <summary>
        /// Плейлист.
        /// </summary>
        private string[] Playlist => playlist.Select((a) => a.Name).ToArray();

        /// <summary>
        /// Громкость воспроизведения.
        /// </summary>
        public double Volume 
        {
            get { return mediaPlayer.Volume; }
            set { mediaPlayer.Volume = value; }
        }

        /// <summary>
        /// Прогресс воспроизведения.
        /// </summary>
        public double Position
        {
            get { return mediaPlayer.Position.TotalSeconds; }
            set { mediaPlayer.Position = TimeSpan.FromSeconds(value); }
        }

        /// <summary>
        /// Длительность аудио в секундах.
        /// </summary>
        public double Duration => CurrentAudio.Duration;

        #endregion


        /// <summary>
        /// Метод начинающий воспроизведение.
        /// </summary>
        public void PlaySong()
        {
            Play();
        }

        /// <summary>
        /// Метод ставящий воспроизведение на паузу.
        /// </summary>
        public void PauseSong()
        {
            Pause();
        }
        
        /// <summary>
        /// Метод прекращающий воспроизведение.
        /// </summary>
        public void StopSong()
        {
           Stop();
        }

        /// <summary>
        /// Метод устанавливающий песню.
        /// </summary>
        /// <param name="path"> Путь к песне. </param>
        public void SetSong(string path)
        {
            LoadAudio(path);
            SelectAudio(0);
        }

        /// <summary>
        /// Метод переключающий песню.
        /// </summary>
        /// <param name="param"> Выбор в какую сторону переключать. </param>
        public void SwitchSong(NextOrBack param)
        {
            if (param == NextOrBack.Next)
            {
                SelectAudio(currentIndex + 1);
            }
            else
            {
                SelectAudio(currentIndex - 1);
            }
        }

    }
}
