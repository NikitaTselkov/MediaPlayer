using Catel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public partial class AudioControl
    {
        #region Props

        /// <summary>
        /// Текущее аудио.
        /// </summary>
        public Audio CurrentAudio => playlist[currentIndex];

        /// <summary>
        /// Плейлист.
        /// </summary>
        public string[] Playlist => playlists[CurrentPlayTitle].Select((a) => a.Name).ToArray();

        /// <summary>
        /// Список плейлистов.
        /// </summary>
        public string[] Playlists => playlists.Select((a) => a.Key).ToArray();

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
            SelectPlaylist("Title");
        }

        /// <summary>
        /// Метод проверяющий закончилась текущая песня или нет.
        /// </summary>
        public bool IsSongEnded()
        {
            return Position == Duration;
        }

        /// <summary>
        /// Метод выбирающий песню.
        /// </summary>
        /// <param name="name"> Название песни. </param>
        public void SelectSong(object name)
        {
            if (isSwitchSong == false)
            {
                if (name == null) throw new ArgumentNullException();

                int index = Playlist.IndexOf(name, 0);

                if (index == -1) throw new ArgumentNullException();

                SelectAudio(index);
            }
            else
            {
                isSwitchSong = false;
            }        
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

            isSwitchSong = true;
        }

    }
}
