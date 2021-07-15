using BigSchool.Models;
using BigSchool.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;

        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var upcommingCourses = _dbContext.Courses
                 .Include(c => c.Lecturer)
                 .Include(c => c.Category)
                 .Where(c => c.DateTime > DateTime.Now);

            var userId = User.Identity.GetUserId();

            foreach (Course item in upcommingCourses)
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
                UpcommingCourses = upcommingCourses,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}