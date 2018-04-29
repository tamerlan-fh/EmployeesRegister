using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;

namespace EmployeesRegister.ViewModels
{
    class EmployeeBlankViewModel : NotifyDataErrorInfoViewModel, IEmployee
    {
        protected virtual EmployeeBlank Blank { get; }

        public EmployeeBlankViewModel(Organization organization)
        {
            Blank = new EmployeeBlank(organization);
            Persons = GetPersons(organization);

            ValidateNotEmpty(nameof(Position), Position);
            ValidateSalary();
            ValidatePerson();
        }

        public Organization Organization { get { return Blank.Organization; } }

        public string Position
        {
            get { return Blank.Position; }
            set
            {
                if (Position == value) return;
                Blank.Position = value; OnPropertyChanged(nameof(Position));
                ValidateNotEmpty(nameof(Position), Position);
            }
        }

        public Person Person
        {
            get { return Blank.Person; }
            set
            {
                if (Person == value) return;
                Blank.Person = value; OnPropertyChanged(nameof(Person));
                ValidatePerson();
            }
        }

        public decimal Salary
        {
            get { return Blank.Salary; }
            set
            {
                if (Salary == value) return;
                Blank.Salary = value; OnPropertyChanged(nameof(Salary));
                ValidateSalary();
            }
        }

        public Person[] Persons { get; }

        #region save 

        public RelayCommand SaveCommand
        {
            get
            {
                if (saveCommand is null)
                    saveCommand = new RelayCommand(p => Save(), p => CanSave());
                return saveCommand;
            }
        }
        private RelayCommand saveCommand;

        protected  void Save()
        {
            Repository.Instance.AddEmployee(Blank);
        }

        protected  bool CanSave()
        {
            return !HasErrors;
        }

        #endregion save 

        #region validate

        private void ValidateSalary()
        {
            RemoveError(nameof(Salary));
            if (!(Salary > 0))
                AddError(nameof(Salary), "значение должно быть положительным");
        }

        private void ValidatePerson()
        {
            RemoveError(nameof(Person));
            if (Person is null)
                AddError(nameof(Person), "необходимо выбрать нового сотрудника");
        }

        #endregion validate

        private Person[] GetPersons(Organization organization)
        {
            var persons = Repository.Instance.GetPersonsNotWorkInOrganization(organization);
            if (!persons.Success)
                return new Person[] { };
            return persons.Value;
        }
    }
}
