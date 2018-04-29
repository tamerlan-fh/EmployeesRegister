namespace EmployeesRegister.Models
{
    class Person: PersonBlank
    {
        public Person(int id)
        {
            Id = id;
        }
        public Person(int id, PersonBlank blank) : this(id)
        {
            Name = blank.Name;
            Phone = blank.Phone;
            Address = blank.Address;
        }

        public int Id { get; }
    }

    class PersonBlank
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
