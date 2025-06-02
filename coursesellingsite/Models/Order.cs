using System;

namespace coursesellingsite.Models
{
    public class Order
    {
        public int OrderId { get; set; } // Primary Key
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public double Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // e.g., "Completed", "Pending", etc.
    }
}
