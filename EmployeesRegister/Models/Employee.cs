namespace EmployeesRegister.Models
{
    class Employee : EmployeeBlank
    {
        public Employee(Person person, Organization organization) : base(organization)
        {
            Person = person;
        }

        public Employee(EmployeeBlank blank) : this(blank.Person, blank.Organization)
        {
            Position = blank.Position;
            Salary = blank.Salary;
            IsActive = true;
        }

        public new Person Person { get; }

        public bool IsActive { get; set; }
    }

    class EmployeeBlank: IEmployee
    {
        public EmployeeBlank(Organization organization)
        {
            Organization = organization;
        }

        public Person Person { get; set; }

        public Organization Organization { get; }

        public string Position { get; set; }

        public decimal Salary { get; set; }
    }

    interface IEmployee
    {
        Person Person { get; }

        Organization Organization{ get; }

        string Position { get; set; }

        decimal Salary { get; set; }
    }
}
