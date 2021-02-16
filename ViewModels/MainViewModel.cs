using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ViewModels
{
    public class MainViewModel : Navigation.NavigateViewModel
    {
        readonly AudioControl audioControl = new AudioControl();

        readonly DataBaseControl dataBaseControl = new DataBaseControl();


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

        // Нужно ли запускать следующую песню автоматически.
        private bool isPlayNextSong = true;
        public bool IsPlayNextSong
        {
            get { return isPlayNextSong; }
            set 
            {
                isPlayNextSong = value;
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


            // dataBaseControl.SaveFileToDatabase(@"C:\Users\nikit\Desktop\Sometimes You're The Hammer, Sometimes You're The Nail.mp3");

            // dataBaseControl.SaveFileToDatabase(@"C:\Users\nikit\Desktop\Sell Your Soul.mp3");

            foreach (var item in dataBaseControl.ReadFileFromDatabase())
            {
                audioControl.SetSong(item.SourceUrl);
            }

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
        /// Метод обновляющий позицию слайдера.
        /// </summary>
        private void UpdatePosition()
        {
            var timer = new Timer() { Interval = 17 };
            timer.Tick += (s, e) =>
            {
                Position = audioControl.Position;

                if (IsPlayNextSong && audioControl.IsSongEnded())
                {
                    PlayNextSong();
                }
            };

            timer.Start();
        }

        private void PlayNextSong()
        {
            audioControl.SwitchSong(NextOrBack.Next);

            audioControl.PlaySong();

            Update();
        }

    }
}
