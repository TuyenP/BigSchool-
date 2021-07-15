using BigSchool.Models;
using BigSchool.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext _dbContext;

        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
            };

            return View("CoursesForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("CoursesForm", viewModel);
            }

            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place
            };

            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();

            foreach (Course item in courses)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.LecturerId);
                item.UserName = user.Name;

                if (userId != null)
                {
                    var find = _dbContext.Attendances.Where(a => a.CourseId == item.Id && a.AttendeeId == userId).FirstOrDefault();
                    if (find == null)
                    {
                        item.isShowGoing = true;
                    }

                    Following findFollow = _dbContext.Followings.FirstOrDefault(p => p.FolloweeId == userId && p.FollowerId == item.LecturerId);
                    if (findFollow == null)
                    {
                        item.isShowFollow = true;
                    }
                }
            }

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();

            return View(courses);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Courses.Single(c => c.Id == id && c.LecturerId == userId);
               

            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = courses.DateTime.ToString("dd/M/yyyy"),
                Time = courses.DateTime.ToString("HH:mm"),
                Category = courses.CategoryId,
                Place = courses.Place,
                Heading = "Edit Course",
                Id = courses.Id
            };

            return View("CoursesForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("CoursesForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userId);

            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = _dbContext.Followings.Where(p => p.FollowerId == userId).ToList();
            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = _dbContext.Attendances.Where(p => p.AttendeeId == userId).ToList();
            var totalCourses = new List<Course>();
            foreach (var lecturer in listFollwee)
            {
                var courses = _dbContext.Courses
                .Where(a => a.LecturerId == lecturer.FolloweeId)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
                foreach (Course item in courses)
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.LecturerId);
                    item.UserName = user.Name;

                    if (userId != null)
                    {
                        var find = _dbContext.Attendances.Where(a => a.CourseId == item.Id && a.AttendeeId == userId).FirstOrDefault();
                        if (find == null)
                        {
                            item.isShowGoing = true;
                        }
                        Following findFollow = _dbContext.Followings.FirstOrDefault(p => p.FolloweeId == userId && p.FollowerId == item.LecturerId);
                        if (findFollow == null)
                        {
                            item.isShowFollow = true;
                        }
                    }
                    totalCourses.Add(item);
                }
            }

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = totalCourses,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(viewModel);

        }
    }
}