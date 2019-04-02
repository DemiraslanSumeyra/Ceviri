using BLL;
using Entity;
using Entity.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace MVCSozluk.Controllers
{
    public class MembersController : Controller
    {
        UnitOfWork _uw = new UnitOfWork();

        // GET: Members
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();


            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public ActionResult ChangePass()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(string oldp, string newp)
        {
            string uid = User.Identity.GetUserId();
            UserStore<Person> store = new UserStore<Person>(_uw.db);
            UserManager<Person> manager = new UserManager<Person>(store);
            Person p = manager.FindById(uid);
            bool isCorrect = manager.CheckPassword(p, oldp);
            if (isCorrect)
            {
                IdentityResult result = manager.ChangePassword(uid, oldp, newp);
                if (result.Succeeded)
                {
                    ViewBag.Success = true;
                }
                else
                {
                    ViewBag.Error = result.Errors;
                }

            }
            else
            {
                ViewBag.WrongPass = true;
            }
            return View();
        }
        [HttpGet]
        public ActionResult MyAccount()
        {
            string uid = User.Identity.GetUserId();
            Person person = _uw.db.Users.Find(uid);
            if (person.HasPhoto)
            {
                ViewBag.Photo = "/Uploads/Members/" + uid + ".jpg";
            }
            MyAccontViewModel vm = new MyAccontViewModel();
            vm.Email = person.Email;
            vm.PhoneNumber = person.PhoneNumber;
            return View(vm);
        }
        [HttpPost]
        public ActionResult MyAccount(MyAccontViewModel info, HttpPostedFileBase imgFile)
        {
            UserStore<Person> store = new UserStore<Person>(UnitOfWork.Create());
            UserManager<Person> manager = new UserManager<Person>(store);

            string uid = User.Identity.GetUserId();
            Person person = manager.FindById(uid);
            person.Email = info.Email;
            person.PhoneNumber = info.PhoneNumber;

            if (imgFile != null)
            {
                string path = Server.MapPath("/Uploads/Members/");
                string old = path + person.Id + ".jpg";
                if (System.IO.File.Exists(old))
                {
                    System.IO.File.Exists(old);
                }
                string _new = path + person.Id + ".jpg";
                imgFile.SaveAs(_new);
                person.HasPhoto = true;
            }
            manager.Update(person);

            if (person.HasPhoto)
            {
                ViewBag.Photo = "/Uploads/Members/" + uid + ".jpg";
            }
            return View(info);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//Sahteciliğe karşı formu kontrol ediyor. Action da karşılama kısmı
        public ActionResult Register(Person person, string password, HttpPostedFileBase Image)
        {
            UserStore<Person> store = new UserStore<Person>(UnitOfWork.Create());
            UserManager<Person> manager = new UserManager<Person>(store);
            var result = manager.Create(person, password);
            if (result.Succeeded)
            {
                if (Image != null)
                {
                    string path = Server.MapPath("/Uploads/Members/");
                    Image.SaveAs(path + person.Id + ".jpg");
                    person.HasPhoto = true;
                    manager.Update(person);


                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Errors = result.Errors;
            }
            return View();
        }
        public ActionResult _LoginModal()
        {
            return View();
        }
        public JsonResult Login(LoginViewModel info)
        {
            ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            SignInStatus result = signInManager.PasswordSignIn(info.Email, info.Password, true, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Json(new { success = true });
                case SignInStatus.Failure:
                    return Json(new { success = false });

            }
            return Json(new { success = false });
        }
    }
}