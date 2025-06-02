using System.ComponentModel.DataAnnotations.Schema;

public class AddCourseContent  // This should match your database table name
{
    public int Id { get; set; }

    // Foreign key property - this is critical
    public int PreviewDetailId { get; set; }

    public string CourseTitle { get; set; }
    public string CourseDescription { get; set; }

   
    public string Video { get; set; }
    public string Pic { get; set; }
   

    [NotMapped]
    public IFormFile VideoFile { get; set; }

    [NotMapped]
    public IFormFile PicFile { get; set; }
}