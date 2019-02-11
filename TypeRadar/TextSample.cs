using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRadar
{
    public class TextSample : INotifyPropertyChanged
    {
        private string text;
        public string Text {
            get => text;
            set
            {
                if (value == text)
                {
                    return;
                }

                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text")); // Check something is listening
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                {
                    return;
                }

                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); // Check something is listening
            }
        }

        private string author;
        public string Author
        {
            get => author;
            set
            {
                if (value == author)
                {
                    return;
                }

                author = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Author")); // Check something is listening
            }
        }

        private Uri uri;
        public Uri Uri
        {
            get => uri;
            set
            {
                if (value == uri)
                {
                    return;
                }

                uri = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Uri")); // Check something is listening
            }
        }

        public TextSample(string text, string name, string author, Uri uri)
        {
            Text = text;
            Name = name;
            Uri = uri;
            Author = author;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
