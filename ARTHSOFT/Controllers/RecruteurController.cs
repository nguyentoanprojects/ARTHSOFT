using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ARTHSOFT.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ARTHSOFT.Controllers
{
    public class RecruteurController : Controller
    {
        public SqlConnection connectionDbLocal { get; set; }

        public RecruteurController()
        {
            connectionDbLocal = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=aspnet-ARTHSOFT-20200904103221;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connectionDbLocal.Open();
        }

        //
        // GET: /Recruteur/Recruteur
        [AllowAnonymous]
        public ActionResult Recruteur()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCandidacies()
        {
            List<object> result = new List<object>();

            String request = @"SELECT Contact.LastName, Contact.FirstName, Contact.Email, Contact.PhoneNumber, Candidacy.Date 
                FROM Candidacy
                INNER JOIN Contact ON Candidacy.IdContact = Contact.Id
                ORDER BY Candidacy.Date DESC";

            using (SqlCommand command = new SqlCommand(request, connectionDbLocal))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(
                        new {
                            LastName = Convert.ToString(reader.GetValue(0)),
                            FirstName = Convert.ToString(reader.GetValue(1)),
                            Email = Convert.ToString(reader.GetValue(2)),
                            PhoneNumber = Convert.ToString(String.Concat('0', reader.GetValue(3))),
                            Date = Convert.ToDateTime(reader.GetValue(4)).ToString("dd/MM/yyyy à HH:mm"),
                        });
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
