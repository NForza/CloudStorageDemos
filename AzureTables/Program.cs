using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTables
{
    class Program
    {
        public class EmployeeRecord : TableEntity
        {

            public EmployeeRecord()
            {
                //Needs to be here for the query to work
            }

            public EmployeeRecord(string lastName, string firstName)
            {
                this.PartitionKey = lastName;
                this.RowKey = firstName;
            }

            public string LastName
            {
                get { return PartitionKey; }
                set { PartitionKey = value; }
            }
            public string FirstName {
                get { return RowKey; }
                set { RowKey = value; }
            }

            public string Email { get; set; }

            public double Salary { get; set; }

            public override string ToString()
            {
                return $"Employee: Name={LastName},{FirstName},Salary={Salary},Email={Email}";
            }
        }

        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("employees");

            table.CreateIfNotExists();

            //var insert = TableOperation.Insert(new EmployeeRecord("Johnson", "John") { Email = "john@johnson.com", Salary = 50000 });
            var insert = TableOperation.InsertOrReplace(new EmployeeRecord("Johnson", "John") { Email = "john@johnson.com", Salary = 50000 });
            var result = table.Execute(insert);            

            AddMoreEmployees(table);

            var qry = new TableQuery<EmployeeRecord>();//.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));
            foreach (var e in table.ExecuteQuery(qry))
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static void AddMoreEmployees(CloudTable table)
        {
            var batchInsert = new TableBatchOperation();
            //Must have same partitionkey!
            batchInsert.Add(TableOperation.InsertOrReplace(new EmployeeRecord("Smith", "John") { Email = "johnsmith@company.com", Salary = 30000 }));
            batchInsert.Add(TableOperation.InsertOrReplace(new EmployeeRecord("Smith", "Steven") { Email = "steven@company.com", Salary = 40000 }));
            table.ExecuteBatch(batchInsert);

            table.Execute(TableOperation.InsertOrReplace(new EmployeeRecord("Johnson", "Sally") { Email = "s.johnson@employee.com", Salary = 50000 }));
            table.Execute(TableOperation.InsertOrReplace(new EmployeeRecord("May", "Jim") { Email = "may@home.com", Salary = 45000 }));
        }
    }
}
