using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Timers;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace pomodoro_menu.ViewModels
{
    public class PomodoroPageViewModel : NotificationObject
    {
        private Timer timer;
        private int pomodoroDuration;
        private int breakDuration;
        private TimeSpan ellapsed;

        public TimeSpan Ellapsed
        {
            get { return ellapsed; }
            set 
            { 
                ellapsed = value;
                OnPropertyChanged();
            }
        }
        public ICommand StartOrPauseCommand { get; set; }
        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set 
            { 
                isRunning = value;
                OnPropertyChanged(); 
            }
        }
        private bool isinbreak;

        public bool IsInBreak
        {
            get { return isinbreak; }
            set 
            { 
                isinbreak = value;
                OnPropertyChanged(); 
            }
        }
        public PomodoroPageViewModel()
        {
            InitializeTimer();
            LoadConfiguredValues();
            StartOrPauseCommand = new Command(StartOrPauseCommandExecute);
        }

        private void LoadConfiguredValues()
        {
            if (Application.Current.Properties.ContainsKey(Literals.PomodoroDuration))
                pomodoroDuration = (int)Application.Current.Properties[Literals.PomodoroDuration];
            else
                pomodoroDuration = 1;
            if (Application.Current.Properties.ContainsKey(Literals.BreakDuration))
                breakDuration = (int)Application.Current.Properties[Literals.BreakDuration];
            else
                breakDuration = 1;
        }

        private void InitializeTimer()
        {
            timer = new Timer
            {
                Interval = 1000
            };
            timer.Elapsed += Timer_Elapsed;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Ellapsed = Ellapsed.Add(TimeSpan.FromSeconds(1));

            if(IsRunning && (Ellapsed.TotalSeconds == pomodoroDuration * 60)) 
            {
                IsRunning = false;
                IsInBreak = true;
                Ellapsed = TimeSpan.Zero;

                SavePomodoroAsync();
            }
            if (IsInBreak && (Ellapsed.TotalSeconds == breakDuration * 60))
            {
                IsRunning = true;
                IsInBreak = false;
                Ellapsed = TimeSpan.Zero;
            }
        }

        private async Task SavePomodoroAsync()
        {
            List<DateTime> history;
            if (Application.Current.Properties.ContainsKey(Literals.History))
            {
                //Deserializing before bc ObservableCollection is complex type
                var json = Application.Current.Properties[Literals.History].ToString();
                history = JsonConvert.DeserializeObject<List<DateTime>>(json);
            }
            else 
            {
                history = new List<DateTime>();
            }
            history.Add(DateTime.Now);

            //Serialize
            var serializedHistory = JsonConvert.SerializeObject(history);
            Application.Current.Properties[Literals.History] = serializedHistory;

            await Application.Current.SavePropertiesAsync();
        }

        private void StartTimer() 
        {
            timer.Start();
            IsRunning = true;
        }
        private void StopTimer()
        {
            timer.Stop();
            isRunning = false;
        }

        private void StartOrPauseCommandExecute()
        {
            if (IsRunning) 
            {
                StopTimer(); 
            }
            else 
            {
                StartTimer(); 
            }
        }
    }
}

