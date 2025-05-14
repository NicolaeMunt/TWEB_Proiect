using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using TWEB_Proiect.Models;
using TWEB_Proiect.Data;
using TWEB_Proiect.Domain.Entities; // Для Question и User классов

namespace TWEB_Proiect.Controllers
{
    public class QuestionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Question/Ask
        public ActionResult Ask()
        {
            var model = new QuestionViewModel();

            // Если пользователь авторизован, предзаполняем поля
            if (Session["UserId"] != null)
            {
                try
                {
                    var userId = (int)Session["UserId"];
                    var user = db.Users.FirstOrDefault(u => u.Id == userId);
                    if (user != null)
                    {
                        model.Username = user.Username;
                        model.Email = user.Email;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Eroare la încărcarea datelor utilizatorului: " + ex.Message;
                }
            }

            return View(model);
        }

        // POST: Question/Ask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ask(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Обработка загруженного файла
                    string attachmentPath = null;
                    if (model.Attachment != null && model.Attachment.ContentLength > 0)
                    {
                        string uploadDir = Server.MapPath("~/Content/uploads/questions/");
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Attachment.FileName);
                        string filePath = Path.Combine(uploadDir, fileName);

                        model.Attachment.SaveAs(filePath);
                        attachmentPath = "~/Content/uploads/questions/" + fileName;
                    }

                    // Создаем объект вопроса для базы данных
                    var question = new TWEB_Proiect.Domain.Entities.Question
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Title = model.QuestionTitle,
                        Category = model.Category,
                        Details = model.QuestionDetails,
                        AttachmentPath = attachmentPath,
                        CreatedDate = DateTime.Now,
                        UserId = Session["UserId"] as int?,
                        Views = 0,
                        Answers = 0,
                        IsResolved = false
                    };

                    db.Questions.Add(question);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Întrebarea ta a fost publicată cu succes!";
                    // ИЗМЕНЕНО: перенаправляем на главную страницу
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Eroare la salvarea întrebării: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Question/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var question = db.Questions.FirstOrDefault(q => q.Id == id);

                if (question != null)
                {
                    question.Views++;
                    db.SaveChanges();
                    return View(question);
                }

                return View((TWEB_Proiect.Domain.Entities.Question)null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Eroare: " + ex.Message;
                return View((TWEB_Proiect.Domain.Entities.Question)null);
            }
        }

        // GET: Question/List
        public ActionResult List()
        {
            try
            {
                var questions = db.Questions
                    .OrderByDescending(q => q.CreatedDate)
                    .ToList();
                return View(questions);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Eroare la încărcarea întrebărilor: " + ex.Message;
                return View(new List<TWEB_Proiect.Domain.Entities.Question>());
            }
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
}