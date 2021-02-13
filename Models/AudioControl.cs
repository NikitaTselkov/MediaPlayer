using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Models
{
    public partial class AudioControl
    {
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

        public void SetSong(string path)
        {
            LoadAudio(path);
            SelectAudio(0);
        }



    }
}
