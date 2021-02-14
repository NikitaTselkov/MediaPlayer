using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ViewModels
{
    public class MainViewModel : Navigation.NavigateViewModel
    {
        readonly AudioControl audioControl = new AudioControl();


        public RelayCommand Play { get; set; }

        public RelayCommand Pause { get; set; }

        public RelayCommand Stop { get; set; }

        // Громкость.
        public double Volume
        {
            get { return audioControl.Volume; }
            set 
            {
                audioControl.Volume = value;
                RaisePropertyChanged();
            }
        }

        // Прогресс воспроизведения.
        public double Position
        {
            get { return audioControl.Position; }
            set
            {
                audioControl.Position = value;
                RaisePropertyChanged();
            }
        }

        //Длительность аудио в секундах.
        public double Duration => audioControl.Duration;


        public MainViewModel()
        {
            Play = new RelayCommand(PlayMethod);

            Pause = new RelayCommand(PauseMethod);

            Stop = new RelayCommand(StopMethod);

            audioControl.SetSong(@"C:\Users\nikit\Desktop\Bullfight.mp3");

            UpdatePosition();
        }

        public void PlayMethod(object param)
        {
            audioControl.PlaySong();
        }

        public void PauseMethod(object param)
        {
            audioControl.PauseSong();
        }

        public void StopMethod(object param)
        {
            audioControl.StopSong();
        }

        /// <summary>
        /// Метод обнавляющий позицию слайдера.
        /// </summary>
        private void UpdatePosition()
        {
            var timer = new Timer() { Interval = 17 };
            timer.Tick += (s, e) =>
            {
                Position = audioControl.Position;
            };

            timer.Start();
        }

    }
}
