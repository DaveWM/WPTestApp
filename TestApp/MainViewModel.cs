using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            _title = "Title";
            Refresh();
        }

        private string _title;
        public string Title { get
            {
                return _title;
            }
            set
            {
                _title = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                Refresh();
            }
        }

        private void Refresh()
        {
            Items.Clear();
            foreach (var str in _allStrings.Where(s => s.ToLower().Contains(_title.ToLower())))
            {
                Items.Add(str);
            }
        }

        private List<string> _allStrings = new List<string>{ "test", "hello", "another one", "title" };
        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();

        public ICommand ResetTitle
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    this.Title = "Reset";
                });
            }
        }

    }
}
