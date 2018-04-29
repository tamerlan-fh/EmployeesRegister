using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeesRegister.ViewModels
{
    class PersonRegistryViewModel : ViewModelBase
    {
        public PersonRegistryViewModel()
        {
            RefreshPersons();
            Repository.Instance.PersonChanged += RepositoryPersonChanged;
        }

        private void RepositoryPersonChanged(object sender, ItemChangedEventArgs<Person> e)
        {
            if(e.Action== ItemChangedEventArgs<Person>.ActionType.Add)
            {
                var person = new PersonViewModel(e.Item);
                Persons.Add(person);
                SelectedPerson = person;
            }
        }

        public ObservableCollection<PersonViewModel> Persons
        {
            get { return persons; }
            private set { persons = value; OnPropertyChanged(nameof(Persons)); }
        }
        private ObservableCollection<PersonViewModel> persons;

        public PersonBlankViewModel SelectedPerson
        {
            get { return selectedPerson; }
            set { selectedPerson = value; OnPropertyChanged(nameof(SelectedPerson)); }
        }
        private PersonBlankViewModel selectedPerson;

        #region refresh

        public RelayCommand RefreshPersonsCommand
        {
            get
            {
                if (refreshPersonsCommand is null)
                    refreshPersonsCommand = new RelayCommand(p => RefreshPersons());
                return refreshPersonsCommand;
            }
        }
        private RelayCommand refreshPersonsCommand;

        private void RefreshPersons()
        {
            var items = Repository.Instance.GetPersons();
            if (!items.Success)
            {
                return;
            }

            Persons = new ObservableCollection<PersonViewModel>(items.Value.Select(x => new PersonViewModel(x)));
        }

        #endregion refresh

        #region remove

        public RelayCommand RemovePersonCommand
        {
            get
            {
                if (removePersonCommand is null)
                    removePersonCommand = new RelayCommand(RemovePerson, CanRemovePerson);
                return removePersonCommand;
            }
        }
        private RelayCommand removePersonCommand;

        private void RemovePerson(object obj)
        {
            if (!(obj is PersonViewModel person))
                return;

            var result = Repository.Instance.RemovePerson(person.Model);
            if (result.Success)
                Persons.Remove(person);
        }

        private bool CanRemovePerson(object obj)
        {
            return obj is PersonViewModel;
        }

        #endregion remove

        #region create

        public RelayCommand CreatePersonCommand
        {
            get
            {
                if (createPersonCommand is null)
                    createPersonCommand = new RelayCommand(p => CreatePerson());
                return createPersonCommand;
            }
        }
        private RelayCommand createPersonCommand;

        public void CreatePerson()
        {
            SelectedPerson = new PersonBlankViewModel();
        }

        #endregion create
    }
}
