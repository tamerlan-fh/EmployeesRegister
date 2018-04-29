using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeesRegister.ViewModels
{
    class PersonViewModel : PersonBlankViewModel
    {
        public Person Model { get; }

        public PersonViewModel(Person person)
        {
            this.Model = person;
            Name = person.Name;
            Address = person.Address;
            Phone = person.Phone;

            var items = Repository.Instance.GetEmployees(Model);
            if (items.Success && items.Value.Any())
                Employees = new ObservableCollection<Employee>(items.Value);

            Repository.Instance.EmployeeChanged += RepositoryEmployeeChanged;
        }

        private void RepositoryEmployeeChanged(object sender, ItemChangedEventArgs<Employee> e)
        {
            if (e.Item.Person.Id != Model.Id)
                return;

            if (e.Action == ItemChangedEventArgs<Employee>.ActionType.Add)
                Employees.Add(e.Item);

            if (e.Action == ItemChangedEventArgs<Employee>.ActionType.Remove)
            {
                var emploee = Employees.FirstOrDefault(x => x.Organization.Id == e.Item.Organization.Id);
                if (emploee != null)
                    Employees.Remove(emploee);
            }

            if (e.Action == ItemChangedEventArgs<Employee>.ActionType.Update)
            {
                var emploee = Employees.FirstOrDefault(x => x.Organization.Id == e.Item.Organization.Id);
                if (emploee != null)
                    Employees.Remove(emploee);
                Employees.Add(e.Item);
            }
        }

        public ObservableCollection<Employee> Employees { get; }
            = new ObservableCollection<Employee>();

        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                CheckToChange();
            }
        }

        public override string Phone
        {
            get { return base.Phone; }
            set
            {
                base.Phone = value;
                CheckToChange();
            }
        }

        public override string Address
        {
            get { return base.Address; }
            set
            {
                base.Address = value;
                CheckToChange();
            }
        }

        protected override void Save()
        {
            Model.Name = Name;
            Model.Phone = Phone;
            Model.Address = Address;
            Repository.Instance.UpdatePerson(Model);
            CheckToChange();
        }

        protected override bool CanSave()
        {
            return base.CanSave() && HasChanges;
        }

        #region changes

        public bool HasChanges
        {
            get { return hasChanges; }
            set
            {
                if (HasChanges == value) return;
                hasChanges = value; OnPropertyChanged(nameof(HasChanges));
            }
        }
        private bool hasChanges;

        private void CheckToChange()
        {
            HasChanges = Name != Model.Name ||
                Address != Model.Address ||
                Phone != Model.Phone;
        }

        private void BreakChanges()
        {
            Name = Model.Name;
            Address = Model.Address;
            Phone = Model.Phone;
            CheckToChange();
        }

        public RelayCommand BreakChangesCommand
        {
            get
            {
                if (breakChangesCommand is null)
                    breakChangesCommand = new RelayCommand(p => BreakChanges(), p => HasChanges);
                return breakChangesCommand;
            }
        }
        private RelayCommand breakChangesCommand;

        #endregion changes
    }
}
