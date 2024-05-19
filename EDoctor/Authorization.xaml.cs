using EDoctor.Controllers;
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
using System.Windows.Shapes;

namespace EDoctor
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    ///https://colorscheme.ru/#0041Tw0w0w0w0
    public partial class Authorization : Window
    {
        private ControllerDoctorDataBase controllerDDataBase;
        public Authorization()
        {
            InitializeComponent();

            controllerDDataBase = new ControllerDoctorDataBase();
        }

        private void tagEntrar_Click(object sender, RoutedEventArgs e)
        {
            StackPanelEntrar.Visibility = Visibility.Visible;
            StackPanelRegistrarse.Visibility= Visibility.Collapsed;

            windowAuthorization.Height = 300;

            tagEntrar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c9bbaa"));
            tagRegistrarse.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
        }

        private void tagRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            StackPanelEntrar.Visibility = Visibility.Collapsed;
            StackPanelRegistrarse.Visibility = Visibility.Visible;

            windowAuthorization.Height = 470;

            tagRegistrarse.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c9bbaa"));
            tagEntrar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));

        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            String email = textBoxEmail.Text;
            String phone = textBoxPhone.Text;
            String password = passwordBoxPassword.Password.ToString();
            String passwordConfirm = passwordBoxPasswordConfirm.Password.ToString();
            String fullName = textBoxFullName.Text;
            String CURP = textBoxCURP.Text;

            if (password != passwordConfirm)
            {
                MessageBox.Show("Sus contraseñas no concuerdan");
                return;
            }

            if (controllerDDataBase.createNewUser(fullName, CURP, phone, email, password))
            {
                MainWindow MW = new MainWindow(email);
                MW.Show();

                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String login = lLogin.Text;
            String password = lPassword.Password.ToString();

            if(controllerDDataBase.login(login, password)) {
                MainWindow MW = new MainWindow(login);
                MW.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("La autorización no ha sido autorizada.");
            }
        }
    }
}
