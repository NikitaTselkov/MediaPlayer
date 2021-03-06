﻿using Models.Audios;
using Models;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using ViewModels.WindowService;

namespace ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        readonly AudioControl audioControl = new AudioControl();

        readonly DataBaseControl dataBaseControl = new DataBaseControl();


        public RelayCommand Play { get; set; }

        public RelayCommand Pause { get; set; }

        public RelayCommand Stop { get; set; }

        public RelayCommand NextSong { get; set; }

        public RelayCommand BackSong { get; set; }

        public RelayCommand SelectSong { get; set; }

        public RelayCommand SelectPlaylist { get; set; }

        public RelayCommand AddPlaylist { get; set; }

        public RelayCommand AddMusic { get; set; }

        public RelayCommand RemoveMusic { get; set; }

        public RelayCommand RemovePlaylist { get; set; }

        public RelayCommand RenamePlaylist { get; set; }

        public RelayCommand PlayOrStop { get; set; }

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

        // Название текущего плейлиста.
        public string CurrentPlaylistTitle
        {
            get { return audioControl.CurrentPlaylistTitle; }
            set
            {
                RaisePropertyChanged();
            }
        }

        // Назавание нового плейлиста.
        private string titleNewPlaylist;
        public string TitleNewPlaylist
        {
            get { return titleNewPlaylist; }
            set
            {
                titleNewPlaylist = value;
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

        // Запущена ли песня.
        private bool isPlaySong;
        public bool IsPlaySong
        {
            get { return isPlaySong; }
            set
            {
                isPlaySong = value;
                RaisePropertyChanged();
            }
        }

        // Плейлист.
        public string[] Playlist
        {
            get {return audioControl.Playlist; }
            set
            {
                RaisePropertyChanged();
            }
        }

        // Плейлисты.
        public string[] Playlists
        {
            get { return audioControl.Playlists; }
            set 
            {
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            Play = new RelayCommand(PlayMethod);

            Pause = new RelayCommand(PauseMethod);

            Stop = new RelayCommand(StopMethod);

            NextSong = new RelayCommand(NextSongMethod);

            BackSong = new RelayCommand(BackSongMethod);

            SelectSong = new RelayCommand(SelectSongMethod);

            SelectPlaylist = new RelayCommand(SelectPlaylistMethod);

            AddPlaylist = new RelayCommand(AddPlaylistMethod);

            AddMusic = new RelayCommand(AddMusicMethod);

            RemoveMusic = new RelayCommand(RemoveMusicMethod);

            RemovePlaylist = new RelayCommand(RemovePlaylistMethod);

            RenamePlaylist = new RelayCommand(RenamePlaylistMethod);

            PlayOrStop = new RelayCommand(PlayOrStopMethod);

            UpdatePosition();
        }

        public void PlayMethod(object param)
        {
            audioControl.PlaySong();

            IsPlaySong = true;
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
            if (param != null)
            {
                audioControl.SelectSong(param);

                Update();
            }          
        }

        public void PlayOrStopMethod(object param)
        {
            if (IsPlaySong == true)
            {
                audioControl.PauseSong();
            }
            else
            {
                audioControl.PlaySong();
            }

            IsPlaySong = !IsPlaySong;
        }

        public void SelectPlaylistMethod(object param)
        {
            if (param == null) param = audioControl.CurrentPlaylistTitle;

            audioControl.StopSong();

            audioControl.SelectPlaylist(param.ToString());

            Update();
        }

        /// <summary>
        /// Метод добавляющий плейлист.
        /// </summary>
        public void AddPlaylistMethod(object param)
        {
            var newPlaylistViewModel = new PlaylistControlViewModel();

            DisplayRootRegistry.ShowModalPresentation(newPlaylistViewModel);

            if (newPlaylistViewModel.NewPlaylist.Count != 0 && newPlaylistViewModel.Result == true)
            {
                var newPlaylist = new List<Audio>();

                TitleNewPlaylist = newPlaylistViewModel.TitleNewPlaylist;

                if (TitleNewPlaylist == null || TitleNewPlaylist == "")
                {
                    TitleNewPlaylist = "New Title";
                }
                while (audioControl.IsPlaylistExist(TitleNewPlaylist))
                {
                    TitleNewPlaylist += 1;
                }
                foreach (var item in newPlaylistViewModel.NewPlaylist)
                {
                    if (!audioControl.IsSongExist(item.SourceUrl))
                    {
                        newPlaylist.Add(item);
                    }
                }
                if (newPlaylist.Count != 0)
                {
                    // Сохраняет песню в базу данных.
                    dataBaseControl.SaveFileToDatabase(newPlaylist, TitleNewPlaylist);

                    audioControl.SetNewPlayList(TitleNewPlaylist, newPlaylist);

                    Update();
                }
            }
        }

        /// <summary>
        /// Метод добавляет музыку через окно с выбором папок.
        /// </summary>
        public void AddMusicMethod(object param)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = "mp3";
            openFileDialog.Filter = "mp3 files (*.mp3)|*.mp3";

            var result = openFileDialog.ShowDialog();

            // Получает абсолютный путь к mp3 файлу.
            var path = openFileDialog.FileName;

            if (result == DialogResult.OK)
            {
                // Если в плейлисте нет этой песни.
                if (!audioControl.IsSongExist(path))
                {
                    // Сохраняет песню в базу данных.
                    dataBaseControl.SaveFileToDatabase(path, CurrentPlaylistTitle);

                    // Добавляет песню в текущий плейлист.
                    audioControl.UpdatePlaylist(path, audioControl.CurrentPlaylistTitle);

                    Update();
                }
            }
        }

        /// <summary>
        /// Метод удаляющий музыку из плейлиста.
        /// Если плейлист закончился, удаляется и он.
        /// </summary>
        public void RemoveMusicMethod(object param)
        {         
            // Подтверждение на удаление.
            if (GetConfirmationForDeletion())
            {
                var name = audioControl.CurrentAudio.Name;

                if (audioControl.RemoveSongFromPlaylist())
                {
                    dataBaseControl.RemoveFileFromDatabase(name);
                }

                Update();
            }
        }

        /// <summary>
        /// Метод удаляющий плейлист.
        /// </summary>
        public void RemovePlaylistMethod(object param)
        {
            // Подтверждение на удаление.
            if (GetConfirmationForDeletion())
            {
                dataBaseControl.RemoveFileFromDatabase(audioControl.CurrentPlaylistTitle, SongOrPlaylist.Playlist);

                audioControl.RemovePlaylist();

                Update();
            }
        }

        /// <summary>
        /// Метод меняющий название текущему плейлисту.
        /// </summary>
        public void RenamePlaylistMethod(object param)
        {
            RenamePlaylistViewModel renamePlaylistViewModel = new RenamePlaylistViewModel();

            DisplayRootRegistry.ShowModalPresentation(renamePlaylistViewModel);

            if (renamePlaylistViewModel.NewTitle != null && renamePlaylistViewModel.NewTitle != "")
            {
                if (!audioControl.IsPlaylistExist(renamePlaylistViewModel.NewTitle))
                {
                    audioControl.RenameCurrentPlaylist(renamePlaylistViewModel.NewTitle);

                    Update();
                }
            }
        }

        public void NextSongMethod(object param)
        {
            audioControl.SwitchSong(NextOrBack.Next);

            audioControl.PlaySong();

            Update();

            IsPlaySong = true;
        }

        public void BackSongMethod(object param)
        {
            audioControl.SwitchSong(NextOrBack.Back);

            audioControl.PlaySong();

            Update();

            IsPlaySong = true;
        }

        private void Update()
        {
            Playlists = Playlists;
            CurrentPlaylistTitle = CurrentPlaylistTitle;
            Playlist = Playlist;
            CurrentSongName = CurrentSongName;
            Duration = Duration;
            IsPlaySong = false;
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

                // Запускает следующую песню если условие true.
                if (IsPlayNextSong && audioControl.IsSongEnded())
                {
                    PlayNextSong();

                    IsPlaySong = true;
                }
            };

            timer.Start();
        }

        private void PlayNextSong()
        {
            audioControl.SwitchSong(NextOrBack.Next);

            audioControl.PlaySong();

            Update();

            IsPlaySong = true;
        }

        /// <summary>
        /// Метод получающий подтверждение на удаление.
        /// </summary>
        /// <returns> Результат запроса true или false. </returns>
        private bool GetConfirmationForDeletion()
        {
            var confirmationForDeletionViewModel = new ConfirmationForDeletionViewModel();

            DisplayRootRegistry.ShowModalPresentation(confirmationForDeletionViewModel);

            return confirmationForDeletionViewModel.Result;
        }

    }
}
