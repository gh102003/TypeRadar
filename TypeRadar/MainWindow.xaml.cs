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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TypeRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextSampleViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new TextSampleViewModel();
            
            DataContext = viewModel;
        }

        private void TextBoxTypingTest_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBoxTypingTest = (TextBox)sender;
            if (textBoxTypingTest.Text == "")
            {
                viewModel.TypingTestTimer.Start();
            }
        }

        private void TextBoxTypingTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            // Find mistakes
            int firstErrorIndex = textBox.Text.Length;
            for (int i = 0; i < textBox.Text.Length; i++)
            {
                if (textBox.Text[i] != viewModel.SelectedTextSample.Text[i])
                {
                    firstErrorIndex = i;
                    break;
                }
            }

            string correctText = textBox.Text.Substring(0, firstErrorIndex);
            var correctTextRun = new Run(correctText)
            {
                Foreground = new SolidColorBrush(Colors.Green),
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 255, 0))
            };

            string missingText = viewModel.SelectedTextSample.Text.Substring(firstErrorIndex);
            var missingTextRun = new Run(missingText)
            {
                Foreground = new SolidColorBrush(Colors.Red)
            };

            // Colour mistakes
            textBlockTextSample.Inlines.Clear();
            textBlockTextSample.Inlines.Add(correctTextRun);
            textBlockTextSample.Inlines.Add(missingTextRun);

            // If completed
            if (textBox.Text == viewModel.SelectedTextSample.Text)
            {
                viewModel.TypingTestTimer.Stop();
                double wpm = viewModel.CalculateWpm(out int wordCount);
                MessageBox.Show($"Well done! Your WPM was {wpm:0.0} over {wordCount} words.");
                return;
            }

            // If empty, restore normal binding to SelectedTextSample.Text
            if (String.IsNullOrEmpty(textBox.Text))
            {
                viewModel.TypingTestTimer.Stop();
                textBlockTextSample.SetBinding(TextBlock.TextProperty, new Binding("SelectedTextSample.Text"));
            }
        }

        private void TextSampleUriSource_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void BtnPrevSample_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PrevTextSample();
            e.Handled = true;
        }

        private void BtnNextSample_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NextTextSample();
            e.Handled = true;
        }

        private void BtnRandomSample_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RandomTextSample();
            e.Handled = true;
        }
    }
}
