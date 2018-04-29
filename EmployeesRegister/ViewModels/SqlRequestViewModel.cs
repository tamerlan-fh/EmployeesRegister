using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;
using System.Linq;

namespace EmployeesRegister.ViewModels
{
    class SqlRequestViewModel : ViewModelBase
    {
        public SqlRequestViewModel()
        {
            RefreshCollections();
        }

        public Employee[] Employees
        {
            get { return employees; }
            private set { employees = value; OnPropertyChanged(nameof(Employees)); }
        }
        private Employee[] employees;

        public OrganizationSqlRequestViewModel[] Organizations
        {
            get { return organizations; }
            private set { organizations = value; OnPropertyChanged(nameof(Organizations)); }
        }
        private OrganizationSqlRequestViewModel[] organizations;

        public Person[] Persons
        {
            get { return persons; }
            private set { persons = value; OnPropertyChanged(nameof(Persons)); }
        }
        private Person[] persons;

        public Employee[] SqlRequestCollection
        {
            get { return slRequestCollection; }
            private set { slRequestCollection = value; OnPropertyChanged(nameof(SqlRequestCollection)); }
        }
        private Employee[] slRequestCollection;

        public RelayCommand RefreshCollectionsCommand
        {
            get
            {
                if (refreshCollectionsCommand is null)
                    refreshCollectionsCommand = new RelayCommand(p => RefreshCollections());
                return refreshCollectionsCommand;
            }
        }
        private RelayCommand refreshCollectionsCommand;

        private void RefreshCollections()
        {
            var personsResult = Repository.Instance.GetPersons();
            if (!personsResult.Success)
                return;
            Persons = personsResult.Value;

            var organizationsResult = Repository.Instance.GetOrganizations();
            if (!organizationsResult.Success)
                return;
            Organizations = organizationsResult.Value
                .Select(x => new OrganizationSqlRequestViewModel(x))
                .ToArray();

            var employeesResult = Repository.Instance.GetEmployees();
            if (!employeesResult.Success)
                return;
            Employees = employeesResult.Value
                .OrderBy(x => x.Organization.Id)
                .ToArray();

            SqlRequestCollection = null;
        }

        public RelayCommand SqlRequest1Command
        {
            get
            {
                if (sqlRequest1Command is null)
                    sqlRequest1Command = new RelayCommand(p => SqlRequest1());
                return sqlRequest1Command;
            }
        }
        private RelayCommand sqlRequest1Command;

        private void SqlRequest1()
        {
            var result = DataBaseManager.Instance.GetSqlRequest1();
            if (!result.Success)
                return;
            SqlRequestCollection = result.Value;
        }

        public RelayCommand SqlRequest2Command
        {
            get
            {
                if (sqlRequest2Command is null)
                    sqlRequest2Command = new RelayCommand(p => SqlRequest2());
                return sqlRequest2Command;
            }
        }
        private RelayCommand sqlRequest2Command;

        private void SqlRequest2()
        {
            var result = DataBaseManager.Instance.GetSqlRequest2();
            if (!result.Success)
                return;
            foreach (var organization in Organizations)
                if (organization.Id == result.Value)
                    organization.IsRich = true;
                else
                    organization.IsRich = false;
        }
    }

    class OrganizationSqlRequestViewModel : ViewModelBase
    {
        private readonly Organization organization;
        public OrganizationSqlRequestViewModel(Organization organization)
        {
            this.organization = organization;
        }
        public string Caption { get { return organization.Caption; } }

        public int Id { get { return organization.Id; } }

        public bool IsRich
        {
            get { return isRich; }
            set
            {
                if (IsRich == value) return;
                isRich = value; OnPropertyChanged(nameof(IsRich));
            }
        }
        private bool isRich;
    }
}
