using GalaSoft.MvvmLight;
using Models;
using Models.Audios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ViewModels.WindowService;

namespace ViewModels
{
    public class PlaylistControlViewModel : ViewModelBase
    {
        private readonly MediaPlayer mediaPlayer = new MediaPlayer();
        private List<Audio> playlist = new List<Audio>();

        public List<Audio> NewPlaylist = new List<Audio>();

        // Громкость.
        public double Volume
        {
            get { return mediaPlayer.Volume; }
            set
            {
                mediaPlayer.Volume = value;
                RaisePropertyChanged();
            }
        }

        // Назавание песни или группы.
        private string titleSongOrGroup;
        public string TitleSongOrGroup
        {
            get { return titleSongOrGroup; }
            set
            {
                titleSongOrGroup = value;
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

        // Плейлист.
        private string[] _playlist;
        public string[] Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;
                RaisePropertyChanged();
            }
        }

        // Прогресс воспроизведения.
        public double Position
        {
            get { return mediaPlayer.Position.TotalSeconds; }
            set
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(value);
                RaisePropertyChanged();
            }
        }

        // Длительность аудио в секундах.
        private double duration;
        public double Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RaisePropertyChanged();
            }
        }

        // Название текущей песни.
        private string currentSongName;
        public string CurrentSongName
        {
            get { return currentSongName; }
            set
            {
                currentSongName = value;
                RaisePropertyChanged();
            }
        }

        // Добавлять все песни в плейлист или нет.
        public bool isSelectedAll;
        public bool IsSelectedAll
        {
            get { return isSelectedAll; }
            set
            {
                isSelectedAll = value;
                RaisePropertyChanged();
            }
        }

        // Текущее аудио.
        private Audio CurrentAudio { get; set; }

        public RelayCommand GetMusicFromWeb { get; set; }
        public RelayCommand Play { get; set; }
        public RelayCommand Stop { get; set; }
        public RelayCommand Pause { get; set; }
        public RelayCommand SelectSong { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand CreateNewPlaylist { get; set; }
        public RelayCommand WindowClosing { get; set; }

        public PlaylistControlViewModel()
        {
            GetMusicFromWeb = new RelayCommand(GetMusicFromWebMethod);
            Play = new RelayCommand(PlayMethod);
            Stop = new RelayCommand(StopMethod);
            Pause = new RelayCommand(PauseMethod);
            SelectSong = new RelayCommand(SelectSongMethod);
            Add = new RelayCommand(AddInPlaylistMethod);
            CreateNewPlaylist = new RelayCommand(CreateNewPlaylistMethod);
            WindowClosing = new RelayCommand(WindowClosingMethod);

            mediaPlayer.Volume = 1;
        }

        // Метод срабатывающий при закрытии окна.
        public void WindowClosingMethod(object param)
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
        }

        public void GetMusicFromWebMethod(object param)
        {
            GetAudiosFromWeb(TitleSongOrGroup);

            Playlist = playlist.Select((a) => a.Name).ToArray();

            SelectAudioMethod(Playlist.First());
        }

        /// <summary>
        /// Метод создающий новый плейлист.
        /// </summary>
        public void CreateNewPlaylistMethod(object param)
        {
            if (IsSelectedAll)
            {
                NewPlaylist = playlist;
            }
            if (NewPlaylist.Count != 0)
            {
                DisplayRootRegistry.ClosePresentation(this);
            }
        }

        /// <summary>
        /// Метод добавляющий выбранную песню в плейлист.
        /// </summary>
        public void AddInPlaylistMethod(object param)
        {
            if (CurrentAudio != null)
            {
                NewPlaylist.Add(CurrentAudio);
            } 
        }

        /// <summary>
        /// Метод запускающий выбранную песню.
        /// </summary>
        public void PlayMethod(object param)
        {
            mediaPlayer.Play();
        }

        /// <summary>
        /// Метод останавливающий выбранную песню.
        /// </summary>
        public void StopMethod(object param)
        {
            mediaPlayer.Stop();
        }

        /// <summary>
        /// Метод ставящий выбранную песню на паузу.
        /// </summary>
        public void PauseMethod(object param)
        {
            mediaPlayer.Pause();
        }

        /// <summary>
        /// Метод выбора песни.
        /// </summary>
        public void SelectSongMethod(object param)
        {
            if (param == null) throw new ArgumentNullException();

            SelectAudioMethod(param.ToString());
        }

        /// <summary>
        /// Метод выбора аудио в плейлисте.
        /// </summary>
        /// <param name="index"> Индекс аудио в плейлисте. </param>
        private void SelectAudioMethod(string songName)
        {
            // Находит позицию текущего элемента в списке.
            var index = GetIndex(songName);

            mediaPlayer.Open(new Uri(playlist[index].SourceUrl, UriKind.Relative));

            // Обновляет выбранное аудио.
            CurrentAudio = playlist[index];

            Duration = CurrentAudio.Duration;

            Position = Position;
        }

        private int GetIndex(string songName)
        {
            return playlist.IndexOf(playlist.First(f => f.Name == songName));
        }

        /// <summary>
        /// Метод получающий музыку с сайта.
        /// </summary>
        private void GetAudiosFromWeb(string title)
        {
            if (title == "") throw new ArgumentNullException();

            Parser parser = new Parser(title);

            foreach (var path in parser.DownloadedRefs)
            {
               playlist.Add(new Audio(path));
            }
        }

    }
}
