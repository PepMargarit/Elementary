using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;


namespace Elementary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Element> elements;
        private string _learningModeText;
        private string _startQuizText;
        private string _clickInfoText;
        private string _leyendaText;
        private bool _isEnglish=false;
        private Element element = null;

        private IEnumerable<KeyValuePair<string, SolidColorBrush>> _categoryColors;
        public IEnumerable<KeyValuePair<string, SolidColorBrush>> CategoryColors
        {
            get { return _categoryColors; }
            set
            {
                _categoryColors = value;
                OnPropertyChanged(); // Notificar a la UI que cambió
            }
        }

        public bool isEnglish
        {
            get { return _isEnglish; }
            set { _isEnglish = value; }
        }
        public string LearningModeText
        {
            get => _learningModeText;
            set { _learningModeText = value; OnPropertyChanged(); }
        }

        public string StartQuizText
        {
            get => _startQuizText;
            set { _startQuizText = value; OnPropertyChanged(); }
        }

        public MainWindow()
        {
            InitializeComponent();
            // CategoryColors = CategoryToColorConverter.GetCategoryColors();
            DataContext = this;
            CategoryToColorConverter.LoadColorsFromJson("Data/colors_es.json");
            CategoryColors = CategoryToColorConverter.GetCategoryColors();
            SetSpanishText();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SetSpanishText()
        {
            LearningModeText = "Modo Aprendizaje";
            StartQuizText = "Iniciar Quiz";
            ClickInfoText = "Clica en el elemento para información";            
            Leyenda = "Leyenda";
            isEnglish = false;
            CategoryToColorConverter.LoadColorsFromJson("Data/colors_es.json");
            CategoryColors = CategoryToColorConverter.GetCategoryColors();
            OnPropertyChanged(nameof(CategoryColors)); 
            LoadElements(isEnglish);
            DisplayElements();
            if (element!=null)
            {
                ElementInfoTextBlock.Text = "";
            }
        }

        private void SetEnglishText()
        {
            LearningModeText = "Learning Mode";
            StartQuizText = "Start Quiz";
            ClickInfoText = "Click on element for Info";
            Leyenda = "Legend";
            isEnglish = true;
            CategoryToColorConverter.LoadColorsFromJson("Data/colors_en.json");
            CategoryColors = CategoryToColorConverter.GetCategoryColors();
            OnPropertyChanged(nameof(CategoryColors)); 
            LoadElements(isEnglish);
            DisplayElements();
            if (element != null)
            {
                ElementInfoTextBlock.Text = "";
            }
        }

        private void SetEnglishLanguage_Click(object sender, RoutedEventArgs e)
        {
            SetEnglishText();
        }

        private void SetSpanishLanguage_Click(object sender, RoutedEventArgs e)
        {
            SetSpanishText();
        }

        public string ClickInfoText
        {
            get => _clickInfoText;
            set { _clickInfoText = value; OnPropertyChanged(); }
        }

        public string Leyenda
        {
            get => _leyendaText;
            set { _leyendaText = value; OnPropertyChanged(); }
        }

        public string Categoria
        {
            get => _clickInfoText;
            set { _clickInfoText = value; OnPropertyChanged(); }
        }

        private void LoadElements(bool languageCode)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string commonPath = Path.Combine(baseDirectory, "Data", "elements.json");
            string languagePath;
            if (languageCode)
            {
                languagePath = Path.Combine(baseDirectory, "Data","elements_en.json");
            }
            else
            {
                languagePath = Path.Combine(baseDirectory, "Data", "elements_es.json");
            }

            if (File.Exists(languagePath) && File.Exists(commonPath)) 
            {

                elements = JsonConvert.DeserializeObject<List<Element>>(File.ReadAllText(commonPath));
                var languageData = JsonConvert.DeserializeObject<List<ElementTranslation>>(File.ReadAllText(languagePath));
                for (int i = 0; i < elements.Count; i++)
                {
                    elements[i].Name = languageData[i].Name;                    
                    
                    elements[i].Category = languageData[i].Category;
                    elements[i].CategoryColor = CategoryToColorConverter.GetBrushForCategory(elements[i].Category);
                }
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
            element = clickedControl.DataContext as Element;
            DisplayElementInfo(element);
        }

        private void DisplayElementInfo(Element element)
        {
            if (isEnglish)
            {
               ElementInfoTextBlock.Text = $"Atomic Number: {element.AtomicNumber}\n" +
                            $"Symbol: {element.Symbol}\n" +
                            $"Name: {element.Name}\n" +
                            $"Atomic Weight: {element.AtomicWeight}\n" +
                            $"Ionization Energy: {element.IonicEnergy}\n" +
                            $"Category: {element.Category}\n" +
                            $"Melting Point: {element.MeltingPoint} °C";


            }
            else
            {
                ElementInfoTextBlock.Text = $"Número Atómico: {element.AtomicNumber}\n" +
                                        $"Símbolo: {element.Symbol}\n" +
                                        $"Nombre: {element.Name}\n" +
                                        $"Peso Atómico: {element.AtomicWeight}\n" +
                                        $"Energia Ionización: {element.IonicEnergy}\n" +
                                        $"Categoría: {element.Category}\n" +
                                        $"Punto de Fusión: {element.MeltingPoint} °C";
            }
            
        }

        private void LearningMode_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para el modo de aprendizaje
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para iniciar el quiz
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}