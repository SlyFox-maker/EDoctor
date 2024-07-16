using EDoctor.Controllers;
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

        private ControllerPatientDataBase controllerPatientDB;
        public MainWindow(String login)
        {
            InitializeComponent();

            controllerPatientDB = new ControllerPatientDataBase();
            //Registration of buttons from navigation panel

            foreach (var child in navigatorPanel.Children)
            {
                if (child is Button button)
                {
                    button.Click += navigationInPanelClick;

                }
            }

            keyValueNavigation.Add("tagButtonPacientes", tabPacientes);
            keyValueNavigation.Add("tagButtonAgenda", tabAgenda);
            keyValueNavigation.Add("tagButtonsNotas", tabNotas);
            keyValueNavigation.Add("tagButtonsCitas", tabCitas);
            keyValueNavigation.Add("tagButtonsSolicitudDelLabarotorio", null);
            keyValueNavigation.Add("tagButtonSolicitudDelRX", null);
            keyValueNavigation.Add("tagButtonsMDA", null);
            keyValueNavigation.Add("tagButtonsCertificadoMedico", null);
            keyValueNavigation.Add("tagButtonsFormatos", null);
            keyValueNavigation.Add("tagButtonsReferencias", null);

            //Getting statistic of patients
            List<int> statisticPatient = controllerPatientDB.getStatisticOfPatients();
            Pacientes_TotalDePacientes.Content = statisticPatient.ElementAt(0);
            Pacientes_TotalDePacientesFemenino.Content = statisticPatient.ElementAt(1);
            Pacientes_TotalDePacientesMasculino.Content = statisticPatient.ElementAt(2);

        }

        private void navigationInPanelClick(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();

            if (sender is Button clickedButton)
            {

                var panel = keyValueNavigation[clickedButton.Name];
                if (panel != null)
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
            Patientes_generalWindow.Visibility = Visibility.Visible;
            Patientes_searchResultWindow.Visibility = Visibility.Collapsed;


        }

        private void newPatientButton_Click(object sender, RoutedEventArgs e)
        {
            clearTagNavigation();

            tabRegistrationOfNewPatient.Visibility = Visibility.Visible;

            String dateNow = DateTime.Now.ToString();
            CreationOfNewPatient_Age.Content = "Fecha de engreso: " + dateNow;
        }

        private void CreationOfNewPatient_DateOfBritch_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null && datePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = datePicker.SelectedDate.Value;

                int age = CalculateAge(selectedDate);
                // Здесь можно использовать выбранную дату, например:

                CreationOfNewPatient_DateOfEntry.Content = "Edad: " + age.ToString();
            }
        }

        private static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        private void CreationOfNewPatient_CreateNewPatient_Click(object sender, RoutedEventArgs e)
        {
            //Takes all date necesarry 
            String fullName = CreationOfNewPatient_FullName.Text.ToString();
            String surnamePaterno = CreationOfNewPatient_SurnamePaterno.Text.ToString();
            String surnameMaterno = CreationOfNewPatient_SurnameMaterno.Text.ToString();

            DateTime dateOfBritch = CreationOfNewPatient_DateOfBritch.SelectedDate.Value;
            String sex = CreationOfNewPatient_Sex.Text;


            String numberPhone = CreationOfNewPatient_NomberPhone.Text.ToString();
            String email = CreationOfNewPatient_Email.Text.ToString();

            String city = CreationOfNewPatient_City.Text.ToString();
            String address = CreationOfNewPatient_Address.Text.ToString();
            String postalCode = CreationOfNewPatient_PostalCode.Text.ToString();

            Debug.WriteLine(sex);

            //Checking necesary dates
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(surnamePaterno) || string.IsNullOrWhiteSpace(surnamePaterno) || dateOfBritch== null || string.IsNullOrWhiteSpace(sex))
            {
                MessageBox.Show("No han llenado todos los datos necesarios");
                return;
            }

            if(controllerPatientDB.createNewPatient(fullName, surnamePaterno, surnameMaterno, dateOfBritch, sex, address, city,numberPhone,postalCode, email))
            {
                MessageBox.Show("¡Nuevo paciente ha creado con exito!");
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error al crear un nuevo paciente");
            }
        }


        private Grid createResultSearchSlot(String fullName, DateTime dateOfBirth, String sex, int age, int id)
        {
            String colorOfBorderSex = "";
            String imageSex = "";
            if (sex == "Hombre")
            {
                colorOfBorderSex = "#698ed5";
                imageSex = "manPatientImage";
            }
            else
            {
                colorOfBorderSex = "#dd5790";
                imageSex = "womanPatientImage";
            }
            // Создаем Grid
            Grid grid = new Grid
            {
                Margin = new Thickness(10, 5, 10, 5),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5CCCCC")),
                VerticalAlignment = VerticalAlignment.Center
            };

            // Определяем колонки
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Создаем Border
            Border border = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorOfBorderSex)),
                Width = 4,
                Height = 80
            };
            Grid.SetColumn(border, 0);
            grid.Children.Add(border);

            // Создаем Image
            Image image = new Image
            {
                Source = (ImageSource)FindResource(imageSex),
                Width = 32,
                Height = 32,
                Margin = new Thickness(10, 0, 0, 0)
            };
            Grid.SetColumn(image, 1);
            grid.Children.Add(image);

            // Создаем StackPanel для текста
            StackPanel textStackPanel = new StackPanel
            {
                Margin = new Thickness(10, 0, 10, 0)
            };
            Grid.SetColumn(textStackPanel, 2);

            // Создаем Label для имени
            Label nameLabel = new Label
            {
                Margin = new Thickness(0, 15, 0, 0),
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Content = fullName
            };
            textStackPanel.Children.Add(nameLabel);

            // Создаем StackPanel для деталей
            StackPanel detailsStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            textStackPanel.Children.Add(detailsStackPanel);

            // Создаем Label для даты рождения
            Label birthDateLabel = new Label
            {
                Margin = new Thickness(0, 0, 0, 0),
                FontSize = 10,
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Fecha de nacimiento: " + dateOfBirth.ToString("dd.MM.yyyy")
            };
            detailsStackPanel.Children.Add(birthDateLabel);

            // Создаем Label для возраста
            Label ageLabel = new Label
            {
                Margin = new Thickness(10, 0, 0, 0),
                FontSize = 10,
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Edad: " + age.ToString()
            };
            detailsStackPanel.Children.Add(ageLabel);

            // Создаем Label для ID
            Label idLabel = new Label
            {
                Margin = new Thickness(10, 0, 0, 0),
                FontSize = 10,
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Id: " + id.ToString()
            };
            detailsStackPanel.Children.Add(idLabel);

            grid.Children.Add(textStackPanel);

            // Создаем Button
            Button button = new Button
            {
                Margin = new Thickness(20, 0, 10, 0),
                Width = 100,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00CC00")),
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Entrar"
            };
            Grid.SetColumn(button, 3);
            grid.Children.Add(button);

            // Добавляем созданный Grid в корневой элемент окна или контейнера

            return grid;
        }

        private void Patientes_SearchButton_Click(object sender, RoutedEventArgs e)
        {
            String nameForSearch = Patientes_SearchPatientesTextBox.Text;

            List<List<Object>> result = controllerPatientDB.getResultOfSearchPatient(nameForSearch);
            if (result == null)
            {
                MessageBox.Show("Patiente no fue encontrado");
                return;
            }
            Patientes_generalWindow.Visibility=Visibility.Collapsed;
            Patientes_searchResultWindow.Visibility = Visibility.Visible;
            Patientes_searchResultWindow.Children.Clear();

            foreach (List<Object> patient in result)
            {
                int id = (int)patient.ElementAt(0);
                String fullName = (String)patient.ElementAt(1);
                DateTime birthDate = (DateTime)patient.ElementAt(2);
                String sex = (String)patient.ElementAt(3);

                int age = CalculateAge(birthDate);

                Grid patientSlot = createResultSearchSlot(fullName, birthDate, sex, age, id);

                Debug.WriteLine(birthDate);

                Patientes_searchResultWindow.Children.Add(patientSlot);
            }
        }

    }
}
