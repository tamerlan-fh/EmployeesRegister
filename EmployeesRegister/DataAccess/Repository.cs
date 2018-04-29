using EmployeesRegister.Models;
using System;

namespace EmployeesRegister.DataAccess
{
    class Repository
    {
        public static Repository Instance
        {
            get
            {
                if (instance is null)
                    instance = new Repository();
                return instance;
            }
        }
        private static Repository instance;

        private Repository() { }

        #region organization

        public MethodResult<Organization[]> GetOrganizations()
        {
            return DataBaseManager.Instance.GetOrganizations();
        }

        public MethodResult<Organization> AddOrganization(OrganizationBlank blank)
        {
            var result = DataBaseManager.Instance.AddOrganization(blank);

            if (result.Success && OrganizationChanged != null)
                OrganizationChanged(this, new ItemChangedEventArgs<Organization>(result.Value, ItemChangedEventArgs<Organization>.ActionType.Add));

            return result;
        }

        public EmptyResult UpdateOrganization(Organization organization)
        {
            var result = DataBaseManager.Instance.UpdateOrganization(organization);
            if (result.Success && OrganizationChanged != null)
                OrganizationChanged(this, new ItemChangedEventArgs<Organization>(organization, ItemChangedEventArgs<Organization>.ActionType.Update));

            return result;
        }

        public EmptyResult RemoveOrganization(Organization organization)
        {
            var result = DataBaseManager.Instance.RemoveOrganization(organization);
            if (result.Success && OrganizationChanged != null)
                OrganizationChanged(this, new ItemChangedEventArgs<Organization>(organization, ItemChangedEventArgs<Organization>.ActionType.Remove));

            return result;
        }

        public event EventHandler<ItemChangedEventArgs<Organization>> OrganizationChanged;

        #endregion organization

        #region person

        public MethodResult<Person> AddPerson(PersonBlank blank)
        {
            var result = DataBaseManager.Instance.AddPerson(blank);

            if (result.Success && PersonChanged != null)
                PersonChanged(this, new ItemChangedEventArgs<Person>(result.Value, ItemChangedEventArgs<Person>.ActionType.Add));

            return result;
        }

        public EmptyResult UpdatePerson(Person person)
        {
            var result = DataBaseManager.Instance.UpdatePerson(person);
            if (result.Success && PersonChanged != null)
                PersonChanged(this, new ItemChangedEventArgs<Person>(person, ItemChangedEventArgs<Person>.ActionType.Update));
            return result;
        }

        public EmptyResult RemovePerson(Person person)
        {
            var result = DataBaseManager.Instance.RemovePerson(person);
            if (result.Success && PersonChanged != null)
                PersonChanged(this, new ItemChangedEventArgs<Person>(person, ItemChangedEventArgs<Person>.ActionType.Remove));
            return result;
        }

        public MethodResult<Person[]> GetPersons()
        {
            return DataBaseManager.Instance.GetPersons();
        }

        public MethodResult<Person[]> GetPersonsNotWorkInOrganization(Organization organization)
        {
            return DataBaseManager.Instance.GetPersonsNotWorkInOrganization(organization);
        }

        public event EventHandler<ItemChangedEventArgs<Person>> PersonChanged;

        #endregion person

        #region employee

        public MethodResult<Employee> AddEmployee(EmployeeBlank employee)
        {
            var result = DataBaseManager.Instance.AddEmployee(employee);
            if (result.Success && EmployeeChanged != null)
                EmployeeChanged(this, new ItemChangedEventArgs<Employee>(result.Value, ItemChangedEventArgs<Employee>.ActionType.Add));

            return result;
        }

        public EmptyResult UpdateEmployee(Employee employee)
        {
            var result = DataBaseManager.Instance.UpdateEmployee(employee);
            if (result.Success && EmployeeChanged != null)
                EmployeeChanged(this, new ItemChangedEventArgs<Employee>(employee, ItemChangedEventArgs<Employee>.ActionType.Update));

            return result;
        }

        public EmptyResult RemoveEmployee(Employee employee)
        {
            var result = DataBaseManager.Instance.RemoveEmployee(employee);
            if (result.Success && EmployeeChanged != null)
                EmployeeChanged(this, new ItemChangedEventArgs<Employee>(employee, ItemChangedEventArgs<Employee>.ActionType.Remove));

            return result;
        }

        public MethodResult<Employee[]> GetEmployees(Organization organization)
        {
            return DataBaseManager.Instance.GetEmployees(organization);
        }

        public MethodResult<Employee[]> GetEmployees(Person person)
        {
            return DataBaseManager.Instance.GetEmployees(person);
        }

        public MethodResult<Employee[]> GetEmployees()
        {
            return DataBaseManager.Instance.GetEmployees();
        }

        public event EventHandler<ItemChangedEventArgs<Employee>> EmployeeChanged;

        #endregion employee
    }
}
