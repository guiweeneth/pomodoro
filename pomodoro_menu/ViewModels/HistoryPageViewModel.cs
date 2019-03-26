using System;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace pomodoro_menu.ViewModels
{
    public class HistoryPageViewModel : NotificationObject
    {
        private ObservableCollection<DateTime> pomodoros;

        public ObservableCollection<DateTime> Pomodoros
        {
            get { return pomodoros; }
            set 
            { 
                pomodoros = value;
                OnPropertyChanged(); 
            }
        }
        public HistoryPageViewModel() 
        {
            LoadHistory(); 
        }

        private void LoadHistory()
        {
            if (Application.Current.Properties.ContainsKey(Literals.History)) 
            {
                var json = Application.Current.Properties[Literals.History].ToString();
                var history = JsonConvert.DeserializeObject<List<DateTime>>(json);
                Pomodoros = new ObservableCollection<DateTime>(history);
            }
        }
    }
}

