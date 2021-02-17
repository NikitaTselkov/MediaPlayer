using System;
using System.Collections.Generic;
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
        private readonly MediaPlayer mediaPlayer;
        private readonly List<Audio> playlist;
        private readonly Dictionary<string, List<Audio>> playlists;  // Список плейлистов.
        private int currentIndex;
        private string currentPlaylistTitle = "Title";

        private bool isSwitchSong;

        public AudioControl()
        {
            mediaPlayer = new MediaPlayer();

            playlist = new List<Audio>();

            // Значение по умолчанию.
            playlists = new Dictionary<string, List<Audio>>
            {
                { currentPlaylistTitle, playlist }
            };

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
        private void LoadAudio(string filepath) => playlist.Add(new Audio(filepath));

        /// <summary>
        /// Метод удаления аудио из плейлиста.
        /// </summary>
        /// <param name="index"> Позиция удоляемого элемента. </param>
        private void RemoveAudio(int index) => playlist.RemoveAt(index);

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
