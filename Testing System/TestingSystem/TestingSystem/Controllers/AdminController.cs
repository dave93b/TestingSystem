using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestingSystem.Models;
using WebApplication1.Addition_Classes;

namespace TestingSystem.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var context=new TestingSystemEntities();
            var results = context.Results.ToList();
            results.Reverse();
            ViewBag.stud = results;
            return View();
        }

        public ActionResult AddNewQuestion()
        {
            var context = new TestingSystemEntities();
            var questions = context.Questions.ToList();
            var answers = context.Answers.ToList();
            ViewBag.Questions = questions;
            ViewBag.Answers = answers;
            return View();
        }
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login() //Login View
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckLogin(string login, string password) //Check Login and Password for Authenti
        {
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = UserAuthentication.GetMd5Hash(md5Hash, password); //Get entered password hash 
                    bool passwordsSame, loginsSame;
                    if (UserAuthentication.VerifyMd5Hash(md5Hash, "25f9e794323b453885f5181f1b624d0b", hash))
                    {
                        passwordsSame = true;
                    } // The hashes are the same.
                    else
                    {
                        passwordsSame = false;
                    } // The hashes are not same.
                    if (UserAuthentication.VerifyLogin(login, "Admin"))
                    {
                        loginsSame = true;
                    }
                    else
                    {
                        loginsSame = false;
                    }
                    if (UserAuthentication.CheckLoginAndPassword(loginsSame, passwordsSame))
                    {
                        FormsAuthentication.SetAuthCookie(login, true);
                        return RedirectToAction("Index", "Admin");
                    }
                    return RedirectToAction("Login", "Admin");
                
            }
        }

        public ActionResult Logout() //Admin Logout
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }
    }
}