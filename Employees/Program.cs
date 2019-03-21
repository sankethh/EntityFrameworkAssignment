using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Employees;

namespace EFemp
{
    class Program
    {
        static void Main(string[] args)
        {
            int Flag = 1;
            int choice;
            while (Flag == 1)
            {
                Console.WriteLine("Choose an option from the below: - \n");
                Console.WriteLine("1) Insert from CSV into DATABASE \n 2) Get all employee Details according to your location \n");
                Console.WriteLine("3) Get all employee details greater than the date given by user \n 4) exit \n");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        string userchoice = Program.Csvinsert();
                        if (userchoice == "n")
                        {
                            Flag = 0;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Enter the location to filter \n");
                        string location = Console.ReadLine();
                        Program.Locationmapping(location);
                        break;
                }
            }
        }
        public static void Locationmapping(string location)
        {
            Entities _repo = new Entities();
            EmployeeDetailsTable emp = new EmployeeDetailsTable();

            List<EmployeeDetailsTable> query = _repo.EmployeeDetailsTables.Where(s => s.LOCATION == location).ToList();
            foreach (var employeeid in query)
            {
                string formateddateofbirth = String.Format("{0:MM/dd/yyyy}", emp.DOB);
                string formateddateofjoining = String.Format("{0:MM/dd/yyyy}", emp.DOJ);
                Console.WriteLine($"EID : {employeeid.EID} NAME : {employeeid.ENAME} DOB : {emp.DOB} Location : {emp.LOCATION} DOJ : {formateddateofjoining} \n");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
        }
        public static string Csvinsert()
        {
            List<MyMappedCSVFile> values = File.ReadAllLines(@"C:\Users\Sanket\Desktop\Neudesic\CSVRecord.csv")
                                           .Skip(1)
                                           .Select(v => MyMappedCSVFile.FromCsv(v))
                                           .ToList();
            foreach (var rowl in values)
            {
                Entities _repo = new Entities();
                EmployeeDetailsTable emp = new EmployeeDetailsTable();
                Console.WriteLine(rowl);
                emp.EID = rowl.EID;
                emp.ENAME = rowl.Name;
                emp.DOB = rowl.DOB;
                emp.LOCATION = rowl.location;
                emp.DOJ = rowl.DOJ;
                try
                {
                    _repo.EmployeeDetailsTables.Add(emp);
                    _repo.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString() + "Warning");
                }
            }
            Console.WriteLine("\n Want to enter another option ? y/n");
            string choice2 = Console.ReadLine();
            return choice2;
        }

        public class MyMappedCSVFile
        {
            public string Name { get; set; }
            public System.DateTime DOB { get; set; }
            public int EID { get; set; }
            public string location { get; set; }
            public System.DateTime DOJ { get; set; }
            public override string ToString()
            {
                return "Person: " + EID + " " + Name + " " + DOB + " " + location + " " + DOJ;
            }
            public static MyMappedCSVFile FromCsv(string csvLine)
            {
                string[] values = csvLine.Split(',');
                MyMappedCSVFile obj = new MyMappedCSVFile();
                obj.EID = Convert.ToInt32(values[0]);
                obj.Name = Convert.ToString(values[2]);
               
                obj.DOB = Convert.ToDateTime(values[10]);
                obj.location = Convert.ToString(values[34]);
                obj.DOJ = Convert.ToDateTime(values[14]);
                return obj;
            }
        }
    }
}