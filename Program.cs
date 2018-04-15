using System;
using Dapper;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using Dapper.FluentColumnMapping;

namespace OracleConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = @"Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = xe)));Persist Security Info=True;User ID=HR;Password=hr";
            try
            {
                var mappings = new ColumnMappingCollection();

                // Start defining the mappings between each property/column for a type
                mappings.RegisterType<Employee>()
                        .MapProperty(x => x.EmployeeId).ToColumn("EMPLOYEE_ID")
                        .MapProperty(x => x.Name).ToColumn("LAST_NAME")
                        .MapProperty(x => x.HireDate).ToColumn("HIRE_DATE")
                        .MapProperty(x => x.JobId).ToColumn("JOB_ID")
                        .MapProperty(x => x.Email).ToColumn("EMAIL");
                mappings.RegisterWithDapper();


                using (var dbConn = new OracleConnection(ConnectionString))
                {
                    //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;                    

                    dbConn.Open();
                    int id = 207;

                    var delQuery = @"delete employees where EMPLOYEE_ID=" + id;
                    dbConn.Execute(delQuery);

                    var insertQuery
                        = "insert into employees(EMPLOYEE_ID,LAST_NAME,HIRE_DATE,JOB_ID,EMAIL) values(207,'Sairam','15-APR-2018','IT_PROG','sairam@gmail.com')";

                    dbConn.Execute(insertQuery);

                    Console.WriteLine("**********");
                    Console.WriteLine("Before update");
                    Print(dbConn, id);

                    var updateQuery
                        = @"update employees set email='sairam.konuru@gmail.com' where EMPLOYEE_ID="+id;
                    dbConn.Execute(updateQuery);

                    Console.WriteLine("**********");
                    Console.WriteLine("After update");
                    Print(dbConn, id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Print(OracleConnection dbConn,int id)
        {
            var selectQuery = @"select * from employees where employee_id="+id;
            var data = dbConn.Query<Employee>(selectQuery);
            var emp1 = data.FirstOrDefault();

            Console.WriteLine(emp1.EmployeeId);
            Console.WriteLine(emp1.Name);
            Console.WriteLine(emp1.Email);
            Console.WriteLine(emp1.HireDate.ToString("dd/MMM/yyyy"));
        }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public string JobId { get; set; }
    }

}
