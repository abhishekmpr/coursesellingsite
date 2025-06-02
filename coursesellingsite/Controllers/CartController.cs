using Microsoft.AspNetCore.Mvc;
using coursesellingsite.Models;
using Microsoft.EntityFrameworkCore;

namespace coursesellingsite.Controllers
{
    public class CartController : Controller
    {
        private readonly DbContextCourse _context;

        public CartController(DbContextCourse context)
        {
            _context = context;
        }

        // Add item to the cart
        [HttpPost]
        public IActionResult AddToCart(int CourseId, string CourseTitle, double CoursePrice)
        {
            // Retrieve UserId from session
            int userId =Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

          

  
                // Add new item to the cart
                var cartItem = new CartItem
                {
                    UserId = userId,
                    Id = CourseId,
                    CourseTitle = CourseTitle,
                    Price = CoursePrice,
                    Quantity = 1 // Initial quantity
                };
                _context.cartItems.Add(cartItem);
            _context.SaveChanges();
            



            //  _context.SaveChanges();
            return RedirectToAction("SavedCourse");
        }

        // View cart items
        public IActionResult SavedCourse()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // Redirect to login if user is not logged in
            }

            var cartItems = _context.cartItems
                .Where(x => x.UserId == userId)
                .ToList();

            return View(cartItems); // Pass cart items to the view
        }

        // Remove item from the cart
        public IActionResult RemoveFromCart(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // Redirect to login if user is not logged in
            }

            var cartItem = _context.cartItems
                .FirstOrDefault(x => x.UserId == userId && x.Id == id);

            if (cartItem != null)
            {
                _context.cartItems.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("SavedCourse"); // Redirect to the cart view
        }
    }
}
