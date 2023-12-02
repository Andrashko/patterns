using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;


namespace mvvm
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly Model model;

        public PersonViewModel()
        {
            model = new Model();
        }

        public  int Count
        {
            get { return model.Count; }
            set
            {
                model.Count = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}