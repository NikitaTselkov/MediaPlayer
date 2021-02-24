using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;


namespace Models.Audios
{
    public enum NextOrBack
    { 
        Next,
        Back
    }

    public partial class AudioControl
    {
        private readonly DataBaseControl dataBaseControl = new DataBaseControl();
        private readonly MediaPlayer mediaPlayer;
        private readonly Dictionary<string, List<Audio>> playlists;  // Список плейлистов.
        private int currentIndex;
        private string currentPlaylistTitle;

        private bool isSwitchSong;

        public AudioControl()
        {
            // Получение данных из базы.
            playlists = dataBaseControl.ReadFileFromDatabase();

            currentPlaylistTitle = playlists.Keys.FirstOrDefault();

            if (currentPlaylistTitle == null)
            {
                // Значение по умолчанию.

                Parser parser = new Parser("");

                currentPlaylistTitle = "Title";

                var newPlaylist = new List<Audio>();

                foreach (var path in parser.DownloadedRefs)
                {
                    newPlaylist.Add(new Audio(path));
                }

                playlists.Add(currentPlaylistTitle, newPlaylist);
            }

            mediaPlayer = new MediaPlayer();

            // Громкость по умолжанию.
            Volume = 1;
        }

        /// <summary>
        /// Метод выбора аудио в плейлисте.
        /// </summary>
        /// <param name="index"> Индекс аудио в плейлисте. </param>
        private void SelectAudio(int index)
        {
            currentIndex = index;

            if (currentIndex >= playlists[currentPlaylistTitle].Count)
                currentIndex = 0;

            if (currentIndex < 0)
                currentIndex = playlists[currentPlaylistTitle].Count - 1;

            mediaPlayer.Open(new Uri(playlists[currentPlaylistTitle][currentIndex].SourceUrl, UriKind.Relative));         

            ProgressChanged?.Invoke(this, Position);
            AudioSelected?.Invoke(this, CurrentAudio);
        }

        /// <summary>
        /// Метод добавления аудио в плейлист из файла.
        /// </summary>
        /// <param name="filepath"> Путь к аудиофайлу. </param> 
        /// <param name="title"> Название плейлиста. </param> 
        private void LoadAudio(string filepath, string title) => playlists[title].Add(new Audio(filepath));

        /// <summary>
        /// Метод удаления аудио из плейлиста.
        /// </summary>
        /// <param name="index"> Позиция удоляемого элемента. </param>
        /// <param name="title"> Название плейлиста. </param> 
        private void RemoveAudio(int index, string title) => playlists[title].RemoveAt(index);

        /// <summary>
        /// Метод находящий название предыдущеге плейлиста.
        /// </summary>
        private string FindFreeTitle()
        {
            var previousKey = "";

            if (playlists.Keys.First() == CurrentPlaylistTitle && playlists.Keys.Count > 1)
            {
                return playlists.Keys.Skip(1).First();
            }

            foreach (var key in playlists.Keys)
            {
                if (key == CurrentPlaylistTitle)
                {
                    break;
                }

                previousKey = key;
            }

            return previousKey;
        }

        #region Controls

        /// <summary>
        /// Воспроизвести аудио.
        /// </summary>
        private void Play()
        {
            if (Playlist.Length == 0)
                return;

            mediaPlayer.Play();
        }

        /// <summary>
        /// Приостановить аудио.
        /// </summary>
        private void Pause()
        {
            mediaPlayer.Pause();
        }

        /// <summary>
        /// Остановить аудио.
        /// </summary>
        private void Stop()
        {
            mediaPlayer.Stop();
        }

        #endregion

        #region Events

        /// <summary>
        /// Событие изменения прогресса воспроизведения.
        /// </summary>
        public event Action<object, double> ProgressChanged;

        /// <summary>
        /// Событие выбора аудио из плейлиста.
        /// </summary>
        private event Action<object, Audio> AudioSelected;

        #endregion
    }
}
