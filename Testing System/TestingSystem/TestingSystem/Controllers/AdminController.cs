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

        [HttpGet]
        public ActionResult AddNewQuestion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewQuestion(string questionText, string answer1Text, string answer2Text, string answer3Text, string answer4Text, int correctAnswerRadio)
        {
            var context = new TestingSystemEntities();
            context.AddQuestion(questionText);

            int qId = context.Questions.ToList().Last().QuestionId;
            context.AddAnswers(qId, answer1Text, answer2Text, answer3Text, answer4Text);

            int lastAnswerId = context.Answers.ToList().Last().AnswerId;
            context.AddCorrectAnswer(qId, lastAnswerId + correctAnswerRadio - 4);

            context.SaveChanges();
            return RedirectToAction("EditQuestions");
        }

        [HttpGet]
        public ActionResult EditQuestions()
        {
            var context = new TestingSystemEntities();
            var questions = context.Questions.ToList();
            var answers = context.Answers.ToList();
            ViewBag.Questions = questions;
            ViewBag.Answers = answers;
            return View();
        }

        [HttpPost]
        public ActionResult EditQuestions(int questionIdToEdit)
        {
            var context = new TestingSystemEntities();
            var questions = context.Questions.ToList();
            var answers = context.Answers.ToList();
            ViewBag.Questions = questions;
            ViewBag.Answers = answers;
            return RedirectToAction("EditQuestion");
        }

        [HttpGet]
        public ActionResult EditQuestion(int questionIdToEdit)
        {
            var context = new TestingSystemEntities();
            var questionToEdit = from question in context.Questions.ToList()
                                 where question.QuestionId == questionIdToEdit
                                 select question.QuestionValue;
            var answersToEdit = from answer in context.Answers.ToList()
                                 where answer.Question.QuestionId == questionIdToEdit
                                 select answer.AnswerValue;
            ViewBag.QuestionIdToEdit = questionIdToEdit;
            ViewBag.Question = questionToEdit.ToList();
            ViewBag.Answers = answersToEdit.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditQuestion(int qToEdit, string questionText, string answer1Text, string answer2Text, string answer3Text, string answer4Text, int correctAnswerRadio)
        {
            var context = new TestingSystemEntities();
            var editingQuestion = (from question in context.Questions.ToList()
                                 where question.QuestionId == qToEdit
                                 select question).First();

            var editingAnswer = (from answer in context.Answers.ToList()
                                where answer.Question.QuestionId == qToEdit
                                select answer).ToList();

            editingQuestion.QuestionValue = questionText;
            editingAnswer[0].AnswerValue = answer1Text;
            editingAnswer[1].AnswerValue = answer2Text;
            editingAnswer[2].AnswerValue = answer3Text;
            editingAnswer[3].AnswerValue = answer4Text;
            context.SaveChanges();

            return RedirectToAction("EditQuestions");
        }

        [HttpPost]
        public ActionResult DeleteQuestion(int questionIdToDel)
        {
            var context = new TestingSystemEntities();

            context.DelQuestionAndAnswers(questionIdToDel);
            context.SaveChanges();
            return RedirectToAction("EditQuestions");
        }

        [HttpPost]
        public ActionResult DeleteResult(int resultIdToDel)
        {
            var context = new TestingSystemEntities();
            var resultToDel = from result in context.Results where result.Id == resultIdToDel select result;
            
            context.Results.Remove(resultToDel.ToList()[0]);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteAllResults()
        {
            var context = new TestingSystemEntities();

            context.Results.RemoveRange(context.Results.ToList());
            context.SaveChanges();
            return RedirectToAction("Index");
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