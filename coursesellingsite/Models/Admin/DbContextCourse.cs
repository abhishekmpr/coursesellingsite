using coursesellingsite.Models;
using Microsoft.EntityFrameworkCore;

namespace coursesellingsite.Models
{
    public class DbContextCourse : DbContext
    {
        public DbContextCourse(DbContextOptions opt) : base(opt) { }


        public DbSet<SignUp> SingUps { get; set; }
      
        public DbSet<AddCourseContent> CourseContent { get; set; }

        public DbSet<Register> Registers { get; set; }



        public DbSet<AddCoursePreviewDetail> AddCoursePreviewDetails { get; set; }
        public DbSet<CartItem> cartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }




    }
}
  

