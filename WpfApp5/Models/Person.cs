using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Smalldatabaseapp.Models
{
    public class Person : INotifyPropertyChanged
    {
        private string? _firstName;
        private string? _lastName;
        private int _age;

        public int Id { get; set; }

        public string? FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string? LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}