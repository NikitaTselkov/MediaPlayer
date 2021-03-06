using System;
using System.Threading;
using System.Timers;
using System.Windows.Controls;


namespace MediaPlayer.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Sticks.xaml
    /// </summary>
    public partial class Sticks : UserControl
    {
        public Sticks()
        {
            InitializeComponent();

            Thread thread = new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() => l1.IsEnabled = true));

                Thread.Sleep(100);

                this.Dispatcher.Invoke(new Action(() => l3.IsEnabled = true));

                Thread.Sleep(50);

                this.Dispatcher.Invoke(new Action(() => l2.IsEnabled = true));
            });

            thread.Start();
        }
    }
}
