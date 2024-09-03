using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.MockData;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/joins")]
    [ApiController]
    public class JoinsLearning : ControllerBase
    {
        [HttpGet("InnerJoinDemo")]
        public IActionResult InnerJoinDemo()
        {
            List<Customer> customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Alice" },
                new Customer { Id = 2, Name = "Bob" },
                new Customer { Id = 3, Name = "John" },
                new Customer { Id = 4, Name = "Emily" }
            };
            List<Order> orders = new List<Order>
            {
                new Order { OrderId = 101, CustomerId = 1 },
                new Order { OrderId = 102, CustomerId = 2 },
                new Order { OrderId = 103, CustomerId = 1 },
                new Order { OrderId = 104, CustomerId = 3 }
            };

            var result = from customer in customers
                         join order in orders on customer.Id equals order.CustomerId
                         select new CustomerOrderDto { CustomerNameID = customer.Id, CustomerName = customer.Name, OrderId = order.OrderId };

            return Ok(result.ToList());
        }

        [HttpGet("GroupJoinDemo")]
        public IActionResult GroupJoinDemo()
        {
            var departments = new List<Department>
            {
                new Department { DepartmentId = 1, Name = "HR" },
                new Department { DepartmentId = 2, Name = "IT" }
            };
            var employees = new List<DepEmployee>
            {
                new DepEmployee { EmployeeId = 101, DepartmentId = 1, Name = "Alice" },
                new DepEmployee { EmployeeId = 102, DepartmentId = 2, Name = "Bob" },
                new DepEmployee { EmployeeId = 103, DepartmentId = 1, Name = "Charlie" },
                new DepEmployee { EmployeeId = 104, DepartmentId = 2, Name = "John" },
                new DepEmployee { EmployeeId = 105, DepartmentId = 1, Name = "Smith" }
            };

            var result = from department in departments
                         join employee in employees on department.DepartmentId equals employee.DepartmentId into grouped
                         select new { DepartmentName = department.Name, Employee = grouped };

            return Ok(result);
        }

        [HttpGet("SelectManyDemo")]
        public IActionResult SelectManyDemo()
        {
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Alice", Courses = new List<string> { "NodeJS", "C#", "ASP.NET Core" } },
                new Student { Id = 2, Name = "Bob", Courses = new List<string> { "MySQL", "SQL Server" } },
                new Student { Id = 3, Name = "John", Courses = new List<string> { "Java", "PHP" } }
            };

            var result = from student in students
                         from course in student.Courses
                         select new { StudentName = student.Name, CorseName = course };

            return Ok(result);
        }

        [HttpGet("InnerJoinDemo2")]
        public IActionResult InnerJoinDemo2()
        {
            IEnumerable<Employee> employees = Employee.GetAllEmployees();
            IEnumerable<Address> addresses = Address.GetAllAddresses();

            // var result = from employee in employees
            //              join address in addresses on employee.AddressId equals address.ID 
            //              select new { EmployeeName = employee.Name, Address = address.AddressLine };

            var result = employees.Join(
                            addresses,
                            employee => employee.AddressId,
                            address => address.ID,
                            (employee, address) => new
                            {
                                EmployeeName = employee.Name,
                                AddressLine = address.AddressLine
                            }).ToList();

            return Ok(result);
        }
    }

    // Inner Join
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
    }

    public class CustomerOrderDto
    {
        public int CustomerNameID { get; set; }
        public string CustomerName { get; set; } = "";
        public int OrderId { get; set; }
    }

    // Group Join
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = "";
    }
    public class DepEmployee
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; } = "";
    }

    // Student
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<string>? Courses { get; set; }
    }
}