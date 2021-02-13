using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class MainViewModel : Navigation.NavigateViewModel
    {
        readonly AudioControl audioControl = new AudioControl();


        public RelayCommand Play { get; set; }

        public RelayCommand Pause { get; set; }

        public RelayCommand Stop { get; set; }


        public MainViewModel()
        {
            Play = new RelayCommand(PlayMethod);

            Pause = new RelayCommand(PauseMethod);

            Stop = new RelayCommand(StopMethod);

            audioControl.SetSong(@"C:\Users\nikit\Desktop\Bullfight.mp3");
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

    }
}
