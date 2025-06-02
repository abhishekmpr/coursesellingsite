public class BuyNowViewModel
{
    public int OrderId { get; set; }  // Primary Key of the Order
    public string CourseTitle { get; set; }
    public double Price { get; set; }  // Total price for the order (may include discounts, taxes, etc.)
    public DateTime OrderDate { get; set; }
    public double CoursePrice { get; set; } // Price of the course (actual course cost)
}
