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

            //var qId = questions[qNumber - 1].QuestionId;
            var answers = from answer in context.Answers.ToList()
                where answer.Question.QuestionId == questions[qNumber - 1].QuestionId
                          select answer.AnswerValue;
            var answersId = from answer in context.Answers.ToList()
                          where answer.Question.QuestionId == questions[qNumber - 1].QuestionId
                            select answer.AnswerId;

            if (answerFromClient != null)
            {
                var correctAnswer = from correct in context.CorrectAnswers.ToList()
                    where correct.Question.QuestionId == questions[qNumber - 2].QuestionId
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
                int totalQ = context.Questions.Count();
                Result result = new Result();
                result.Name = Session["FirstName"].ToString();
                result.LastName = Session["LastName"].ToString();
                result.Group = Session["Group"].ToString();
                result.PCName = Request.UserHostName;
                result.DateAndTime = DateTime.Now;
                result.IPAddress = Request.UserHostAddress;
                result.Points = points;
                result.TotalPoints = totalQ;
                result.AnswersPercent = ((decimal)points/(decimal)totalQ)*100;
                context.Results.Add(result);
                context.SaveChanges();

                return Json(new {isFinished = true, result = points, totalQuestions = totalQ });
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string firstName="",string lastName="", string group="", string pcName="")
        {
            Session["FirstName"] = firstName;
            Session["LastName"] = lastName;
            Session["Group"] = group;
            Session["PCName"] = pcName;
            return RedirectToAction("Testing");
        }
    }
}