using Microsoft.AspNetCore.Mvc;
using Webapplication.Models;
using Webapplication.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Webapplication.Controllers;
[Log]
public class EmployeeController:Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _WebHostEnivernment;
    private CookieOptions lastvisitedcookie;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IValidation _validation;


    public EmployeeController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment,ILogger<EmployeeController> logger, IValidation validation)
    {
        lastvisitedcookie= new CookieOptions();
        lastvisitedcookie.Expires=DateTime.Now.AddDays(30);
        _db=db;
        _WebHostEnivernment = webHostEnvironment;
        _logger=logger;
        _validation=validation;
    }
    public IActionResult Start()
    {
        string tempuserid= HttpContext.Session.GetString("Session");
        var condcheck1= new string[]{"Create","Status","Profile","Ratings","PendingForm","ApprovedForm","RejectedForm"};
        if(condcheck1.Contains(Request.Cookies[tempuserid]))
         return RedirectToAction(Request.Cookies[tempuserid],"Employee");
        else
         return RedirectToAction("Index"); 
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public IActionResult Index()
    {
        var createtablefromDb = _db.ReimbursementDetails.Where(s => s.EmployeeId == (HttpContext.Session.GetString("Session")));
        string tempuserid=HttpContext.Session.GetString("Session");
        if(!string.IsNullOrEmpty(tempuserid))
        {
            Response.Cookies.Append(tempuserid,"Index",lastvisitedcookie);
            ViewBag.Message=tempuserid;
            IEnumerable<Employees> employeedetailslist = (IEnumerable<Employees>) createtablefromDb;
            
        return View(employeedetailslist);
        }
        
        return RedirectToAction("LoginPage","Login");
    }
    [HttpGet]
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
    //[Authorize(Roles=Employee)]
    public IActionResult Create()
    {
        string tempuserid=HttpContext.Session.GetString("Session");
        SignUpAccount signupaccount =  _validation.viewProfile(HttpContext.Session.GetString("Session"));
        if(!string.IsNullOrEmpty(tempuserid))
        {
            Response.Cookies.Append(tempuserid,"Create",lastvisitedcookie);
            ViewBag.Message=tempuserid;
            ViewBag.name=signupaccount.EmployeeName;
        return View();
        }
        return RedirectToAction("LoginPage","Login");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public async Task<IActionResult> Create(Employees employee)
    {
        SignUpAccount signupaccount =  _validation.viewProfile(HttpContext.Session.GetString("Session"));
        Console.WriteLine(Request.Form.Files.Count);
        if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
        {
                employee.Status="Pending";
                employee.SubmittedDate=DateTime.Now;
                try
                {
                        string wwwRootPath = _WebHostEnivernment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(employee.File.FileName);
                        string extension = Path.GetExtension(employee.File.FileName);
                        if(extension !=".pdf")
                        {
                            ModelState.AddModelError("file","This is not valid format(PDF only)");
                            ViewBag.name=signupaccount.EmployeeName;
                            return View("Create");
                        }
                        employee.Attachment = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string serverFolder = Path.Combine(wwwRootPath + "/Files/", fileName);
                        using (var fileStream = new FileStream(serverFolder,FileMode.Create))
                        {
                            await employee.File.CopyToAsync(fileStream);
                        }  
                    _db.ReimbursementDetails.Add(employee);
                    await _db.SaveChangesAsync();
                    ViewBag.alert="Your form has been submitted";
                    return View("Status");
                }
                catch(NullReferenceException nullreferenceexception)
                {
                    System.Diagnostics.Trace.TraceError(nullreferenceexception.Message);
                    return RedirectToAction("Error","Home");
                }
        }
        return RedirectToAction("LoginPage","Login");
    } 
    [HttpGet]
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public IActionResult Status()
    {
        string tempuserid=HttpContext.Session.GetString("Session");
         if(!string.IsNullOrEmpty(tempuserid))
        {
            Response.Cookies.Append(tempuserid,"Status",lastvisitedcookie);
            ViewBag.Message=tempuserid;
            return View();
        }
        return RedirectToAction("LoginPage","Login");
    }
    [HttpGet]
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

   public IActionResult Profile()
   {
    string tempuserid=HttpContext.Session.GetString("Session");
    SignUpAccount signupaccount =  _validation.viewProfile(HttpContext.Session.GetString("Session"));
    
    if(!string.IsNullOrEmpty(tempuserid))
    {
        Response.Cookies.Append(tempuserid,"Profile",lastvisitedcookie);
        ViewBag.Message=HttpContext.Session.GetString("Session");
        return View(signupaccount);
    }
    return RedirectToAction("LoginPage","Login");
   }
    
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public IActionResult PendingForm()
    {
        string tempuserid=HttpContext.Session.GetString("Session");
        var createtablefromDb = _db.ReimbursementDetails.Where(s => s.EmployeeId == (HttpContext.Session.GetString("Session")) && s.Status=="Pending");
        int count=createtablefromDb.Count();
        if(count==0)
        {
            if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
             return View("NoForm");
            return RedirectToAction("LoginPage","Login"); 
        }
        else
        {
            if(!string.IsNullOrEmpty(tempuserid))
            {
                Response.Cookies.Append(tempuserid,"PendingForm",lastvisitedcookie);
                ViewBag.Message=HttpContext.Session.GetString("Session");
                IEnumerable<Employees> employeedetailslist = (IEnumerable<Employees>) createtablefromDb;
                
            return View(employeedetailslist);
            }
        }
        
        return RedirectToAction("LoginPage","Login");
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public IActionResult ApprovedForm()
    {
        string tempuserid=HttpContext.Session.GetString("Session");
        var createtablefromDb = _db.ReimbursementDetails.Where(s => s.EmployeeId == (HttpContext.Session.GetString("Session")) && s.Status=="Approved");
        int count=createtablefromDb.Count();
        if(count==0)
        {
            if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
             return View("NoForm");
            return RedirectToAction("LoginPage","Login"); 
        }
        else
        {
            if(!string.IsNullOrEmpty(tempuserid))
            {
                Response.Cookies.Append(tempuserid,"ApprovedForm",lastvisitedcookie);
                ViewBag.Message=HttpContext.Session.GetString("Session");
                IEnumerable<Employees> employeedetailslist = (IEnumerable<Employees>) createtablefromDb;
                
            return View(employeedetailslist);
            }
        }
        
        return RedirectToAction("LoginPage","Login");
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public IActionResult RejectedForm()
    {
        string tempuserid=HttpContext.Session.GetString("Session");
        var createtablefromDb = _db.ReimbursementDetails.Where(s => s.EmployeeId == (HttpContext.Session.GetString("Session")) && s.Status=="Rejected");
        int count=createtablefromDb.Count();
        if(count==0)
        {
            if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
             return View("NoForm");
            return RedirectToAction("LoginPage","Login"); 
        }
        else
        {
            if(!string.IsNullOrEmpty(tempuserid))
            {
                Response.Cookies.Append(tempuserid,"RejectedForm",lastvisitedcookie);
                ViewBag.Message=HttpContext.Session.GetString("Session");
                IEnumerable<Employees> employeedetailslist = (IEnumerable<Employees>) createtablefromDb;
                
            return View(employeedetailslist);
        }
        }
        
        return RedirectToAction("LoginPage","Login");
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public IActionResult Privacy()
    {
         if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
        {
        return View();
        }
        return RedirectToAction("LoginPage","Login");
    }
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]

    public IActionResult Contact()
    {
         if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
        {
        return View();
        }
        return RedirectToAction("LoginPage","Login");
    }
  
    [HttpGet]
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
    public IActionResult PutRemarks()
    {
        if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))  
        { 
           return View();
        }
        return RedirectToAction("Loginpage","Login"); 
    }
    [HttpPost]
    [ResponseCache(Location=ResponseCacheLocation.None, NoStore =true)]
    public async Task<IActionResult> PutRemarks(Ratings ratings)
    {
        if(!string.IsNullOrEmpty(ViewBag.Message=HttpContext.Session.GetString("Session")))
        {
            ratings.EmployeeId = Request.Form["id"];
            ratings.Feedback = Request.Form["fe"];
            ratings.Rating = Request.Form["ra"];
            Console.WriteLine(ratings.Rating);
            Console.WriteLine(ratings.Feedback);
            HttpClient httpClient=new HttpClient();


            string apiurl= "http://localhost:5027/api/Ratings";
            var jsondata=JsonConvert.SerializeObject(ratings);
            var data1=new StringContent(jsondata,Encoding.UTF8, "application/json");
            var httpresponse=httpClient.PostAsync(apiurl,data1);
            var result=await httpresponse.Result.Content.ReadAsStringAsync();
            if(result=="true")
            {
                ViewBag.feedback="Feedback Submitted";
                return View("Thanks");
            }
            ViewBag.feedback="Feedback not Submitted";
            return View();
        }
            return RedirectToAction("LoginPage","Login");

    }
}
