namespace EmployeesRegister.Models
{
    class Organization: OrganizationBlank
    {
        public Organization(int id)
        {
            Id = id;
        }

        public Organization(int id, OrganizationBlank blank) : this(id)
        {
            Caption = blank.Caption;
        }

        public int Id { get; }
    }

    class OrganizationBlank
    {
        public string Caption { get; set; }
    }
}
