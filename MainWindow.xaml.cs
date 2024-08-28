using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;


namespace Elementary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Element> elements;
        public IEnumerable<KeyValuePair<string, SolidColorBrush>> CategoryColors { get; }

        public MainWindow()
        {
            InitializeComponent();
            CategoryColors = CategoryToColorConverter.GetCategoryColors();
            DataContext = this;
            LoadElements();
            DisplayElements();
        }

        private void LoadElements()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDirectory, "Data", "elements.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                elements = JsonConvert.DeserializeObject<List<Element>>(json);
            }
            else
            {
                MessageBox.Show("El archivo de elementos no se encuentra.");
                elements = new List<Element>();
            }
        }

        private void DisplayElements()
        {
            PeriodicTableGrid.Children.Clear();
            PeriodicTableGrid.RowDefinitions.Clear();
            PeriodicTableGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 18; i++)
            {
                PeriodicTableGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            
            for (int i = 0; i < 7; i++)
            {
                PeriodicTableGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Añade una fila adicional con altura específica para el espacio
            PeriodicTableGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

            // Añade las filas restantes después del espacio
            for (int i = 0; i < 2; i++)
            {
                PeriodicTableGrid.RowDefinitions.Add(new RowDefinition());
            }

            PeriodicTableGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5) });

            foreach (var element in elements)
            {
                ElementControl elementControl = new ElementControl
                {
                    DataContext = element,
                    Margin = new Thickness(1),
                    Padding = new Thickness(3)
                };

                elementControl.MouseLeftButtonUp += ElementControl_Click;

                int adjustedRow = element.Row;
                if (element.Row >= 7)
                {
                    adjustedRow++; // Ajusta la fila por el espacio añadido
                }

                Grid.SetRow(elementControl, adjustedRow);
                Grid.SetColumn(elementControl, element.Column);

                PeriodicTableGrid.Children.Add(elementControl);
            }
        }
               
        private void ElementControl_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ElementControl clickedControl = sender as ElementControl;
            Element element = clickedControl.DataContext as Element;
            DisplayElementInfo(element);
        }

        private void DisplayElementInfo(Element element)
        {
            ElementInfoTextBlock.Text = $"Número Atómico: {element.AtomicNumber}\n" +
                                        $"Símbolo: {element.Symbol}\n" +
                                        $"Nombre: {element.Name}\n" +
                                        $"Peso Atómico: {element.AtomicWeight}\n" +
                                        $"Energia Ionización: {element.IonicEnergy}\n" +
                                        $"Categoría: {element.Category}\n" +
                                        $"Punto de Fusión: {element.MeltingPoint} °C";
        }

        private void LearningMode_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para el modo de aprendizaje
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para iniciar el quiz
        }
    }
}