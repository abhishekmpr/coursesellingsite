using coursesellingsite.Models;
using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    private readonly DbContextCourse _context;

    public OrderController(DbContextCourse context)
    {
        _context = context;
    }

    // GET: Order/BuyNow/{CourseId}
    [Route("Order/BuyNow/{CourseId}")]
    public IActionResult BuyNow(int CourseId)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var course = _context.AddCoursePreviewDetails.FirstOrDefault(c => c.Id == CourseId);
        if (course == null)
        {
            return NotFound();
        }

        // Store course information in session to handle the purchase process
        HttpContext.Session.SetInt32("BuyNow_CourseId", CourseId);
        HttpContext.Session.SetString("BuyNow_CourseTitle", course.CourseTitle);
        HttpContext.Session.SetString("BuyNow_CoursePrice", course.CoursePrice.ToString());

        return RedirectToAction("Checkout", "Order");
    }

    // POST: Order/ProcessBuyNow
    [HttpPost]
    public IActionResult ProcessBuyNow(int CourseId, string CourseTitle, double CoursePrice)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return Unauthorized();

        // Store the selected course temporarily in session
        HttpContext.Session.SetInt32("BuyNow_CourseId", CourseId);
        HttpContext.Session.SetString("BuyNow_CourseTitle", CourseTitle);
        HttpContext.Session.SetString("BuyNow_CoursePrice", CoursePrice.ToString());

        return Ok();
    }

    public IActionResult Checkout()
    {
        int? courseId = HttpContext.Session.GetInt32("BuyNow_CourseId");
        string courseTitle = HttpContext.Session.GetString("BuyNow_CourseTitle");
        string coursePriceStr = HttpContext.Session.GetString("BuyNow_CoursePrice");

        if (courseId == null || courseTitle == null || coursePriceStr == null)
            return RedirectToAction("Index", "Home");

        double coursePrice = Convert.ToDouble(coursePriceStr);

        var model = new BuyNowViewModel
        {
            OrderId = courseId.Value,
            CourseTitle = courseTitle,
            Price = coursePrice
        };

        return View(model); // Show payment page or order review
    }

    // Confirm payment after checkout
    [HttpPost]
    public IActionResult ConfirmPayment()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return Unauthorized();

        int? courseId = HttpContext.Session.GetInt32("BuyNow_CourseId");
        string? courseTitle = HttpContext.Session.GetString("BuyNow_CourseTitle");
        string? coursePriceStr = HttpContext.Session.GetString("BuyNow_CoursePrice");

        if (courseId == null || string.IsNullOrEmpty(courseTitle) || string.IsNullOrEmpty(coursePriceStr))
            return BadRequest("Invalid session data");

        // ✅ Check for duplicate purchase
        var existingOrder = _context.Orders
            .FirstOrDefault(o => o.UserId == userId && o.CourseId == courseId);


        if (existingOrder != null)
        {
            TempData["Message"] = "You already purchased this course.";
            return RedirectToAction("MyOrder");
        }

        double coursePrice = Convert.ToDouble(coursePriceStr);

        // ✅ Save new order
        var order = new Order
        {
            UserId = userId.Value,
            CourseId = courseId.Value,
            CourseTitle = courseTitle,
            Price = coursePrice,
            OrderDate = DateTime.Now,
            Status = "Completed"
        };
        _context.Orders.Add(order);

        // ✅ Auto-Enroll in course (UserCourse table required)
        var userCourse = new UserCourse
        {
            UserId = userId.Value,
            CourseId = courseId.Value,
            EnrolledDate = DateTime.Now
        };
        _context.UserCourses.Add(userCourse);

        _context.SaveChanges();

        // ✅ Optional: send confirmation email here

        // Clear session
        HttpContext.Session.Remove("BuyNow_CourseId");
        HttpContext.Session.Remove("BuyNow_CourseTitle");
        HttpContext.Session.Remove("BuyNow_CoursePrice");

        return RedirectToAction("OrderSuccess");
    }

    // Show all orders for a user
    public IActionResult MyOrderDetails()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Home");

        var orders = _context.Orders
            .Where(o => o.UserId == userId.Value)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders); // Simple order details view
    }


    public IActionResult MyOrder()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Home");

        var ordersWithCourseDetails = _context.Orders
    .Where(o => o.UserId == userId.Value)
    .OrderByDescending(o => o.OrderDate)
    .Select(o => new
    {
        Order = o,
        Course = _context.AddCoursePreviewDetails
            .FirstOrDefault(c => c.Id == o.CourseId)
    })
    .ToList();

        // Prepare the model
        var model = ordersWithCourseDetails.Select(o => new BuyNowViewModel
        {
            OrderId = o.Order.OrderId,  // Use OrderId instead of Id
            CourseTitle = o.Course?.CourseTitle,  // CourseTitle from the course details
            Price = o.Order.Price,  // The total price of the order (from the order)
            OrderDate = o.Order.OrderDate,  // Order date from the order
            CoursePrice = o.Course?.CoursePrice ?? 0  // The course price (if available)
        }).ToList();


        return View(model);
    }


    // Show order success message
    public IActionResult OrderSuccess()
    {
        return View();
    }

    // This method checks if the user has purchased the course
    public bool HasPurchasedCourse(int userId, int courseId)
    {
        return _context.Orders.Any(o => o.UserId == userId && o.CourseId == courseId);
    }


}


