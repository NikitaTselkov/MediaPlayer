using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;


namespace Models
{
    public partial class AudioControl
    {    
        private readonly MediaPlayer mediaPlayer;
        private readonly List<Audio> playlist;
        private Timer timer;
        private int currentIndex;

        public AudioControl()
        {
            mediaPlayer = new MediaPlayer();

            playlist = new List<Audio>();

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

            if (currentIndex >= playlist.Count)
                currentIndex = 0;

            if (currentIndex < 0)
                currentIndex = playlist.Count - 1;

            mediaPlayer.Open(new Uri(playlist[currentIndex].SourceUrl));

            ProgressChanged?.Invoke(this, Position);
            AudioSelected?.Invoke(this, CurrentAudio);
           // timer.Start();
        }

        /// <summary>
        /// Метод добавления аудио в плейлист из файла.
        /// </summary>
        /// <param name="filepath"> Путь к аудиофайлу. </param> 
        private void LoadAudio(string filepath) => playlist.Add(new Audio(filepath));

        /// <summary>
        /// Метод добавления нескольких аудио в плейлист.
        /// </summary>
        /// <param name="filepaths"> Массив путей к аудио файлам. </param>
        private void LoadAudio(params string[] filepaths)
        {
            foreach (var file in filepaths)
                LoadAudio(file);
        }

        /// <summary>
        /// Метод удаления аудио из плейлиста.
        /// </summary>
        /// <param name="index"> Позиция удоляемого элемента. </param>
        private void RemoveAudio(int index) => playlist.RemoveAt(index);

        /// <summary>
        /// Метод удаления нескольких аудио из плейлиста.
        /// </summary>
        /// <param name="index"> Позиция удоляемых элементов. </param>
        private void RemoveAudio(params int[] indexes)
        {
            foreach (var index in indexes)
                RemoveAudio(index);
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
