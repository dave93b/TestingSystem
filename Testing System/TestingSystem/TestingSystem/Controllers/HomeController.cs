using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestingSystem.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Testing()
        {
            Session["QuestionNumber"] = 0;
            Session["Points"] = 0;
            return View();
        }

        [HttpPost]
        public JsonResult Testing(int? answerFromClient)
        {
            int qNumber = Convert.ToInt32(Session["QuestionNumber"]);
            int resultPoints = Convert.ToInt32(Session["Points"]);
            var context = new TestingSystemEntities();
            var questions = context.Questions.ToList();
            qNumber++;

            var answers = from answer in context.Answers.ToList()
                where answer.Question.QuestionId == qNumber
                select answer.AnswerValue;
            var answersId = from answer in context.Answers.ToList()
                          where answer.Question.QuestionId == qNumber
                          select answer.AnswerId;

            if (answerFromClient != null)
            {
                var correctAnswer = from correct in context.CorrectAnswers.ToList()
                    where correct.Question.QuestionId == qNumber - 1
                    select correct.Answer.AnswerId;
                var ca = correctAnswer.ToList();
                if (ca[0] == answerFromClient)
                {
                    resultPoints++;
                    Session["Points"] = resultPoints;
                }
            }

            if (qNumber <= questions.Count)
            {
                object data = new object[]
                {
                    questions[qNumber-1].QuestionValue,
                    answers,
                    answersId
                };
                Session["QuestionNumber"]=qNumber;

                return Json(data);
            }
            else
            {
                int points = Convert.ToInt32(Session["Points"]);
                
                Result result=new Result();
                result.Name = Session["FirstName"].ToString();
                result.LastName = Session["LastName"].ToString();
                result.Group = Session["Group"].ToString();
                result.PCName = Request.UserHostName;
                result.DateAndTime=DateTime.Now;
                result.IPAddress = Request.UserHostAddress;
                result.Points = points;
                context.Results.Add(result);
                context.SaveChanges();

                return Json(new {isFinished = true, result = points});
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string firstName,string lastName, string group, string pcName)
        {
            Session["FirstName"] = firstName;
            Session["LastName"] = lastName;
            Session["Group"] = group;
            Session["PCName"] = pcName;
            return RedirectToAction("Testing");
        }
    }
}