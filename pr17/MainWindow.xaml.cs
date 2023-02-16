using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;

namespace pr17
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// Логин: admin
    /// Пароль: admin
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int countNumber;
        public static bool correct;
        DispatcherTimer disTimer = new DispatcherTimer();
        int countTime; 
        int kolError = 0;
        public MainWindow()
        {
            InitializeComponent();
            disTimer.Interval = new TimeSpan(0, 0, 1);
            disTimer.Tick += new EventHandler(DisTimer_Tick);
        }
        private void DisTimer_Tick(object sender, EventArgs e)
        {
            if (countTime == 0) // Если 60 секунд закончились, то выводим кнопку для получения нового кода
            {
                BtnGetCode.IsEnabled = true;
                BtnGetCode.Visibility = Visibility.Visible;
                disTimer.Stop();
                tbNewCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbNewCode.Text = "Получить новый код можно будет через " + countTime + " секунд";
            }
            countTime--;
        }
        private void btn_In_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text == "admin")
            {
                if (Password.Password == "admin")
                {
                    Random rnd = new Random();
                    int value = rnd.Next(10000, 99999);
                    string key = Convert.ToString(value);
                    MessageBox.Show("Код для входа:" + key);
                    KeyPro twoAuthentication = new KeyPro(key);
                    twoAuthentication.ShowDialog();
                    Correct correct12 = new Correct();
                    if (correct == true) 
                    {
                        this.Close();
                        correct12.Show();
                    }
                    else 
                    {
                        if (kolError >= 1) 
                        {
                            
                            CAPTCHA captcha = new CAPTCHA();
                            captcha.ShowDialog();
                            if (correct == true)
                            {
                                this.Close();
                                correct12.Show();
                                Authorization.Visibility = Visibility.Collapsed;
                            }
                            else 
                            {
                                MessageBox.Show("Текст введён не верно! Попробуйте ещё раз!");
                                CAPTCHA captchaReplay = new CAPTCHA();
                                captchaReplay.ShowDialog();
                                if (correct == true)
                                {
                                    this.Close();
                                    correct12.Show();
                                    Authorization.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    MessageBox.Show("Вход не удачен");
                                    Login.Text = "";
                                    Password.Password = "";
                                    Login.IsEnabled = true;
                                    Password.IsEnabled = true;
                                    btn_In.Visibility = Visibility.Visible;
                                    BtnGetCode.Visibility = Visibility.Collapsed;
                                    kolError = 0;
                                }
                            }
                        }
                        else 
                        {
                            if (countNumber == 5) 
                            {
                                MessageBox.Show("Введенный код не является верным! ");
                            }
                            btn_In.Visibility = Visibility.Collapsed;
                            BtnGetCode.Visibility = Visibility.Collapsed;
                            Login.IsEnabled = false;
                            Password.IsEnabled = false;
                            countTime = 60; 
                            tbNewCode.Text = "Получить новый код можно будет через " + countTime + " секунд";
                            tbNewCode.Visibility = Visibility.Visible;
                            disTimer.Start();
                            kolError++;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }

        private void BtnGetCode_Click(object sender, RoutedEventArgs e)
        {
            btn_In_Click(sender,e);
        }
    }
}
