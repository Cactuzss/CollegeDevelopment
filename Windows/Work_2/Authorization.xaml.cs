using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using TaskFirst;

namespace practice.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        bool isActive = true;
        BackgroundWorker blocker = new BackgroundWorker();

        const String theWord = "employe";

        int counter = 0;

        public Authorization()
        {
            InitializeComponent();

            blocker.DoWork += block;
            //backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            //backgroundWorker1.WorkerReportsProgress = true;

        }

        private void Log_in(object sender, RoutedEventArgs e)
        {
            if (counter == 3) 
            {
                if (!blocker.IsBusy)
                    blocker.RunWorkerAsync(); 

                MessageBox.Show("Rember the account data"); 
                return; 
            }

            if (!isActive) { MessageBox.Show("Wait a minute"); return; }

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

        private void block(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            isActive = false;
            Thread.Sleep(1000 * 60 / 4);
            isActive = true;
            counter = 0;
        }
    }
}
