using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace BigSchool.Controllers.Api
{
    public class CoursesController : ApiController
    {
        private ApplicationDbContext _dbContext;

        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }

        [Authorize]
        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Courses.Single(c => c.Id == id && c.LecturerId == userId);

            if (courses.IsCanceled)
            {
                return NotFound();
            }

            courses.IsCanceled = true;
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}