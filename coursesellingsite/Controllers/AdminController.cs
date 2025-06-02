using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using coursesellingsite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace coursesellingsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly DbContextCourse _courseDb;
        private readonly IWebHostEnvironment _environment;

        public AdminController(DbContextCourse courseDb, IWebHostEnvironment environment)
        {
            _courseDb = courseDb;
            _environment = environment;
        }

        public IActionResult AdminLogin() => View();

        [HttpPost]
        public IActionResult AdminLogin(SignUp s)
        {
            var log = _courseDb.SingUps
                .FirstOrDefault(x => x.Email == s.Email && x.Password == s.Password);

            if (log == null)
            {
                ViewBag.Error = "Invalid credentials.";
                return View();
            }

            HttpContext.Session.SetString("MySession", s.Email);
            HttpContext.Session.SetString("Id", log.Id.ToString());
            return RedirectToAction("DashBoard");
        }

        public IActionResult SignUp() => View();

        [HttpPost]
        public IActionResult SignUp(SignUp sign)
        {
            if (!ModelState.IsValid) return View(sign);

            _courseDb.SingUps.Add(sign);
            _courseDb.SaveChanges();
            return RedirectToAction("AdminLogin");
        }

        public IActionResult DashBoard()
        {
            ViewBag.Users = _courseDb.SingUps.ToList();
            ViewBag.TotalCourses = _courseDb.AddCoursePreviewDetails.Count();
            ViewBag.PendingApprovals = _courseDb.AddCoursePreviewDetails
                .Count(c => c.SellingMode != null && c.SellingMode.ToLower() == "pending");
            ViewBag.Previewdetails = _courseDb.AddCoursePreviewDetails.ToList();
            return View();
        }



        [HttpGet]
        public IActionResult AddCoursePreviewDetail() => View();

        [HttpPost]
        public async Task<IActionResult> AddCoursePreviewDetail(AddCoursePreviewDetail model)
        {

          
           
           // Ensure your view name matches this

            // —— Video Upload ——
            if (model.PreviewDemoVideo != null && model.PreviewDemoVideo.Length > 0)
            {
                var videoFolder = Path.Combine(_environment.WebRootPath, "uploads", "videos");
                Directory.CreateDirectory(videoFolder);

                var uniqueVideoName = $"{Guid.NewGuid()}{Path.GetExtension(model.PreviewDemoVideo.FileName)}";
                var videoPath = Path.Combine(videoFolder, uniqueVideoName);

                using (var stream = new FileStream(videoPath, FileMode.Create))
                {
                    await model.PreviewDemoVideo.CopyToAsync(stream);
                }

                model.PreviewDemoVideoPath = $"/uploads/videos/{uniqueVideoName}";
            }
            else
            {
                ModelState.AddModelError(nameof(model.PreviewDemoVideo), "Please select a video file.");
                return View(model);
            }

            // —— Image Upload ——
            if (model.PicFile != null && model.PicFile.Length > 0)
            {
                var imageFolder = Path.Combine(_environment.WebRootPath, "uploads", "images");
                Directory.CreateDirectory(imageFolder);

                var uniqueImageName = $"{Guid.NewGuid()}{Path.GetExtension(model.PicFile.FileName)}";
                var imagePath = Path.Combine(imageFolder, uniqueImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.PicFile.CopyToAsync(stream);
                }

                model.Pic = $"/uploads/images/{uniqueImageName}";
            }

            // —— Meta & Save ——
            model.CreatedOn = DateTime.Now;
            model.LastUpdate = DateTime.Now;

            await _courseDb.AddCoursePreviewDetails.AddAsync(model);
            await _courseDb.SaveChangesAsync();

            TempData["success"] = "Course preview added successfully!";
            return RedirectToAction("DashBoard");
        }

        public IActionResult EditCoursePreview(int id)
        {
            var preview = _courseDb.AddCoursePreviewDetails.FirstOrDefault(c => c.Id == id);
            if (preview == null) return NotFound();
            return View(preview);
        }

        [HttpPost]
        public async Task<IActionResult> EditCoursePreview(AddCoursePreviewDetail model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.LastUpdate = DateTime.Now;
            _courseDb.Update(model);
            await _courseDb.SaveChangesAsync();

            TempData["success"] = "Preview updated successfully!";
            return RedirectToAction("DashBoard");
        }

        public IActionResult DeleteCoursePreview(int id)
        {
            var preview = _courseDb.AddCoursePreviewDetails.FirstOrDefault(c => c.Id == id);
            if (preview == null) return NotFound();

            // delete image file
            if (!string.IsNullOrEmpty(preview.Pic))
            {
                var imgPath = Path.Combine(_environment.WebRootPath, preview.Pic.TrimStart('/'));
                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }

            // delete video file
            if (!string.IsNullOrEmpty(preview.PreviewDemoVideoPath))
            {
                var videoPath = Path.Combine(_environment.WebRootPath, preview.PreviewDemoVideoPath.TrimStart('/'));
                if (System.IO.File.Exists(videoPath))
                    System.IO.File.Delete(videoPath);
            }

            _courseDb.Remove(preview);
            _courseDb.SaveChanges();

            TempData["success"] = "Preview deleted successfully!";
            return RedirectToAction("DashBoard");
        }

        public IActionResult AddCourseContent()
        {
            var c = _courseDb.AddCoursePreviewDetails.ToList();
            ViewBag.PreviewContent = c;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddCourseContent(AddCourseContent model)
        {
            // —— Video Upload —— (Same as before)
            if (model.VideoFile != null && model.VideoFile.Length > 0)
            {
                var videoFolder = Path.Combine(_environment.WebRootPath, "uploads", "videos");
                Directory.CreateDirectory(videoFolder);

                var uniqueVideoName = $"{Guid.NewGuid()}{Path.GetExtension(model.VideoFile.FileName)}";
                var videoPath = Path.Combine(videoFolder, uniqueVideoName);

                using (var stream = new FileStream(videoPath, FileMode.Create))
                {
                    await model.VideoFile.CopyToAsync(stream);
                }

                model.Video = $"/uploads/videos/{uniqueVideoName}";
            }
            else
            {
                ModelState.AddModelError(nameof(model.VideoFile), "Please select a video file.");
                return View(model);
            }

            // —— Image Upload —— (Handle image upload)
            if (model.PicFile != null && model.PicFile.Length > 0)
            {
                var imageFolder = Path.Combine(_environment.WebRootPath, "uploads", "images");
                Directory.CreateDirectory(imageFolder);

                var uniqueImageName = $"{Guid.NewGuid()}{Path.GetExtension(model.PicFile.FileName)}";
                var imagePath = Path.Combine(imageFolder, uniqueImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.PicFile.CopyToAsync(stream);
                }

                model.Pic = $"/uploads/images/{uniqueImageName}";
            }
            else
            {
                // Set a default image if Pic is not provided (you can change this to a more suitable default image)
                model.Pic = "/uploads/images/default.jpg";  // Ensure this path is valid
            }

            // Ensure that the Pic field is never null
            if (string.IsNullOrEmpty(model.Pic))
            {
                ModelState.AddModelError(nameof(model.Pic), "Please upload an image.");
                return View(model);
            }

            // Save the entity
            await _courseDb.CourseContent.AddAsync(model);
            await _courseDb.SaveChangesAsync();

            return RedirectToAction("DashBoard");
        }

    }

   





}







