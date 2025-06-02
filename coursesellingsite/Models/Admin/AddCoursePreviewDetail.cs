using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace coursesellingsite.Models;

public class AddCoursePreviewDetail
{
    [Key]
    public int Id { get; set; }

   
    public string CourseTitle { get; set; }

  
    public string CourseDescription { get; set; }

    
    public string RelatedCategory { get; set; }

   
    public string CourseName { get; set; }

    
    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime LastUpdate { get; set; }

 
    public string UsedLanguage { get; set; }

    
    public string SellingMode { get; set; }

    
    public double CoursePrice { get; set; }

   
    public decimal SellingPrice { get; set; }

    // File input from form (not mapped to DB)
    [NotMapped]
    public IFormFile PreviewDemoVideo { get; set; }

    // Stored in DB
    public string PreviewDemoVideoPath { get; set; }

    // File input from form (image)
    [NotMapped]
    public IFormFile PicFile { get; set; }

    // Stored in DB
    public string Pic { get; set; }

    
    public string CourseLevel { get; set; }

    
    public string PreRequirement { get; set; }

    public bool IsPublic { get; set; }  // This can be true or false

    public virtual ICollection<UserCourse> UserCourses { get; set; }

}
