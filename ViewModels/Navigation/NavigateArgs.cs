using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Navigation
{
    public class NavigateArgs : EventArgs
    {
        public NavigateArgs()
        {

        }

        public NavigateArgs(string url)
        {
            Url = url;
        }

        public string Url { get; }


        public override string ToString()
        {
            return $"Url: {Url}.";
        }
    }
}
