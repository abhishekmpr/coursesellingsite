public class CartItem
{
    public int Id { get; set; }

    public int CourseId { get; set; }        // Unique ID of the course
    public string CourseTitle { get; set; }  // Title/name of the course
    public Double Price { get; set; }       // Price of the course
    public int Quantity { get; set; }        // How many of this course user added

    public int UserId { get; set; }
}
