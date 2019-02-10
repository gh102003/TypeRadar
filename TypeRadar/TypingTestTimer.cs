using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TypeRadar
{
    class TypingTestTimer : INotifyPropertyChanged
    {
        private TimeSpan timeElapsed;
        public TimeSpan TimeElapsed
        {
            get => timeElapsed;
            private set
            {
                timeElapsed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeElapsed"));
            }
        }

        public readonly TextSample textSample;

        private DispatcherTimer timer;
        private Stopwatch stopwatch;

        public event PropertyChangedEventHandler PropertyChanged;

        public TypingTestTimer(TextSample textSample)
        {
            this.textSample = textSample;
        }

        public void Start()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            stopwatch = new Stopwatch();

            stopwatch.Start();
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            stopwatch.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeElapsed = stopwatch.Elapsed;
        }
    }
}
