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
                UserRol = "Administrador"
            };

            if (currentUser.Profiles.Any(p => p.Id == 2))
            {
                var currentAuditor = Auditor.Dao.GetByUser(currentUser.Id);
                var auditorStatus = AuditorStatus.Dao.Get(currentAuditor.AuditorStatus.Id);

                currentProfile.FileNumber = currentAuditor.FileNumber;
                currentProfile.StartDate = currentAuditor.StartDate;
                currentProfile.StartDateString = currentAuditor.StartDate.ToString("yyyy-MM-dd");
                currentProfile.Status = auditorStatus.Name;

                currentProfile.UserRol = "Auditor";
            }
            else if (currentUser.Profiles.Any(p => p.Id == 4))
            {
                var currentResponsible = Responsible.Dao.GetByUser(currentUser.Id);
                var responsibleStatus = AuditorStatus.Dao.Get(currentResponsible.ResponsibleStatus.Id);

                currentProfile.FileNumber = currentResponsible.FileNumber;
                currentProfile.StartDate = currentResponsible.StartDate;
                currentProfile.StartDateString = currentResponsible.StartDate.ToString("yyyy-MM-dd");
                currentProfile.Status = responsibleStatus.Name;

                currentProfile.UserRol = "Responsable";
            }

            return View(currentProfile);
        }        
    }
}