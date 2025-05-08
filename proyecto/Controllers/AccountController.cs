using proyecto.Bussines;
using proyecto.DTOs;
using System.Linq;
using System.Web.Mvc;
using DNF.Security.Bussines;

namespace proyecto.Controllers
{
    [Authenticated]
    public class AccountController : Controller
    {
        public ActionResult Profile()
        {
            User currentUser = Current.User;
            var currentProfile = new ProfileDTO
            {
                UserName = currentUser.FullName,
                UserEmail = currentUser.Email,
                UserRol = string.Join(", ", currentUser.Profiles.Select(p => p.Name)),
                UserRolId = (int)currentUser.Profiles.First().Id
            };

            if (currentUser.Profiles.Any(p => p.Id == 2))
            {
                var currentAuditor = Auditor.Dao.GetByUser(currentUser.Id);
                var auditorStatus = AuditorStatus.Dao.Get(currentAuditor.AuditorStatus.Id);

                currentProfile.FileNumber = currentAuditor.FileNumber;
                currentProfile.StartDate = currentAuditor.StartDate;
                currentProfile.StartDateString = currentAuditor.StartDate.ToString("yyyy-MM-dd");
                currentProfile.Status = auditorStatus.Name;
                currentProfile.UserRolId = 2;
                //currentProfile.UserRol = "Auditor";
            }
            else if (currentUser.Profiles.Any(p => p.Id == 4))
            {
                var currentResponsible = Responsible.Dao.GetByUser(currentUser.Id);
                var responsibleStatus = AuditorStatus.Dao.Get(currentResponsible.ResponsibleStatus.Id);
                var responsibleDepartment = Department.Dao.Get(currentResponsible.Department.Id);

                currentProfile.FileNumber = currentResponsible.FileNumber;
                currentProfile.StartDate = currentResponsible.StartDate;
                currentProfile.StartDateString = currentResponsible.StartDate.ToString("yyyy-MM-dd");
                currentProfile.Status = responsibleStatus.Name;
                currentProfile.Department = responsibleDepartment.Name;
                currentProfile.UserRolId = 4;
                //currentProfile.UserRol = "Responsable";
            }

            return View(currentProfile);
        }        
    }
}