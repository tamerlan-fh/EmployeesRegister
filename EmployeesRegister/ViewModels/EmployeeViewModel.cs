using System;
using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;

namespace EmployeesRegister.ViewModels
{
    class EmployeeViewModel : NotifyDataErrorInfoViewModel, IEmployee
    {
        public Employee Model { get; }

        public EmployeeViewModel(Employee employee)
        {
            this.Model = employee;
            Position = Model.Position;
            Salary = Model.Salary;
        }

        public Person Person
        {
            get { return Model.Person; }
        }

        public Organization Organization
        {
            get { return Model.Organization; }
        }

        public string Position
        {
            get { return position; }
            set
            {
                if (Position == value) return;
                position = value; OnPropertyChanged(nameof(Position));
                ValidateNotEmpty(nameof(Position), Position);
                CheckToChange();
            }
        }
        private string position;

        public decimal Salary
        {
            get { return salary; }
            set
            {
                if (Salary == value) return;
                salary = value; OnPropertyChanged(nameof(Salary));
                ValidateSalary();
                CheckToChange();
            }
        }
        private decimal salary;

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

        protected void Save()
        {
            Model.Salary = Salary;
            Model.Position = Position;
            Repository.Instance.UpdateEmployee(Model);
            CheckToChange();
        }

        protected bool CanSave()
        {
            return !HasErrors && HasChanges;
        }

        #endregion save 

        #region validate

        private void ValidateSalary()
        {
            RemoveError(nameof(Salary));
            if (!(Salary > 0))
                AddError(nameof(Salary), "значение должно быть положительным");
        }

        #endregion validate

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
            HasChanges = Salary != Model.Salary || Position != Model.Position;
        }

        private void BreakChanges()
        {
            Salary = Model.Salary;
            Position = Model.Position;
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
