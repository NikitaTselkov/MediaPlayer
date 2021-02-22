using GalaSoft.MvvmLight;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModels.WindowService;

namespace ViewModels
{
    public class ConfirmationForDeletionViewModel : ViewModelBase
    {
        private bool result; 
        public bool Result 
        {
            get { return result; }
            set
            {
                result = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GetResult { get; set; }


        public ConfirmationForDeletionViewModel()
        {
            GetResult = new RelayCommand(GetResultMethod);
        }

        public void GetResultMethod(object param)
        {
            if (param == null) throw new ArgumentNullException();

            Result = Convert.ToBoolean(param);

            DisplayRootRegistry.ClosePresentation(this);
        }
    }
}
