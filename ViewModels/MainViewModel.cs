using Models;
using System;
using System.Windows.Forms;

namespace ViewModels
{
    public class MainViewModel : Navigation.NavigateViewModel
    {
        readonly AudioControl audioControl = new AudioControl();


        public RelayCommand Play { get; set; }

        public RelayCommand Pause { get; set; }

        public RelayCommand Stop { get; set; }

        public RelayCommand NextSong { get; set; }

        public RelayCommand BackSong { get; set; }

        public RelayCommand SelectSong { get; set; }

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

        // Длительность аудио в секундах.
        public double Duration
        {
            get {return audioControl.Duration; }
            set 
            {
                RaisePropertyChanged();
            }
        }

        // Название текущей песни.
        public string CurrentSongName
        {
            get { return audioControl.CurrentAudio.Name; }
            set
            { 
                RaisePropertyChanged(); 
            }
        }

        // Плейлист.
        public string[] Playlist => audioControl.Playlist;


        public MainViewModel()
        {
            Play = new RelayCommand(PlayMethod);

            Pause = new RelayCommand(PauseMethod);

            Stop = new RelayCommand(StopMethod);

            NextSong = new RelayCommand(NextSongMethod);

            BackSong = new RelayCommand(BackSongMethod);

            SelectSong = new RelayCommand(SelectSongMethod);


            audioControl.SetSong(@"C:\Users\nikit\Desktop\Bullfight.mp3");

            audioControl.SetSong(@"C:\Users\nikit\Desktop\Sometimes You're The Hammer, Sometimes You're The Nail.mp3");

            audioControl.SetSong(@"C:\Users\nikit\Desktop\Sell Your Soul.mp3");

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

        public void SelectSongMethod(object param)
        {
            if (param == null) throw new ArgumentNullException();

            audioControl.SelectSong(param);

            Update();
        }

        public void NextSongMethod(object param)
        {
            audioControl.SwitchSong(NextOrBack.Next);

            Update();
        }

        public void BackSongMethod(object param)
        {
            audioControl.SwitchSong(NextOrBack.Back);

            Update();
        }

        private void Update()
        {
            CurrentSongName = CurrentSongName;
            Duration = Duration;
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
