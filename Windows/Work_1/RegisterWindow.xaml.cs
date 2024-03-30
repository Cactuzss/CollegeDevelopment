using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Security.Policy;

namespace TaskFirst
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        const string filename = "\\data.txt";
        string datapath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + filename;

        string stringfix(string s)
        {
            StringBuilder sb = new StringBuilder(s.Trim());
            for (int i = 0; i < sb.Length; i++)
                sb[i] = char.ToLower(sb[i]);
            sb[0] = char.ToUpper(sb[0]);
            return sb.ToString();
        }

        bool checkEmail(string str)
        {
            if (str.Length == 0) return false;
            if ((str[0] < 'a' && str[0] > 'z') || (str[0] < 'A' && str[0] > 'Z')) return false;

            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9_]*@[a-zA-Z][a-zA-Z0-9_]*\\.[a-zA-Z]([a-zA-Z0-9_]*|$)$");
            MatchCollection matchCollection = regex.Matches(str);

            if (matchCollection.Count == 1)
                return true;
            return false;
        }

        bool checkPhone(string str)
        {
            if (str.Length == 0) return false;

            for (int i = 1; i < str.Length; i++)
                if (str[i] < '0' || str[i] > '9')
                    return false;

            if (str[0] == '+' && str[1] == '7')
                return str.Length == 12;
            else if (str[0] == '8' )
                return str.Length == 11;
            return false;
        }

        bool checkPassport(string series, string number)
        {
            if (series[0] != '6' || series[1] != '3') return false;
            if (series.Length != 4 || number.Length != 6) return false;
            return true;
        }

        bool checkID(string id)
        {
            using (StreamReader sr = new StreamReader(datapath))
            {
                if (id.Length == 0) return false;
                Regex regex = new Regex("^\\d+(\\t| )");

                string line = sr.ReadLine();
                StringBuilder sb = new StringBuilder();

                while (line != null)
                {
                    MatchCollection matchCollection = regex.Matches(line);

                    if (matchCollection.Count < 1) return false;

                    sb = new StringBuilder(matchCollection[0].Value);
                    sb.Length--;

                    if (sb.ToString() == id) return false;

                    line = (string)sr.ReadLine();
                }

                foreach (var c in id) if (Char.IsLetter(c)) return false;
                return true;
            }
        }

        bool checkName(TextBox box)
        {
            if (box.Text.Length == 0) return false;
           
            box.Text = stringfix(box.Text);

            Regex regex = new Regex("[а-яА-Я]+");

            return regex.Matches(box.Text).Count != 0;
        }

        void changeFocus(TextBox to)
        {
            to.Focus();
            to.Text = to.Text.Trim();
        }

        public RegisterWindow()
        {
            if (!File.Exists(datapath))
                File.Create(datapath);

            InitializeComponent();
        }


        private void Register_Button(object sender, RoutedEventArgs e)
        {
            idBox.Text = idBox.Text.Trim();
            lastName.Text = lastName.Text.Trim();
            firstName.Text = firstName.Text.Trim();
            secondName.Text = secondName.Text.Trim();
            passportSeries.Text = passportSeries.Text.Trim();
            passportNumber.Text = passportNumber.Text.Trim();
            phone.Text = phone.Text.Trim();
            email.Text = email.Text.Trim();

            if (!checkID(idBox.Text))
            { MessageBox.Show("ID must be unique and numeric!"); return; }
            if (!checkName(lastName) || !checkName(firstName) || !checkName(secondName))
            { MessageBox.Show("Name must be written with сyrillic"); return; }
            if (!checkPassport(passportSeries.Text, passportNumber.Text))
            { MessageBox.Show("Incorrect passport data format"); return; }
            if (!checkPhone(phone.Text))
            { MessageBox.Show("Phone number must be in +7xxxxxxxxxx or 8xxxxxxxxxx format"); return; }
            if (!checkEmail(email.Text))
            { MessageBox.Show("Email must be in mail@domen.ext"); return; }

            File.AppendAllText(datapath, idBox.Text + "\t" +
                                         lastName.Text + " " +
                                         firstName.Text + " " +
                                         secondName.Text + "\t" +
                                         passportSeries.Text + " " +
            passportNumber.Text + "\t" +
                                         phone.Text + "\t" +
                                         email.Text + "\n");

            MessageBox.Show("Success");
        }

        private void lastName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space) changeFocus(firstName);
        }

        private void firstName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space) changeFocus(secondName);
        }

        private void secondName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space) changeFocus(passportSeries);
        }

        private void idBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space) changeFocus(lastName);
        }

        private void passportSeries_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space || passportSeries.Text.Length == 5) changeFocus(passportNumber);
        }

        private void passportNumber_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space || passportNumber.Text.Length == 7) changeFocus(phone);
        }

        private void phone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space) changeFocus(email);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            idBox.Clear();
            firstName.Clear();
            secondName.Clear();
            lastName.Clear();
            phone.Clear();
            email.Clear();
            passportSeries.Clear();
            passportNumber.Clear();
        }
    }
}