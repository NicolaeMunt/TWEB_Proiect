using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using TWEB_Proiect.Models;
using TWEB_Proiect.Data;
using TWEB_Proiect.Domain.Entities;
using TWEB_Proiect.Attributes;

namespace TWEB_Proiect.Controllers
{
     public class AnswerController : Controller
     {
          private ApplicationDbContext db = new ApplicationDbContext();

          // POST: Answer/Create - Создание ответа (только для авторизованных пользователей)
          [HttpPost]
          [ValidateAntiForgeryToken]
          [SessionAuthorize]
          public ActionResult Create(AnswerViewModel model)
          {
               if (ModelState.IsValid)
               {
                    try
                    {
                         var userId = (int)Session["UserId"];
                         var user = db.Users.FirstOrDefault(u => u.Id == userId);

                         if (user == null)
                         {
                              TempData["ErrorMessage"] = "Utilizatorul nu a fost găsit.";
                              return RedirectToAction("Details", "Question", new { id = model.QuestionId });
                         }

                         // Проверяем, существует ли вопрос
                         var question = db.Questions.FirstOrDefault(q => q.Id == model.QuestionId);
                         if (question == null)
                         {
                              TempData["ErrorMessage"] = "Întrebarea nu a fost găsită.";
                              return RedirectToAction("List", "Question");
                         }

                         // Обработка загруженного файла
                         string attachmentPath = null;
                         if (model.Attachment != null && model.Attachment.ContentLength > 0)
                         {
                              string uploadDir = Server.MapPath("~/Content/uploads/answers/");
                              if (!Directory.Exists(uploadDir))
                              {
                                   Directory.CreateDirectory(uploadDir);
                              }

                              string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Attachment.FileName);
                              string filePath = Path.Combine(uploadDir, fileName);

                              model.Attachment.SaveAs(filePath);
                              attachmentPath = "~/Content/uploads/answers/" + fileName;
                         }

                         // Создаем ответ
                         var answer = new Answer
                         {
                              Content = model.Content,
                              QuestionId = model.QuestionId,
                              UserId = userId,
                              Username = user.Username,
                              Email = user.Email,
                              CreatedDate = DateTime.Now,
                              AttachmentPath = attachmentPath,
                              IsAccepted = false
                         };

                         db.Answers.Add(answer);

                         // Обновляем счетчик ответов в вопросе
                         question.Answers++;

                         db.SaveChanges();

                         TempData["SuccessMessage"] = "Răspunsul tău a fost publicat cu succes!";
                         return RedirectToAction("Details", "Question", new { id = model.QuestionId });
                    }
                    catch (Exception ex)
                    {
                         TempData["ErrorMessage"] = "Eroare la salvarea răspunsului: " + ex.Message;
                    }
               }

               // Если есть ошибки валидации, возвращаемся к деталям вопроса
               TempData["ErrorMessage"] = "Te rugăm să completezi toate câmpurile obligatorii.";
               return RedirectToAction("Details", "Question", new { id = model.QuestionId });
          }

          // POST: Answer/Accept - Принятие ответа как правильного (только автор вопроса)
          [HttpPost]
          [ValidateAntiForgeryToken]
          [SessionAuthorize]
          public ActionResult Accept(int answerId)
          {
               try
               {
                    var userId = (int)Session["UserId"];
                    var answer = db.Answers.Include(a => a.Question).FirstOrDefault(a => a.Id == answerId);

                    if (answer == null)
                    {
                         TempData["ErrorMessage"] = "Răspunsul nu a fost găsit.";
                         return RedirectToAction("List", "Question");
                    }

                    // Проверяем, что текущий пользователь - автор вопроса
                    if (answer.Question.UserId != userId)
                    {
                         TempData["ErrorMessage"] = "Doar autorul întrebării poate accepta răspunsurile.";
                         return RedirectToAction("Details", "Question", new { id = answer.QuestionId });
                    }

                    // Сбрасываем все принятые ответы для этого вопроса
                    var allAnswers = db.Answers.Where(a => a.QuestionId == answer.QuestionId).ToList();
                    foreach (var ans in allAnswers)
                    {
                         ans.IsAccepted = false;
                    }

                    // Принимаем выбранный ответ
                    answer.IsAccepted = true;
                    answer.Question.IsResolved = true;

                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Răspunsul a fost acceptat ca fiind corect!";
                    return RedirectToAction("Details", "Question", new { id = answer.QuestionId });
               }
               catch (Exception ex)
               {
                    TempData["ErrorMessage"] = "Eroare la acceptarea răspunsului: " + ex.Message;
                    return RedirectToAction("List", "Question");
               }
          }

          // POST: Answer/Delete - Удаление ответа (только автор ответа или автор вопроса)
          [HttpPost]
          [ValidateAntiForgeryToken]
          [SessionAuthorize]
          public ActionResult Delete(int answerId)
          {
               try
               {
                    var userId = (int)Session["UserId"];
                    var answer = db.Answers.Include(a => a.Question).FirstOrDefault(a => a.Id == answerId);

                    if (answer == null)
                    {
                         TempData["ErrorMessage"] = "Răspunsul nu a fost găsit.";
                         return RedirectToAction("List", "Question");
                    }

                    // Проверяем права на удаление (автор ответа или автор вопроса)
                    if (answer.UserId != userId && answer.Question.UserId != userId)
                    {
                         TempData["ErrorMessage"] = "Nu aveți permisiunea să ștergeți acest răspuns.";
                         return RedirectToAction("Details", "Question", new { id = answer.QuestionId });
                    }

                    var questionId = answer.QuestionId;
                    var question = answer.Question;

                    // Удаляем файл вложения, если есть
                    if (!string.IsNullOrEmpty(answer.AttachmentPath))
                    {
                         var filePath = Server.MapPath(answer.AttachmentPath);
                         if (System.IO.File.Exists(filePath))
                         {
                              System.IO.File.Delete(filePath);
                         }
                    }

                    db.Answers.Remove(answer);

                    // Обновляем счетчик ответов
                    question.Answers--;
                    if (question.Answers < 0) question.Answers = 0;

                    // Если удаляемый ответ был принят, снимаем статус "решено" с вопроса
                    if (answer.IsAccepted)
                    {
                         question.IsResolved = false;
                    }

                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Răspunsul a fost șters cu succes.";
                    return RedirectToAction("Details", "Question", new { id = questionId });
               }
               catch (Exception ex)
               {
                    TempData["ErrorMessage"] = "Eroare la ștergerea răspunsului: " + ex.Message;
                    return RedirectToAction("List", "Question");
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