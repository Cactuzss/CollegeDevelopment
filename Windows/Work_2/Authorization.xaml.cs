using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TaskFirst;

namespace practice.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        const String theWord = "employe";

        int counter = 0;
        int deathtime = 10;

        DispatcherTimer timer;
        DispatcherTimer blocker;

        public Authorization()
        {
            InitializeComponent();

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(deathtime) };
            timer.Tick += DeathTimer;

            timer.Start();

            blocker = new DispatcherTimer { Interval = TimeSpan.FromSeconds(deathtime / 2) };
            blocker.Tick += block;

        }

        private void DeathTimer(object sender, EventArgs e)
        {
            //this.Close();
            MessageBox.Show("Do no forgor about this window");
        }

        private void Window_Activity(object sender, TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();

        }
        private void Window_Activity(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void Log_in(object sender, RoutedEventArgs e)
        {
            Window_Activity(sender, e);

            if (counter == 3) 
            {
                btn.IsEnabled = false;
                blocker.Start();

                return; 
            }


            if (logen.Text == theWord)
            {
                if (passwodr.Password == theWord)
                {
                    var xd = new RegisterWindow();
                    xd.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("The password is incorrect");
                    counter++;
                }
            }
            else
            {
                MessageBox.Show("The logen is incorrect");
                counter++;
            }
        }

        private void block(object sender, EventArgs e)
        {
            btn.IsEnabled = true;
            counter = 0;
        }
    }
}
