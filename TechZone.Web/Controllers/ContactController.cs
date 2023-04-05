using System.Web.Mvc;
using TechZone.Model.Models;
using TechZone.Service;
using TechZone.Web.Mappings;
using TechZone.Web.Models;

namespace TechZone.Web.Controllers
{
    public class ContactController : Controller
    {
        private IContactDetailService _contactDetailService;
        private IMappingService _mappingService;

        public ContactController(IContactDetailService contactDetailService, IMappingService mappingService)
        {
            this._contactDetailService = contactDetailService;
            this._mappingService = mappingService;
        }

        // GET: Contact
        public ActionResult Index()
        {
            var model = _contactDetailService.GetDefaultContact();
            var contactViewModel = _mappingService.Mapper.Map<ContactDetail, ContactDetailViewModel>(model);
            return View(contactViewModel);
        }
    }
}