using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coursesellingsite.Models
{
    public class UserCourse
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to User
        public int UserId { get; set; }

        // Foreign key to Course
        public int CourseId { get; set; }

        // Date when the user enrolled in the course
        public DateTime EnrolledDate { get; set; }

        // Navigation property to the related User
        [ForeignKey("UserId")]
        public virtual Register User { get; set; }

        // Navigation property to the related Course
        [ForeignKey("CourseId")]
        public virtual AddCoursePreviewDetail Course { get; set; }
    }
}
