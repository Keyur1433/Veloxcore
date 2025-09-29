using System.Data;
using System.Text.RegularExpressions;

//Part 1 – Basics

//Dataset 1 (Numbers)
List<int> numbers = new List<int> { 2, 5, 8, 10, 3, 7, 6, 1 };

//Filter all numbers greater than 5.
List<int> filteredNumbers = numbers.Where(n => n > 5).ToList();

Console.WriteLine("filteredNumbers:");
foreach (var n in filteredNumbers)
{
    Console.WriteLine(n);
}

//Sort the numbers in ascending order.
List<int> sortedNums = numbers.OrderBy(n => n).ToList();

Console.WriteLine("sortedNums:");
foreach (var n in sortedNums)
{
    Console.WriteLine(n);
}

//Find the maximum and minimum numbers.
int max = numbers.Max();
int min = numbers.Min();

Console.WriteLine("Min & Max:");
Console.WriteLine(max + " " + min);

//Count how many numbers are even.
int totalEvens = numbers.Where(n => n % 2 == 0).Count();
Console.WriteLine("Total Evens: " + totalEvens);

//Dataset 2 (Employees)
List<Employee> employees = new List<Employee> {
    new Employee { Id = 1, Name = "Alice", Department = "HR", Salary = 5000 },
    new Employee { Id = 2, Name = "Bob", Department = "IT", Salary = 7000 },
    new Employee { Id = 3, Name = "Charlie", Department = "IT", Salary = 8000 },
    new Employee { Id = 4, Name = "David", Department = "Finance", Salary = 6500 },
    new Employee { Id = 5, Name = "Eva", Department = "HR", Salary = 5500 }
};

//Get the names of all employees.
List<string> empNames = employees.Select(e => e.Name).ToList();

Console.WriteLine("Employee Names:");
foreach (var e in empNames)
{
    Console.WriteLine(e);
}

//Find employees who work in IT department.
var empInIT = employees.Where(e => e.Department == "IT");

Console.WriteLine("Employee who works in IT department:");
foreach (var e in empInIT)
{
    Console.WriteLine(e.Name);
}

//Find the highest salary employee.
int highestSalary = employees.Select(e => e.Salary).Max();
Console.WriteLine("Highest salary: " + highestSalary);

//Get employees sorted by Salary (descending).
var empSortedBySalary = employees.OrderByDescending(e => e.Salary);

Console.WriteLine("Employee sorted in descending by salary:");
foreach (var e in empSortedBySalary)
{
    Console.WriteLine("Name: " + e.Name + " Salary: " + e.Salary);
}



//Part 2 – Intermediate

//Dataset 1 (Numbers)
List<int> numbers1 = new List<int> { 2, 5, 8, 10, 3, 7, 6, 1, 8, 5, 10 };

//Get the distinct numbers from the list.
var distinctNumbers = numbers1.Distinct();

Console.WriteLine("Distinct numbers:");
foreach (var number in distinctNumbers)
{
    Console.WriteLine(number);
}

//Get the top 3 largest numbers.
var top3LargestNums = numbers1.OrderByDescending(n => n).Take(3);

Console.WriteLine("Top 3 largest numbers");
foreach (var number in top3LargestNums)
{
    Console.WriteLine(number);
}

//Skip the first 5 numbers and return the rest.
var numbersAfterSkippedNNums = numbers1.Skip(3);

Console.WriteLine("Skip the first 5 numbers and return the rest:");
foreach (var number in numbersAfterSkippedNNums)
{
    Console.WriteLine(number);
}

//Dataset 2 (Employees & Departments)
List<Department> departments = new List<Department> {
    new Department { Id = 1, Name = "HR" },
    new Department { Id = 2, Name = "IT" },
    new Department { Id = 3, Name = "Finance" }
};

//Group employees by Department.
var empsByDepts = employees.GroupBy(e => e.Department);

Console.WriteLine("Group employees by Department:");
foreach (var group in empsByDepts)
{
    foreach (var emp in group)
    {
        Console.WriteLine("Name: " + emp.Name + " Department: " + emp.Department);
    }
}

//Get the total salary paid in each department.
var totalSalaryByDept = employees.GroupBy(e => e.Department).Select(group => new
{
    Dept = group.Key,
    Salary = group.Sum(e => e.Salary)
});

Console.WriteLine("Salary department wise");
foreach (var item in totalSalaryByDept)
{
    Console.WriteLine($"Department: {item.Dept}, Total Salary: {item.Salary}");
}

//Perform an inner join between Employees and Departments to return "EmployeeName - DepartmentName".
var empAndDept = from emp in employees
                 join dept in departments
                 on emp.Department equals dept.Name
                 select new
                 {
                     Name = emp.Name,
                     Department = dept.Name,
                 };

Console.WriteLine("Perform an inner join between Employees and Departments to return EmployeeName - DepartmentName.");

foreach (var item in empAndDept)
{
    Console.WriteLine($"Employee: {item.Name}, Department: {item.Department}");
}

//Find the employee with the second highest salary.
var empWith2ndHighestSalary = employees.OrderByDescending(e => e.Salary).Skip(1).Take(1);

Console.WriteLine("Employee with the second highest salary: ");
foreach (var emp in empWith2ndHighestSalary)
{
    Console.WriteLine($"Name: {emp.Name} Salary: {emp.Salary} ");
}

//Get the list of employees where name starts with 'A' or 'E'.
var empWithSpecificAlpha = employees.Where(e => e.Name.StartsWith("A") || e.Name.StartsWith("E"));

Console.WriteLine("List of employees where name starts with 'A' or 'E':");
foreach (var emp in empWithSpecificAlpha)
{
    Console.WriteLine($"Name: {emp.Name}");
}

class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public int Salary { get; set; }
}

class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
}