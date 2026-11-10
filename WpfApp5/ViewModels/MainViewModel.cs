using Smalldatabaseapp.Data;
using Smalldatabaseapp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Smalldatabaseapp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context = new();
        private Person? _selectedPerson;
        private string _searchText = string.Empty;

        public ObservableCollection<Person> People { get; set; } = new();

        public Person? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterPeople();
            }
        }

        // Comandos para los botones
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SortByNameCommand { get; }
        public ICommand SortByAgeCommand { get; }

        public MainViewModel()
        {
            LoadDatabase();

            AddCommand = new RelayCommand(_ => AddPerson());
            DeleteCommand = new RelayCommand(_ => DeletePerson(), _ => SelectedPerson != null);
            SaveCommand = new RelayCommand(_ => SaveDatabase());
            SortByNameCommand = new RelayCommand(_ => SortByName());
            SortByAgeCommand = new RelayCommand(_ => SortByAge());
        }

        private void LoadDatabase()
        {
            _context.Database.EnsureCreated();

            if (!_context.People.Any())
            {
                _context.People.AddRange(
                    new Person { FirstName = "Alex", LastName = "Johnson", Age = 20 },
                    new Person { FirstName = "Jordan", LastName = "Smith", Age = 22 },
                    new Person { FirstName = "Taylor", LastName = "Brown", Age = 19 },
                    new Person { FirstName = "Morgan", LastName = "Davis", Age = 21 }
                );
                _context.SaveChanges();
            }

            var sortedList = _context.People.OrderBy(p => p.LastName).ToList();
            People = new ObservableCollection<Person>(sortedList);
        }

        private void FilterPeople()
        {
            string query = SearchText.Trim().ToLower();
            IQueryable<Person> linqQuery = _context.People;

            if (!string.IsNullOrEmpty(query))
            {
                linqQuery = linqQuery.Where(p =>
                    (p.FirstName != null && p.FirstName.ToLower().Contains(query)) ||
                    (p.LastName != null && p.LastName.ToLower().Contains(query))
                );
            }

            var results = linqQuery.OrderBy(p => p.LastName).ToList();
            People.Clear();
            foreach (var person in results)
            {
                People.Add(person);
            }
        }

        private void AddPerson()
        {
            var newPerson = new Person { FirstName = "New", LastName = "Person", Age = 18 };
            _context.People.Add(newPerson);
            People.Add(newPerson);
            SelectedPerson = newPerson;
        }

        private void DeletePerson()
        {
            if (SelectedPerson != null)
            {
                _context.People.Remove(SelectedPerson);
                People.Remove(SelectedPerson);
                SelectedPerson = null;
            }
            else
            {
                MessageBox.Show("Please select a person to delete.", "Delete Person", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveDatabase()
        {
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Database successfully saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SortByName()
        {
            var sorted = People.OrderBy(p => p.LastName).ToList();
            People.Clear();
            foreach (var p in sorted) People.Add(p);
        }

        private void SortByAge()
        {
            var sorted = People.OrderBy(p => p.Age).ToList();
            People.Clear();
            foreach (var p in sorted) People.Add(p);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}