using System;

namespace Webapplication.Models
{
    public interface IValidation
    {
        public   int validateDetails(SignUpAccount signupaccount);

        public   int validateLogin(Account account);
        public  int changePassword(ForgotPassword forgotpassword);
        public  int validateForm(Employees employee);
        public  SignUpAccount viewProfile(string EmployeeID);


    }
}