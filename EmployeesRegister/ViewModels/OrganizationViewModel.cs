using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeesRegister.ViewModels
{
    class OrganizationViewModel : OrganizationBlankViewModel
    {
        public Organization Model { get; }

        public OrganizationViewModel(Organization organization)
        {
            this.Model = organization;
            Caption = organization.Caption;
            RefreshEmployees();
            Repository.Instance.EmployeeChanged += RepositoryEmployeeChanged;
            Repository.Instance.PersonChanged += RepositoryPersonChanged;
        }

        private void RepositoryPersonChanged(object sender, ItemChangedEventArgs<Person> e)
        {
            if (e.Action == ItemChangedEventArgs<Person>.ActionType.Remove)
                foreach (var employee in Employees.Where(x => x.Person.Id == e.Item.Id).ToArray())
                    Employees.Remove(employee);
        }

        private void RepositoryEmployeeChanged(object sender, ItemChangedEventArgs<Employee> e)
        {
            if (e.Item.Organization.Id != Model.Id)
                return;

            if (e.Action == ItemChangedEventArgs<Employee>.ActionType.Add)
            {
                var employee = new EmployeeViewModel(e.Item);
                Employees.Add(employee);
                SelectedEmployee = employee;
            }
        }

        public override string Caption
        {
            get { return base.Caption; }
            set
            {
                base.Caption = value;
                CheckToChange();
            }
        }

        protected override void Save()
        {
            Model.Caption = Caption;
            Repository.Instance.UpdateOrganization(Model);
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
            HasChanges = Caption != Model.Caption;
        }

        private void BreakChanges()
        {
            Caption = Model.Caption;
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

        #region employees

        public ObservableCollection<EmployeeViewModel> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged(nameof(Employees)); }
        }
        private ObservableCollection<EmployeeViewModel> employees;

        public IEmployee SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }
        private IEmployee selectedEmployee;

        private void RefreshEmployees()
        {
            var items = Repository.Instance.GetEmployees(Model);
            if (!items.Success)
            {
                return;
            }

            Employees = new ObservableCollection<EmployeeViewModel>(items.Value.Select(x => new EmployeeViewModel(x)));
        }

        #region remove employee

        public RelayCommand RemoveEmployeeCommand
        {
            get
            {
                if (removeEmployeeCommand is null)
                    removeEmployeeCommand = new RelayCommand(RemoveEmployee, CanRemoveEmployee);
                return removeEmployeeCommand;
            }
        }
        private RelayCommand removeEmployeeCommand;

        private void RemoveEmployee(object obj)
        {
            if (!(obj is EmployeeViewModel employee))
                return;

            var result = Repository.Instance.RemoveEmployee(employee.Model);
            if (result.Success)
                Employees.Remove(employee);
        }

        private bool CanRemoveEmployee(object obj)
        {
            return obj is EmployeeViewModel;
        }

        #endregion remove employee

        #region create employee

        public RelayCommand CreateEmployeeCommand
        {
            get
            {
                if (createEmployeeCommand is null)
                    createEmployeeCommand = new RelayCommand(p => CreateEmployee());
                return createEmployeeCommand;
            }
        }
        private RelayCommand createEmployeeCommand;

        public void CreateEmployee()
        {
            SelectedEmployee = new EmployeeBlankViewModel(Model);
        }

        #endregion create employee

        #endregion employees
    }
}
