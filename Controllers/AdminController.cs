using System;
using Webapplication.Models;
using System.Diagnostics;
using Webapplication.Data;
using Microsoft.AspNetCore.Mvc;


namespace Webapplication.Controllers;
[Log]
public class AdminController: Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<AdminController> _logger;
    private readonly IValidation _validation;


    public AdminController(ApplicationDbContext db,ILogger<AdminController> logger,IValidation validation)
    {
        _logger = logger;
        _db=db;
        _validation=validation;
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
    public IActionResult Reports(Employees employees)
    {
        if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
        {
            
            IEnumerable<Employees> employeedetailslist = _db.ReimbursementDetails.Where(m => m.Status!="Approved");
            IEnumerable<Employees> employeedetailslist1 = employeedetailslist.Where(v => v.Status !="Rejected");
            return View(employeedetailslist1);
        }
        return RedirectToAction("LoginPage","Login");
    }
      
    public IActionResult Approve(Employees employees)
    {
        SignUpAccount signupaccount =  _validation.viewProfile(employees.EmployeeId);
        Console.WriteLine(signupaccount.Emailaddress);
        var text="Your form has been approved";
        employees.Status = "Approved";
        _db.ReimbursementDetails.Update(employees);
        _db.SaveChanges();
        // Validation.sendEmail(signupaccount.Emailaddress,text);
        //Console.WriteLine(mail);
        return RedirectToAction("Reports","Admin");
    }
     public IActionResult Reject(Employees employees)
    {
        employees.Status = "Rejected";
        _db.ReimbursementDetails.Update(employees);
        _db.SaveChanges();
        return RedirectToAction("Reports","Admin");
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
    public IActionResult Home()
    {
        if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
        {
        return View();
        }
        return RedirectToAction("LoginPage","Login");
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
    public IActionResult AdminProfile()
   {
    SignUpAccount signupaccount =  _validation.viewProfile(HttpContext.Session.GetString("Session"));
    
    if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
    {
        
    return View(signupaccount);
    }
    return RedirectToAction("LoginPage","Login");
   }
   [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
   public IActionResult Records()
   {
     if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
    {
        var employeedetailslist = _db.ReimbursementDetails.ToList();
       int count= employeedetailslist.Count;
       ViewBag.count="Total Number of reports: "+ count;
            
        return View(employeedetailslist);
    }
    return RedirectToAction("LoginPage","Login");
   }

[ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
  public async Task<IActionResult>  GetRemarks()
    {
    if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
    {
      HttpClient httpClient=new HttpClient();
      string apiurl= "http://localhost:5027/api/Ratings";
      var response = httpClient.GetAsync(apiurl).Result;
      IEnumerable<Ratings> listoffeedback=response.Content.ReadAsAsync<IEnumerable<Ratings>>().Result;
      return View(listoffeedback);
    }
    return RedirectToAction("LoginPage","Login");
    }
}