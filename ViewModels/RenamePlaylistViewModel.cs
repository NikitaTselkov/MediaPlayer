using GalaSoft.MvvmLight;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModels.WindowService;

namespace ViewModels
{
    public class RenamePlaylistViewModel : ViewModelBase
    {
        // Новое назавание плейлиста.
        private string newTitle;
        public string NewTitle
        {
            get { return newTitle; }
            set
            {
                newTitle = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand Accept { get; set; }
        public RelayCommand Cancel { get; set; }

        public RenamePlaylistViewModel()
        {
            Accept = new RelayCommand(AcceptMethod);
            Cancel = new RelayCommand(CancelMethod);
        }

        public void AcceptMethod(object param)
        {
            DisplayRootRegistry.ClosePresentation(this);
        }

        public void CancelMethod(object param)
        {
            NewTitle = "";

            DisplayRootRegistry.ClosePresentation(this);
        }
    }
}
