using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Navigation
{
    public class NavigateViewModel : ViewModelBase
    {
        public NavigateViewModel()
        {

        }

        public void Navigate(string url)
        {
            Messenger.Default.Send<NavigateArgs>(new NavigateArgs(url));
        }
    }
}
