using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace mvvm
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Model model;

        public ViewModel(Model model)
        {
            this.model = model;
        }

        public int Count
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