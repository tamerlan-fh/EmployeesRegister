using EmployeesRegister.Models;
using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace EmployeesRegister.DataAccess
{
    partial class DataBaseManager
    {
        #region organization

        public MethodResult<Organization[]> GetOrganizations()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM [organization] WHERE active=1;";

                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<Organization[]>(null, table.Description);

                var organizations = table.Value.Select().Select(record => new Organization(Convert.ToInt32(record["oid"]))
                {
                    Caption = Convert.ToString(record["name"])
                })
                .ToArray();

                return new MethodResult<Organization[]>(organizations);
            }
        }

        public MethodResult<Organization> AddOrganization(OrganizationBlank blank)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO [organization] (name) VALUES(@name);";
                command.Parameters.Add(new SQLiteParameter("@name", blank.Caption) { DbType = DbType.String });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new MethodResult<Organization>(null, result.Description);

                return new MethodResult<Organization>(new Organization(result.Value, blank));
            }
        }

        public EmptyResult UpdateOrganization(Organization organization)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE [organization] SET name =@name WHERE oid=@oid;";
                command.Parameters.Add(new SQLiteParameter("@oid", organization.Id) { DbType = DbType.Int32 });
                command.Parameters.Add(new SQLiteParameter("@name", organization.Caption) { DbType = DbType.String });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new EmptyResult(result.Description);

                return new EmptyResult();
            }
        }

        public EmptyResult RemoveOrganization(Organization organization)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE [employee] SET active=0 WHERE active=1 AND oid=@oid;";
                command.CommandText += @"UPDATE [organization] SET active=0 WHERE oid=@oid;";
                command.Parameters.Add(new SQLiteParameter("@oid", organization.Id) { DbType = DbType.Int32 });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new EmptyResult(result.Description);

                return new EmptyResult();
            }
        }

        #endregion organization

        #region person

        public MethodResult<Person> AddPerson(PersonBlank blank)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO [person] (name, phone, address) VALUES(@name, @phone, @address);";
                command.Parameters.Add(new SQLiteParameter("@name", blank.Name) { DbType = DbType.String });
                command.Parameters.Add(new SQLiteParameter("@phone", blank.Phone) { DbType = DbType.String });
                command.Parameters.Add(new SQLiteParameter("@address", blank.Address) { DbType = DbType.String });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new MethodResult<Person>(null, result.Description);

                return new MethodResult<Person>(new Person(result.Value, blank));
            }
        }

        public EmptyResult UpdatePerson(Person person)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE [person] SET name =@name, phone=@phone, address=@address WHERE pid=@pid;";
                command.Parameters.Add(new SQLiteParameter("@pid", person.Id) { DbType = DbType.Int32 });
                command.Parameters.Add(new SQLiteParameter("@name", person.Name) { DbType = DbType.String });
                command.Parameters.Add(new SQLiteParameter("@phone", person.Phone) { DbType = DbType.String });
                command.Parameters.Add(new SQLiteParameter("@address", person.Address) { DbType = DbType.String });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new EmptyResult(result.Description);

                return new EmptyResult();
            }
        }

        public EmptyResult RemovePerson(Person person)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE [employee] SET active=0 WHERE active=1 AND pid=@pid;";
                command.CommandText += @"UPDATE [person] SET active=0 WHERE pid=@pid;";
                command.Parameters.Add(new SQLiteParameter("@pid", person.Id) { DbType = DbType.Int32 });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new EmptyResult(result.Description);

                return new EmptyResult();
            }
        }

        public MethodResult<Person[]> GetPersons()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT * FROM [person] WHERE active=1;");

                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<Person[]>(null, table.Description);

                var persons = table.Value.Select().Select(record => new Person(Convert.ToInt32(record["pid"]))
                {
                    Name = Convert.ToString(record["name"]),
                    Address = Convert.ToString(record["address"]),
                    Phone = Convert.ToString(record["phone"])
                })
                .ToArray();

                return new MethodResult<Person[]>(persons);
            }
        }

        public MethodResult<Person[]> GetPersonsNotWorkInOrganization(Organization organization)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT * FROM [person] WHERE active=1 AND pid NOT IN (SELECT pid FROM [employee] WHERE active=1 AND oid=@oid);");
                command.Parameters.Add(new SQLiteParameter("@oid", organization.Id) { DbType = DbType.Int32 });
                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<Person[]>(null, table.Description);

                var persons = table.Value.Select().Select(record => new Person(Convert.ToInt32(record["pid"]))
                {
                    Name = Convert.ToString(record["name"]),
                    Address = Convert.ToString(record["address"]),
                    Phone = Convert.ToString(record["phone"])
                })
                .ToArray();

                return new MethodResult<Person[]>(persons);
            }
        }

        #endregion person

        #region employee

        public MethodResult<Employee> AddEmployee(EmployeeBlank employee)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO [employee] (oid, pid, position, salary) VALUES(@oid, @pid, @position, @salary);";
                command.Parameters.Add(new SQLiteParameter("@oid", employee.Organization.Id) { DbType = DbType.Int32 });
                command.Parameters.Add(new SQLiteParameter("@pid", employee.Person.Id) { DbType = DbType.Int32 });
                command.Parameters.Add(new SQLiteParameter("@position", employee.Position) { DbType = DbType.String });
                command.Parameters.Add(new SQLiteParameter("@salary", employee.Salary) { DbType = DbType.Decimal });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new MethodResult<Employee>(null, result.Description);

                return new MethodResult<Employee>(new Employee(employee));
            }
        }

        public EmptyResult UpdateEmployee(Employee employee)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE [employee] SET position=@position, salary=@salary WHERE pid=@pid AND oid=@oid;";
                command.Parameters.Add(new SQLiteParameter("@oid", employee.Organization.Id) { DbType = DbType.Int32 });
                command.Parameters.Add(new SQLiteParameter("@pid", employee.Person.Id) { DbType = DbType.Int32 });
                command.Parameters.Add(new SQLiteParameter("@position", employee.Position) { DbType = DbType.String });
                command.Parameters.Add(new SQLiteParameter("@salary", employee.Salary) { DbType = DbType.Decimal });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new EmptyResult(result.Description);

                return new EmptyResult();
            }
        }

        public EmptyResult RemoveEmployee(Employee employee)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE [employee] SET active=0 WHERE pid=@pid AND oid=@oid;";
                command.Parameters.Add(new SQLiteParameter("@oid", employee.Organization.Id) { DbType = DbType.Int32 });
                command.Parameters.Add(new SQLiteParameter("@pid", employee.Person.Id) { DbType = DbType.Int32 });

                var result = ExecuteNonQuery(command);
                if (!result.Success)
                    return new EmptyResult(result.Description);

                return new EmptyResult();
            }
        }

        public MethodResult<Employee[]> GetEmployees(Organization organization)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT [person].pid, name, phone, address, position, salary FROM [employee] JOIN [person] ON [employee].pid=[person].pid WHERE oid=@oid AND [person].active=1 AND [employee].active=1;";
                command.Parameters.Add(new SQLiteParameter("@oid", organization.Id) { DbType = DbType.Int32 });

                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<Employee[]>(null, table.Description);

                var employees = table.Value.Select().Select(record => new Employee(new Person(Convert.ToInt32(record["pid"]))
                {
                    Name = Convert.ToString(record["name"]),
                    Address = Convert.ToString(record["address"]),
                    Phone = Convert.ToString(record["phone"])
                }, organization)
                {
                    IsActive = true,
                    Position = Convert.ToString(record["position"]),
                    Salary = Convert.ToDecimal(record["salary"])
                })
                .ToArray();

                return new MethodResult<Employee[]>(employees);
            }
        }

        public MethodResult<Employee[]> GetEmployees(Person person)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT [organization].*, position, salary FROM [employee] JOIN [organization] ON [employee].oid=[organization].oid WHERE pid=@pid AND [organization].active=1 AND [employee].active=1;";
                command.Parameters.Add(new SQLiteParameter("@pid", person.Id) { DbType = DbType.Int32 });

                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<Employee[]>(null, table.Description);

                var employees = table.Value.Select().Select(record => new Employee(person, new Organization(Convert.ToInt32(record["oid"]))
                {
                    Caption = Convert.ToString(record["name"])
                })
                {
                    IsActive = true,
                    Position = Convert.ToString(record["position"]),
                    Salary = Convert.ToDecimal(record["salary"])
                })
                .ToArray();

                return new MethodResult<Employee[]>(employees);
            }
        }

        public MethodResult<Employee[]> GetEmployees()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT organization.oid, organization.name as oname, person.pid, person.name as pname, person.phone, person.address, position, salary FROM [employee] JOIN [organization],[person] ON [employee].oid=[organization].oid AND [employee].pid=[person].pid  WHERE [employee].active=1;";

                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<Employee[]>(null, table.Description);

                var employees = table.Value.Select().Select(record => new Employee(
                    new Employee(new Person(Convert.ToInt32(record["pid"]))
                    {
                        Name = Convert.ToString(record["pname"]),
                        Address = Convert.ToString(record["address"]),
                        Phone = Convert.ToString(record["phone"])
                    },
                    new Organization(Convert.ToInt32(record["oid"]))
                    {
                        Caption = Convert.ToString(record["oname"])
                    })
                    {
                        IsActive = true,
                        Position = Convert.ToString(record["position"]),
                        Salary = Convert.ToDecimal(record["salary"])
                    })).ToArray();

                return new MethodResult<Employee[]>(employees);
            }
        }

        #endregion employee

        public MethodResult<Employee[]> GetSqlRequest1()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT organization.*, person.*,max_salary FROM (SELECT oid, pid, Max(salary) AS max_salary FROM [employee] WHERE active=1 GROUP BY(oid)) tb JOIN organization,person ON tb.oid=organization.oid AND tb.pid=person.pid;";

                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<Employee[]>(null, table.Description);

                var employees = table.Value.Select().Select(record => new Employee(
                    new Employee(new Person(Convert.ToInt32(record["pid"]))
                    {
                        Name = Convert.ToString(record["name"]),
                        Address = Convert.ToString(record["address"]),
                        Phone = Convert.ToString(record["phone"])
                    },
                    new Organization(Convert.ToInt32(record["oid"]))
                    {
                        Caption = Convert.ToString(record["name"])
                    })
                    {
                        IsActive = true,
                        Salary = Convert.ToDecimal(record["max_salary"])
                    })).ToArray();

                return new MethodResult<Employee[]>(employees);
            }
        }

        public MethodResult<int?> GetSqlRequest2()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT oid FROM (SELECT oid, Sum(salary) AS sum_salary  FROM employee WHERE active=1 GROUP BY oid) ORDER BY sum_salary DESC LIMIT 1;";

                var table = Execute(command);
                if (!table.Success)
                    return new MethodResult<int?>(null, table.Description);

                var record = table.Value.Select().FirstOrDefault();
                if (record is null)
                    return new MethodResult<int?>(null, "отсутствует");

                return new MethodResult<int?>(Convert.ToInt32(record["oid"]));
            }
        }
    }
}
