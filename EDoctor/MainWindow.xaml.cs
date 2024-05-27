using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<String, StackPanel> keyValueNavigation = new Dictionary<String, StackPanel>();
        public MainWindow(String login)
        {
            InitializeComponent();

            //Registration of buttons from navigation panel

            foreach(var child in navigatorPanel.Children)
            {
                if (child is Button button)
                {
                    button.Click += navigationInPanelClick;
                    
                }
            }

            keyValueNavigation.Add("tagButtonPacientes",tabPacientes);
            keyValueNavigation.Add("tagButtonAgenda",tabAgenda);
            keyValueNavigation.Add("tagButtonsNotas", tabNotas);
            keyValueNavigation.Add("tagButtonsCitas", tabCitas);
            keyValueNavigation.Add("tagButtonsSolicitudDelLabarotorio",null);
            keyValueNavigation.Add("tagButtonSolicitudDelRX", null);
            keyValueNavigation.Add("tagButtonsMDA", null);
            keyValueNavigation.Add("tagButtonsCertificadoMedico", null);
            keyValueNavigation.Add("tagButtonsFormatos", null);
            keyValueNavigation.Add("tagButtonsReferencias", null);

        }

        private void navigationInPanelClick(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();

            if (sender is Button clickedButton)
            {

                var panel = keyValueNavigation[clickedButton.Name];
                if(panel != null)
                {
                    clickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5ba2f4"));
                    panel.Visibility = Visibility.Visible;
                }
            }
        }



        private void clearTagNavigation()
        {
            tagButtonPacientes.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));
            tagButtonAgenda.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));
            tagButtonsNotas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));
            tagButtonsCitas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));


            //Main Panels
            tabPacientes.Visibility = Visibility.Collapsed;
            tabAgenda.Visibility = Visibility.Collapsed;
            tabNotas.Visibility = Visibility.Collapsed;
            tabCitas.Visibility = Visibility.Collapsed;


            //SubPanels
            tabRegistrationOfNewPatient.Visibility = Visibility.Collapsed;


        }

        private void newPatientButton_Click(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();
            
            tabRegistrationOfNewPatient.Visibility = Visibility.Visible;

            String dateNow = DateTime.Now.ToString();
            FechaDeEngresoNewPatientLabel.Content= "Fecha de engreso: " + dateNow;
        }

        private void FechaDeNacimientoNewPatientDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null && datePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = datePicker.SelectedDate.Value;

                int age = CalculateAge(selectedDate);
                // Здесь можно использовать выбранную дату, например:

                EdadNewPatientLabel.Content = "Edad: " + age.ToString();
            }
        }

        private static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
