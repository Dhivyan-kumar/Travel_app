using System;
using System.Collections.Generic;
using Webapplication.Models;
using System.Data.SqlClient;
using System.Data;

namespace Webapplication.Models
{
    public class Repository
    {

        private static List<Employees> allEmployees= new List<Employees>();
        public static IEnumerable<Employees> AllEmployees
        {
            get{
                return allEmployees;
            }
        }
        public static void Create(Employees employee)
        {
            allEmployees.Add(employee);
        }
        public static void Delete(Employees employee)
        {
            for(int i=0;i<allEmployees.Count;i++)
            {
                if(allEmployees[i].EmployeeId==employee.EmployeeId)
                {
                    allEmployees.RemoveAt(i);
                }
            }
        }
        public static void Update(Employees employee)
        {
            foreach(var updateval in allEmployees)
            {
                if(updateval.EmployeeId==employee.EmployeeId)
                {
                    // updateval.Cost1=employee.Cost1;
                }
            }
        }
    }
}