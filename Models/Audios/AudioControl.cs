using Catel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Audios
{
    public partial class AudioControl
    {
        #region Props

        /// <summary>
        /// Текущее аудио.
        /// </summary>
        public Audio CurrentAudio => playlists[currentPlaylistTitle][currentIndex];

        /// <summary>
        /// Плейлист.
        /// </summary>
        public string[] Playlist => playlists[currentPlaylistTitle].Select((a) => a.Name).ToArray();

        /// <summary>
        /// Название текущего плейлиста.
        /// </summary>
        public string CurrentPlaylistTitle => currentPlaylistTitle;

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
        /// Метод устанавливающий песню из плейлиста.
        /// </summary>
        public void SetNewSong(string playlistTitle)
        {
            SelectPlaylist(playlistTitle);
            SelectAudio(0);
        }

        /// <summary>
        /// Метод добавления плей листа.
        /// </summary>
        /// <param name="filepath"> Плейлист </param> 
        public void SetNewPlayList(string title, List<Audio> playlist)
        {
            if (!playlists.Keys.Any(a => a == title))
            {
                playlists.Add(title, playlist);
            }
            else
            {
                playlists[title] = playlist;
            }

            SetNewSong(title);
        }

        /// <summary>
        /// Метод добавляющий в плейлист новую песню.
        /// </summary>
        public void UpdatePlaylist(string path, string title)
        {
            LoadAudio(path, title);
        }

        /// <summary>
        /// Метод проверяющий находится ли эта песня в плейлисте.
        /// </summary>
        public bool IsSongExist(string path)
        {
            var name = path.Substring(path.LastIndexOf('\\') + 1).Replace(".mp3", "");

            foreach (var playlist in playlists.Values)
            {
                if (playlist.Any(a => a.Name == name))
                {
                    return true;
                }
            }

            return false;
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
        /// Метод выбора плейлиста.
        /// </summary>
        /// <param name="index"> Название плейлиста. </param>
        public void SelectPlaylist(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException();

            if (playlists.Keys.Any(a => a == title))
            {
                currentIndex = 0;

                currentPlaylistTitle = title;

                SelectAudio(currentIndex);
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
