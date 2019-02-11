using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TypeRadar
{
    class TextSampleViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region TextSamples
        public ObservableCollection<TextSample> TextSamples { get; set; }

        private int selectedTextSampleIndex;
        public int SelectedTextSampleIndex
        {
            get => selectedTextSampleIndex;

            set
            {
                if (value >= TextSamples.Count || value < 0)
                {
                    throw new Exception("SelectedTextSampleIndex");
                }

                selectedTextSampleIndex = value;
                // Update selected text sample
                SelectedTextSample = TextSamples[value];
                // Clear textBoxTypingTest
                TypingTestInput = "";

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTextSampleIndex"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTextSample"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypingTestInput"));
            }
        }
        public TextSample SelectedTextSample { get; private set; }
        
        private void LoadTextSamples()
        {
            string json;
            var uri = new Uri("/textSamples.json", UriKind.Relative);
            using (Stream stream = Application.GetResourceStream(uri).Stream)
            {
                StreamReader streamReader = new StreamReader(stream);
                json = streamReader.ReadToEnd();
            }
            List<TextSample> textSamples = JsonConvert.DeserializeObject<List<TextSample>>(json);
            TextSamples = new ObservableCollection<TextSample>(textSamples);

            SelectedTextSampleIndex = 0;
        }

        public void PrevTextSample()
        {
            if (SelectedTextSampleIndex < 1)
            {
                SelectedTextSampleIndex = TextSamples.Count - 1;
            }
            else
            {
                SelectedTextSampleIndex--;
            }
        }

        public void NextTextSample()
        {
            if (SelectedTextSampleIndex + 1 >= TextSamples.Count)
            {
                SelectedTextSampleIndex = 0;
            } else
            {
                SelectedTextSampleIndex++;
            }
        }
        #endregion

        public string TypingTestInput { get; set; }

        public TypingTestTimer TypingTestTimer { get; set; }

        public double CalculateWpm(out int wordCount)
        {
            string[] words = SelectedTextSample.Text.Split(' ');
            wordCount = words.Length;

            return wordCount / TypingTestTimer.TimeElapsed.TotalMinutes;
        }

        public TextSampleViewModel()
        {
            LoadTextSamples();
            TypingTestTimer = new TypingTestTimer(SelectedTextSample);
        }
    }
}
