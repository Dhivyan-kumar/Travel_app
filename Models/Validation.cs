using System.Text.RegularExpressions;
using Webapplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
namespace Webapplication.Models
{
    public class Validation:IValidation
    {
        static Dictionary<string, string[]> employeedata = new Dictionary<string, string[]>();

        static Validation()
        {
            using(SqlConnection connection = new SqlConnection(getConnectionString()))
            {
                connection.Open();
                Console.WriteLine("Connection Established");
                SqlCommand selectcommand= new SqlCommand("RegistrationDetailsStoredProcedure",connection);
                selectcommand.CommandType=CommandType.StoredProcedure;
                SqlDataReader readdata= selectcommand.ExecuteReader();
                while(readdata.Read())
                {
                    string[] userdata= new string[4];
                    userdata[0]= readdata["Password"].ToString();
                    userdata[1]=readdata["Emailaddress"].ToString();
                    userdata[2]=readdata["Employeename"].ToString();
                    userdata[3]=readdata["Role"].ToString();
                    employeedata.Add(readdata["EmployeeId"].ToString(),userdata);
                }
            }
        }
        public   int validateDetails(SignUpAccount signupaccount)
        {
            string idpattern="^[A-Z]{3}[0-9]{5}$";
            string userpattern="^[a-zA-Z]*$";
            string passpattern="^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
            Regex rg1= new Regex(idpattern);
            Regex rg2= new Regex(passpattern);
            Regex rg3= new Regex(userpattern);
            string id = signupaccount.EmployeeID;
            string name = signupaccount.EmployeeName;
            string email = signupaccount.Emailaddress;
            string password = signupaccount.Password;
            string role=signupaccount.Role;
            // Console.WriteLine(signupaccount.EmployeeID);
            // Console.WriteLine("hello");
            if(rg1.IsMatch(signupaccount.EmployeeID.ToString()))
            {
                if(rg2.IsMatch(signupaccount.Password) && rg3.IsMatch(signupaccount.EmployeeName))
                {
                    if(string.Equals(signupaccount.Password,signupaccount.ConfirmPassword))
                    {
                        foreach(var checkdata in employeedata)
                        {
                            if(string.Equals(checkdata.Key,signupaccount.EmployeeID)|| string.Equals(signupaccount.Emailaddress,checkdata.Value[1]))
                            {
                                Console.WriteLine("User already exists");
                                return 5;
                            }
                        }
                            using(SqlConnection connection = new SqlConnection(getConnectionString()))
                            {
                                connection.Open();
                                SqlCommand insertCommand= new SqlCommand("RegistrationDetailsStoredProcedure",connection);
                                insertCommand.CommandType=CommandType.StoredProcedure;
                                insertCommand.Parameters.AddWithValue("@operation","insertdetails");
                                insertCommand.Parameters.AddWithValue("@EmployeeId",signupaccount.EmployeeID);
                                insertCommand.Parameters.AddWithValue("@Employeename",signupaccount.EmployeeName);
                                insertCommand.Parameters.AddWithValue("@Emailaddress",signupaccount.Emailaddress);
                                insertCommand.Parameters.AddWithValue("@Password",signupaccount.Password);
                                insertCommand.Parameters.AddWithValue("@Role",signupaccount.Role);
                                insertCommand.ExecuteNonQuery();
                                string[] userdata= new string[4];
                                userdata[0]= password;
                                userdata[1]=email;
                                userdata[2]=name;
                                userdata[3]=role;
                                employeedata.Add(id,userdata);
                            }
                            return 1;
                    }
                    else
                    {
                        Console.WriteLine("Password mismatch");  
                        return 2;
                    } 
                }   
                else
                {
                    Console.WriteLine("Invalid username or password");
                    return 3;
                }  
            }
            else
            {
                Console.WriteLine("Invalid ID");
                return 4;
            }
        }
        public   int validateLogin(Account account)
        {
            
                foreach(var checkdata in employeedata)
                {
                    if(string.Equals(account.EmployeeId,checkdata.Key) && string.Equals(checkdata.Value[3],"Admin"))
                    {
                        if(string.Equals(account.Password,checkdata.Value[0]))
                         return 4;
                        
                    }
                    
                    else if(string.Equals(checkdata.Key,account.EmployeeId))
                    {
                        if( string.Equals(account.Password,checkdata.Value[0]))
                            return 1; 
                        else
                            return 2;
                    }  
                }
            return 3;
        }
        public  int changePassword(ForgotPassword forgotpassword)
        {
            string id=forgotpassword.EmployeeID;
            string email=forgotpassword.Emailaddress;
            string password=forgotpassword.Password;
            string confirmpassword=forgotpassword.ConfirmPassword;
            Console.WriteLine(email+password);
            string passpattern="^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
             Regex rg1= new Regex(passpattern);
             foreach(KeyValuePair<string, string[]> checkdata in employeedata)
             {
                if(email.ToString()==checkdata.Value[1].ToString() && string.Equals(id,checkdata.Key))
                {
                            using(SqlConnection connection = new SqlConnection(getConnectionString()))
                            {
                                connection.Open();
                                Console.WriteLine("update connection establsihed");
                                SqlCommand updatecommand = new SqlCommand("RegistrationDetailsStoredProcedure",connection);
                                updatecommand.CommandType= CommandType.StoredProcedure;
                                updatecommand.Parameters.AddWithValue("@operation","updateDetails");
                                updatecommand.Parameters.AddWithValue("@Emailaddress",email);
                                updatecommand.Parameters.AddWithValue("@EmployeeId",id);
                                updatecommand.Parameters.AddWithValue("@Password",password);
                                updatecommand.ExecuteNonQuery();
                            }
                            return 1;
                }
             }
             return 2;
        }
        public  int validateForm(Employees employee)
        {
            
            // int fileSize= employee.Attachment.ContentLength;
            foreach(KeyValuePair<string,string[]> validatedata in employeedata)
            {
                if(string.Equals(employee.EmployeeName,validatedata.Value[2]) && string.Equals(employee.EmployeeId,validatedata.Key))
                {
                    return 1;
                }     
            }
            return 2;
        }
        public  SignUpAccount viewProfile(string EmployeeID)
        {
            SignUpAccount signupaccount= new SignUpAccount();
            try
            {
            using(SqlConnection connection= new SqlConnection(getConnectionString()))
            {
                connection.Open();
                string queryString=$"SELECT * FROM Signuptable WHERE EmployeeId='{EmployeeID}';";
                SqlCommand sqlCommand= new SqlCommand(queryString,connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while(reader.Read())
                {
                    signupaccount.EmployeeID=reader["EmployeeId"].ToString();
                    signupaccount.EmployeeName=reader["EmployeeName"].ToString();
                    signupaccount.Emailaddress=reader["Emailaddress"].ToString();
                    signupaccount.Role=reader["Role"].ToString();  
                }    
            }   
            }
            catch(SqlException sqlexception)
            {
                Console.WriteLine(sqlexception);
            }
            return signupaccount;
        }
        public static string getConnectionString()
        {
            var build=new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json",optional:true, reloadOnChange:true);
            IConfiguration configuration=build.Build();
            string connectionString = Convert.ToString(configuration.GetConnectionString("DefaultConnection"));
            if(connectionString!=null)
             return connectionString;
            return ""; 
        }

        // public static async Task sendEmail(string receiver,string message)
        // {
        //     try
        //     {
        //         var senderEmail= new MailAddress("dhivyankumar253@gmail.com","Dhivyan");
        //         var receiverEmail= new MailAddress(receiver,"Receiver");
        //         var password="Kd.com@253";
        //         var subject="Reimbursement Status";
        //         var body=message;
        //         Console.WriteLine("Trying............!!!!!......");
        //         var smtp = new SmtpClient
        //         {
        //             Host="smtp.gmail.com",
        //             Port=587,
        //             EnableSsl=true,
        //             DeliveryMethod= SmtpDeliveryMethod.Network,
        //             UseDefaultCredentials=false,
        //             Credentials= new NetworkCredential(senderEmail.Address,password)
        //         };  
        //         using(var mess=new MailMessage(senderEmail,receiverEmail)
        //         {
        //             Subject=subject,
        //             Body=body
        //         })
        //         {
        //             await smtp.SendMailAsync(mess);
        //             Console.WriteLine("Mail Sent");
        //         }
        //     }
        //     catch(Exception exception)
        //     {
        //         Console.WriteLine("Error in sending Email");
        //     }
        // }
    }
}