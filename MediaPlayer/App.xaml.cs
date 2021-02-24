using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModels.WindowService;
using ViewModels;
using System.ComponentModel;
using MediaPlayer.Windows;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainViewModel mainVM;

        public App()
        {
            DisplayRootRegistry.RegisterWindowType<MainViewModel, MainWindow>();
            DisplayRootRegistry.RegisterWindowType<PlaylistControlViewModel, PlaylistControlDialog>();           
            DisplayRootRegistry.RegisterWindowType<ConfirmationForDeletionViewModel, ConfirmationForDeletionDialog>();                    
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RunProgramLogic();
        }


        private void RunProgramLogic()
        {
            mainVM = new MainViewModel();

            DisplayRootRegistry.ShowPresentation(mainVM);          
        }
    }
}
