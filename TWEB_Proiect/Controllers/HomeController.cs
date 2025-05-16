using System;
using System.Linq;
using System.Web.Mvc;
using TWEB_Proiect.Data;
using TWEB_Proiect.Domain.Entities;
using System.Collections.Generic;

namespace TWEB_Proiect.Controllers
{
     public class HomeController : Controller
     {
          private ApplicationDbContext db = new ApplicationDbContext();

          // Перенаправление на Ask вопроса
          public ActionResult Ask_Question()
          {
               return RedirectToAction("Ask", "Question");
          }

          public ActionResult Index()
          {
               ViewBag.Title = "Home Page";

               try
               {
                    // Загружаем данные для разных табов
                    var recentQuestions = db.Questions
                        .OrderByDescending(q => q.CreatedDate)
                        .Take(10)
                        .ToList();

                    var mostAnsweredQuestions = db.Questions
                        .OrderByDescending(q => q.Answers)
                        .Take(10)
                        .ToList();

                    var recentlyAnsweredQuestions = db.Questions
                        .Where(q => q.IsResolved)
                        .OrderByDescending(q => q.CreatedDate)
                        .Take(10)
                        .ToList();

                    var unansweredQuestions = db.Questions
                        .Where(q => q.Answers == 0)
                        .OrderByDescending(q => q.CreatedDate)
                        .Take(10)
                        .ToList();

                    // Создаем ViewModel
                    var viewModel = new HomeViewModel
                    {
                         RecentQuestions = recentQuestions,
                         MostAnsweredQuestions = mostAnsweredQuestions,
                         RecentlyAnsweredQuestions = recentlyAnsweredQuestions,
                         UnansweredQuestions = unansweredQuestions,
                         TotalQuestions = db.Questions.Count(),
                         TotalAnswers = db.Questions.Sum(q => (int?)q.Answers) ?? 0
                    };

                    ViewBag.UserCount = db.Users.Count();
                    ViewBag.QuestionCount = viewModel.TotalQuestions;

                    return View(viewModel);
               }
               catch (Exception ex)
               {
                    // В случае ошибки передаем пустую модель
                    ViewBag.UserCount = 0;
                    ViewBag.QuestionCount = 0;
                    ViewBag.ErrorMessage = "Ошибка загрузки данных: " + ex.Message;

                    var emptyModel = new HomeViewModel
                    {
                         RecentQuestions = new List<Question>(),
                         MostAnsweredQuestions = new List<Question>(),
                         RecentlyAnsweredQuestions = new List<Question>(),
                         UnansweredQuestions = new List<Question>(),
                         TotalQuestions = 0,
                         TotalAnswers = 0
                    };

                    return View(emptyModel);
               }
          }

          // ОБНОВЛЕННЫЙ МЕТОД - теперь возвращает переделанную страницу "О нас"
          public ActionResult About()
          {
               ViewBag.Title = "Despre noi";
               ViewBag.Message = "Despre INJÎNER.MD și echipa noastră";

               // Возвращаем переделанную страницу Contact.cshtml, которая теперь служит как страница "О нас"
               return View("Contact");
          }

          public ActionResult Contact()
          {
               ViewBag.Message = "Your contact page.";
               return View();
          }

          public ActionResult Blog()
          {
               ViewBag.Message = "Blog page.";
               return View();
          }

          protected override void Dispose(bool disposing)
          {
               if (disposing)
               {
                    db.Dispose();
               }
               base.Dispose(disposing);
          }
     }

     // ViewModel для главной страницы
     public class HomeViewModel
     {
          public List<Question> RecentQuestions { get; set; } = new List<Question>();
          public List<Question> MostAnsweredQuestions { get; set; } = new List<Question>();
          public List<Question> RecentlyAnsweredQuestions { get; set; } = new List<Question>();
          public List<Question> UnansweredQuestions { get; set; } = new List<Question>();
          public int TotalQuestions { get; set; }
          public int TotalAnswers { get; set; }
     }
}