using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace KontrolnayaWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Maslov_BaklanovEntities db = new Maslov_BaklanovEntities();
        List<AnalyzesTable> analysesInDB = new List<AnalyzesTable>();
        List<AnalyzesTable> analysesInTable = new List<AnalyzesTable>();

        List<ServicesTable> servicesTable = new List<ServicesTable>();
        List<ServicesTable> servicesDB = new List<ServicesTable>();
        public MainWindow()
        {
            InitializeComponent();

            //ResizeMode = ResizeMode.NoResize;

            analysesInDB = db.AnalyzesTable.ToList();

            List<string> analysesNames = new List<string>();

            foreach (AnalyzesTable analys in analysesInDB)
            {
                analysesNames.Add(analys.AnalysName);
            }
            ComboBoxAnalyses.ItemsSource = analysesNames;

            servicesDB = db.ServicesTable.ToList();
            List<string> servicesNames = new List<string>();
            foreach(ServicesTable service in servicesDB)
            {
                servicesNames.Add(service.ServiceName);
            }
            ComboBoxServices.ItemsSource = servicesNames;
        }
        ObservableCollection<Analys> selectedAnalyses = new ObservableCollection<Analys>();
        private void AddAnalys(object sender, RoutedEventArgs e)
        {
            // Получение выбранного элемента из ComboBox
            string selectedAnalysisName = ComboBoxAnalyses.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedAnalysisName))
            {
                bool isAlreadyAdded = selectedAnalyses.Any(a => a.AnalysName == selectedAnalysisName);

                if (!isAlreadyAdded)
                {
                    AnalyzesTable selectedAnalysis = db.AnalyzesTable.FirstOrDefault(a => a.AnalysName == selectedAnalysisName);

                    if (selectedAnalysis != null)
                    {
                        Analys newAnalys = new Analys
                        {
                            id = selectedAnalysis.id,
                            AnalysName = selectedAnalysis.AnalysName,
                            AnalysPrice = selectedAnalysis.AnalysPrice
                        };

                        bool isNameAlreadyAdded = selectedAnalyses.Any(a => a.AnalysName == newAnalys.AnalysName);

                        if (!isNameAlreadyAdded)
                        {
                            selectedAnalyses.Add(newAnalys);
                            totalAnalysesPrice += selectedAnalysis.AnalysPrice;

                            UpdateTotalPriceAll();
                        }
                    }
                }
            }

            AnalysesDG.ItemsSource = selectedAnalyses;
            UpdateTotalPrice();
            UpdateTotalPriceAnalys();
        }


        public class Analys
        {
            public int id { get; set; }
            public string AnalysName { get; set; }
            public decimal AnalysPrice { get; set; }


        }
        public class Service
        {
            public int id { get; set; }
            public string ServiceName { get; set; }

            public decimal ServicePrice { get; set; }
        }

        private void RemoveRow(object sender, RoutedEventArgs e)
        {
            Analys selectedAnalys = AnalysesDG.SelectedItem as Analys;

            if (selectedAnalys != null)
            {
                // Удаление выбранной строки из источника данных
                selectedAnalyses.Remove(selectedAnalys);
                totalAnalysesPrice -= selectedAnalys.AnalysPrice;

                UpdateTotalPriceAll();
            }
            UpdateTotalPriceAnalys();
            
        }

        private ObservableCollection<Service> selectedServices = new ObservableCollection<Service>();

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранного элемента из ComboBox
            string selectedServiceName = ComboBoxServices.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedServiceName))
            {
                // Проверка, чтобы выбранный элемент не повторялся в коллекции selectedServices
                bool isAlreadyAdded = selectedServices.Any(s => s.ServiceName == selectedServiceName);

                if (!isAlreadyAdded)
                {
                    // Поиск выбранного сервиса в базе данных
                    ServicesTable selectedService = db.ServicesTable.FirstOrDefault(s => s.ServiceName == selectedServiceName);

                    
                    if (selectedService != null)
                    {
                        // Создание нового объекта Service и добавление его в коллекцию selectedServices
                        Service newService = new Service
                        {
                            id = selectedService.id,
                            ServiceName = selectedService.ServiceName,
                            ServicePrice = selectedService.ServicePrice

                        };
                        totalServicesPrice += selectedService.ServicePrice;

                        UpdateTotalPriceAll();

                        selectedServices.Add(newService);
                        
                    }
                }
            }

            // Обновление источника данных DataGrid
            DataGridServices.ItemsSource = selectedServices;
            UpdateTotalPrice();
            
        }

        private void BtnRemoveServices_Click(object sender, RoutedEventArgs e)
        {
            Service selectedService = DataGridServices.SelectedItem as Service;

            if (selectedService != null)
            {
                // Удаление выбранной строки из коллекции selectedServices
                selectedServices.Remove(selectedService);

                // Обновление источника данных DataGrid
                DataGridServices.ItemsSource = null;
                DataGridServices.ItemsSource = selectedServices;
                totalServicesPrice -= selectedService.ServicePrice;

                UpdateTotalPriceAll();
            }
            UpdateTotalPrice();
           
        }

        private void UpdateTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (Service service in selectedServices)
            {
                totalPrice += service.ServicePrice;
            }

            // Обновление значения в TextBlock
            TextBlockTotalPriceService.Text = $"Сумма всех услуг: {totalPrice}"; // Замените TextBlockTotalPrice на имя вашего TextBlock
        }

        private void UpdateTotalPriceAnalys()
        {
            decimal totalPrice = 0;

            foreach (Analys analys in selectedAnalyses)
            {
                totalPrice += analys.AnalysPrice;
            }

            // Обновление значения в TextBlock
            TextBlockTotalPriceAnalys.Text = $"Сумма всех анализов: {totalPrice}"; // Замените TextBlockTotalPrice на имя вашего TextBlock
        }

        private decimal totalServicesPrice = 0;
        private decimal totalAnalysesPrice = 0;

        private void UpdateTotalPriceAll()
        {
            decimal totalPrice = totalServicesPrice + totalAnalysesPrice;

            // Обновление значения в TextBlock
            TextBlockTotalPriceAll.Text = $"Общая сумма: {totalPrice}"; // Замените TextBlockTotalPrice на имя вашего TextBlock
        }

        private void TextBox_NameOparacii_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
            textBox.GotFocus -= TextBox_NameOparacii_GotFocus;
        }

        private void TextBoxNumberDocument_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
            textBox.GotFocus -= TextBoxNumberDocument_GotFocus;
        }

        private void TextBoxFIO_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
            textBox.GotFocus -= TextBoxFIO_GotFocus;
        }

        private void TextBoxFIODoctor_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
            textBox.GotFocus -= TextBoxFIODoctor_GotFocus;
        }
    }
}
