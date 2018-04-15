using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace OracleConnect
{
    class TestDb
    {
        public static void Main(string[] args)
        {
            try
            {
                //Database.SetInitializer(new DropCreateDatabaseAlways<OracleDbContext>());
                //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
                using (var ctx = new OracleDbContext())
                {
                    InsertData(ctx);

                    int employeeId = 207;
                    Print(ctx, employeeId);

                    employeeId = 208;
                    Print(ctx, employeeId);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.InnerException.Message);
                    }
                    else
                    {
                        Console.Write(ex.InnerException.Message);
                    }
                }
                else
                {
                    Console.Write(ex.Message);
                }
            }
        }

        private static void Print(OracleDbContext ctx, int employeeId)
        {
            var emp = ctx.Employees.FirstOrDefault(p => p.EmployeeId == employeeId);


            Console.WriteLine("EmployeeIemp: " + emp.EmployeeId);
            Console.WriteLine("Name: " + emp.Name);
            Console.WriteLine("Email: " + emp.Email);
            Console.WriteLine("JobId: " + emp.JobId);
            Console.WriteLine("Hire_Date: " + emp.Hire_Date.ToString("dd/MMM/yyyy"));
            Console.WriteLine("************");
        }

        private static void InsertData(OracleDbContext ctx)
        {
            var emp1 = new Employee
            {
                EmployeeId = 207,
                Name = "Sai",
                Hire_Date = DateTime.Now,
                JobId = "IT_PROG",
                Email = "sai@gmail.com"
            };

            var emp2 = new Employee
            {
                EmployeeId = 208,
                Name = "Vani",
                Hire_Date = DateTime.Now,
                JobId = "SA_MAN",
                Email = "vani@gmail.com"
            };
            ctx.Employees.Add(emp1);
            ctx.Employees.Add(emp2);

            //ctx.Database.Log = s => Console.WriteLine(s);

            ctx.SaveChanges();

            var dept1 = new Department
            {
                DepartmentId = 271,
                Name = "SW",
                ManagerId = emp1.EmployeeId
            };
            var dept2 = new Department
            {
                DepartmentId = 272,
                Name = "HRD",
                ManagerId = emp2.EmployeeId
            };

            ctx.Departments.Add(dept1);
            ctx.Departments.Add(dept2);
            ctx.SaveChanges();
        }
    }

    [Table(name: "EMPLOYEES")]
    public class Employee
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "EMPLOYEE_ID")]
        public int EmployeeId { get; set; }

        [Column(name: "LAST_NAME")]
        public string Name { get; set; }

        [Column(name: "EMAIL")]
        public string Email { get; set; }

        [Column(name: "HIRE_DATE")]
        public DateTime Hire_Date { get; set; }

        [ForeignKey("Job")]
        [Column(name: "JOB_ID")]
        public string JobId { get; set; }

        public virtual Job Job { get; set; }
    }

    [Table(name: "DEPARTMENTS")]
    public class Department
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "DEPARTMENT_ID")]
        public int DepartmentId { get; set; }

        [Column(name: "DEPARTMENT_NAME")]
        public string Name { get; set; }

        [ForeignKey("Manager")]
        [Column(name: "MANAGER_ID")]
        public int ManagerId { get; set; }

        public virtual Employee Manager { get; set; }
    }

    [Table(name: "JOBS")]
    public class Job
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "JOB_ID")]
        public string JobId { get; set; }

        [Column(name: "JOB_TITLE")]
        public string Title { get; set; }
    }

    public class OracleDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("HR");

            //modelBuilder.Entity<Job>().ToTable("JOBS");
            //modelBuilder.Entity<Job>().HasKey<string>(p => p.JobId);
            //modelBuilder.Entity<Job>().Property(t => t.JobId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //modelBuilder.Entity<Employee>().ToTable("EMPLOYEES");
            //modelBuilder.Entity<Employee>().HasKey(p => p.EmployeeId);
            //modelBuilder.Entity<Employee>().Property(t => t.EmployeeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //modelBuilder.Entity<Department>().ToTable("DEPARTMENTS");
            //modelBuilder.Entity<Department>().HasKey(p => p.DepartmentId);
            //modelBuilder.Entity<Department>().Property(t => t.DepartmentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);           
        }
    }
}

