using coursesellingsite.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace coursesellingsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbContextCourse course;


        public HomeController(DbContextCourse course)
        {
            this.course = course;

        }

        public IActionResult Index()
        {




            var c = course.AddCoursePreviewDetails.ToList();
            ViewBag.Details = c;


            return View();



        }
        public IActionResult Viewcourse(int Id)
        {
            var selectedCourse = course.AddCoursePreviewDetails.FirstOrDefault(x => x.Id == Id);
            
            var contentList = course.CourseContent
                                     .Where(x => x.PreviewDetailId == Id)
                                     .ToList();
            
            
            ViewBag.Detail = contentList;
           
            

            return View(selectedCourse);
        }



        public IActionResult VP(int Id)
        {
            // Get the course details
            var selectedCourse = course.AddCoursePreviewDetails.FirstOrDefault(x => x.Id == Id);
            if (selectedCourse == null)
            {
                return NotFound(); // If course is not found, return not found page
            }

            // Get the content related to the selected course
            var contentList = course.CourseContent
                                     .Where(x => x.PreviewDetailId == Id)
                                     .ToList();

            // Check if user has access to the course
            bool hasAccess = CheckUserAccess(selectedCourse);
            ViewBag.HasAccess = hasAccess; // Passing to view

            // Pass course content and details to the view
            ViewBag.ContentList = contentList;
            ViewBag.CourseDetails = course.AddCoursePreviewDetails.ToList();

            return View(selectedCourse); // Return the course details to the view
        }

        private bool CheckUserAccess(AddCoursePreviewDetail course)
        {
            // Get user ID from session
            int? userId = HttpContext.Session.GetInt32("UserId");

            // If user is not logged in, deny access
            if (userId == null)
            {
                return false; // No access if not logged in
            }

            // Check if the user has purchased the course
            bool hasPurchased = course.UserCourses != null && course.UserCourses
                                                 .Any(uc => uc.UserId == userId.Value && uc.CourseId == course.Id);

            return hasPurchased; // Return true if course is purchased
        }







        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Register model)
        {
            if (ModelState.IsValid)
            {
                course.Registers.Add(model); // Add to database
                course.SaveChanges();       // Commit changes
                TempData["success"] = "User registered successfully!";
                return RedirectToAction("Success");
            }

            return View(model); // Return view with validation messages
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // Authenticate the user
                var user = course.Registers
                    .FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                if (user != null)
                {
                    // Store User ID and Email in session
                    HttpContext.Session.SetInt32("UserId", user.Id); // Store UserId
                    HttpContext.Session.SetString("UserEmail", user.Email); // Store UserEmail (optional)

                    // Redirect to the Dashboard
                    return RedirectToAction("Dashboard", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }



        [HttpGet]
        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var purchasedCourseIds = course.UserCourses
                                            .Where(uc => uc.UserId == userId.Value)
                                            .Select(uc => uc.CourseId)
                                            .ToList();

            var purchasedCourses = course.AddCoursePreviewDetails
                                         .Where(c => purchasedCourseIds.Contains(c.Id))
                                         .ToList();

            ViewBag.PurchasedCourses = purchasedCourses;

            return View();
        }


        public IActionResult Details(int courseId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var courses = course.AddCoursePreviewDetails.FirstOrDefault(c => c.Id == courseId);
            if (courses == null)
            {
                return NotFound();
            }

            // Check if the course is locked
            bool hasAccess = HasPurchasedCourse(userId.Value, courseId);

            // Pass the locked status to the view
            ViewData["HasAccess"] = hasAccess;

            return View(courses);
        }


        // Check if the user has purchased the course
        public bool HasPurchasedCourse(int userId, int courseId)
        {
            return course.Orders.Any(o => o.UserId == userId && o.CourseId == courseId);
        }

        [HttpPost]
        public IActionResult PurchaseCourse(int courseId)
        {
            // Get the logged-in user's UserId from session
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login"); // If the user is not logged in, redirect to login
            }

            // Check if the user has already purchased the course
            var existingPurchase = course.UserCourses
                                        .FirstOrDefault(uc => uc.UserId == userId.Value && uc.CourseId == courseId);

            if (existingPurchase != null)
            {
                // If the user has already purchased this course
                TempData["Error"] = "You have already purchased this course.";
                return RedirectToAction("Dashboard");
            }

            // Create a new UserCourse record to represent the purchase
            var userCourse = new UserCourse
            {
                UserId = userId.Value,
                CourseId = courseId,
                EnrolledDate = DateTime.Now // Store the purchase date
            };

            // Add the record to the UserCourses table
            course.UserCourses.Add(userCourse);
            course.SaveChanges();

            // Optionally, store a success message
            TempData["Success"] = "Course purchased successfully!";

            return RedirectToAction("Dashboard");
        }

    }


}





   



