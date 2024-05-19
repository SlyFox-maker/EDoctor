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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(String login)
        {
            InitializeComponent();
        }


        private void tagButtonPacientes_Click(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();

            tagButtonPacientes.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5ba2f4"));
            tabPacientes.Visibility = Visibility.Visible;
        }

        private void tagButtonAgenda_Click(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();

            tagButtonAgenda.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5ba2f4"));
            tabAgenda.Visibility = Visibility.Visible;
        }

        private void tagButtonsNotas_Click(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();

            tagButtonsNotas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5ba2f4"));
            tabNotas.Visibility = Visibility.Visible;
        }

        private void tagButtonsCitas_Click(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();

            tagButtonsCitas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5ba2f4"));
            tabCitas.Visibility = Visibility.Visible;
        }



        private void clearTagNavigation()
        {
            tagButtonPacientes.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));
            tagButtonAgenda.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));
            tagButtonsNotas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));
            tagButtonsCitas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a6ab0"));

            tabPacientes.Visibility = Visibility.Collapsed;
            tabAgenda.Visibility = Visibility.Collapsed;
            tabNotas.Visibility = Visibility.Collapsed;
            tabCitas.Visibility = Visibility.Collapsed;



        }
    }
}
