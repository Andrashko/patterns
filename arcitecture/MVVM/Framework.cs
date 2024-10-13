/*
*https://www.codeproject.com/Articles/1163180/Bare-Metal-MVVM-Where-The-Code-Meets-The-Road-Part
*
*
*
*
*/

using System.ComponentModel;
using System.Reflection;


namespace mvvm
{

    public abstract class Framework
    {
        private readonly Dictionary<string, Binding> bindings = new Dictionary<string, Binding>();
        public object DataContext { get; set; }

        protected void SetBinding(string property)
        {
            Binding binding = new Binding(property)
            {
                Source = DataContext,
                Value = DataContext.GetType().GetProperty(property)?.GetValue(DataContext, null)
            };
            binding.Parse();
            bindings.Add(property, binding);
        }

        protected object ValueOf(string property)
        {
            return bindings[property].Value;
        }
    }
    public class Binding
    {
        public Binding(string property)
        {
            Name = property;
        }

        public object Source { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }

        public void Parse()
        {
            INotifyPropertyChanged inpc = Source as INotifyPropertyChanged;
            if (inpc == null)
            {
                return;
            }
            inpc.PropertyChanged += Inpc_PropertyChanged;
        }

        private void Inpc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyInfo propertyInfo = Source.GetType().GetProperty(e.PropertyName);
            object value = propertyInfo?.GetValue(Source);
            Value = value;
            Console.WriteLine($"{e.PropertyName} changed to {value}");
        }
    }

}