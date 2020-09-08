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
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ARTHSOFT.Controllers
{
    public class CandidatController : Controller
    {
        public SqlConnection connectionDbLocal { get; set; }

        public CandidatController()
        {
            connectionDbLocal = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=aspnet-ARTHSOFT-20200904103221;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connectionDbLocal.Open();
        }

        //
        // GET: /Candidat/Candidat
        [AllowAnonymous]
        public ActionResult Candidat()
        {
            return View();
        }

        //
        // POST: /Candidat/Apply
        [HttpPost]
        public async Task<ActionResult> Apply()
        {
            Result result = new Result();

            Candidacy candidacy = new Candidacy();

            if (HttpContext.Request.Form.AllKeys.Any())
            {
                String dataCandidacy = HttpContext.Request.Form["Candidacy"];
                candidacy = JsonConvert.DeserializeObject<Candidacy>(dataCandidacy);
            }

            if (HttpContext.Request.Files.AllKeys.Any())
            {
                HttpPostedFileBase httpPostedFileImage = HttpContext.Request.Files["UploadImage"];
                if(httpPostedFileImage != null)
                {
                    MemoryStream msAvatar = new MemoryStream();
                    httpPostedFileImage.InputStream.CopyTo(msAvatar);

                    candidacy.Avatar = msAvatar;
                }

                HttpPostedFileBase httpPostedFileCV = HttpContext.Request.Files["UploadCV"];
                if(httpPostedFileCV != null)
                {
                    MemoryStream msCV = new MemoryStream();
                    httpPostedFileCV.InputStream.CopyTo(msCV);

                    candidacy.CV = msCV;
                }
            }

            if (candidacy == null)
            {
                return Json(new Result()
                {
                    Status = false,
                    Error = "Le modèle de données renvoi null"
                }, JsonRequestBehavior.AllowGet);
            }

            if (candidacy.Contact == null)
            {
                return Json(new Result()
                {
                    Status = false,
                    Error = "Des informations sont incorects dans vos coordonnées"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!AddContact(candidacy.Contact))
            {
                return Json(new Result()
                {
                    Status = false,
                    Error = "L'ajout en base de vos coordonnées a échoué"
                }, JsonRequestBehavior.AllowGet);
            }

            int idContact = GetIdContact(candidacy.Contact.Email);
            if (idContact == -1)
            {
                return Json(new Result()
                {
                    Status = false,
                    Error = "La récupération de l'id de vos coordonnées a échoué"
                }, JsonRequestBehavior.AllowGet);
            }

            using (SqlCommand command = new SqlCommand(String.Empty, connectionDbLocal))
            {
                command.CommandText = "INSERT INTO [Candidacy] VALUES(@IdContact, @Formations, @Experiences, @Date, @Skills, @OtherInformation)";

                command.Parameters.AddWithValue("@IdContact", idContact);
                command.Parameters.AddWithValue("@Formations", String.Join(", ", candidacy.Formations.Select(f => f.Value)));
                command.Parameters.AddWithValue("@Experiences", String.Join(", ", candidacy.Experiences.Select(e => e.Value)));
                command.Parameters.AddWithValue("@Date", DateTime.Now);
                command.Parameters.AddWithValue("@Skills", candidacy.Skills);
                command.Parameters.AddWithValue("@OtherInformation", candidacy.OtherInformation);

                if(command.ExecuteNonQuery() > 0)
                {
                    await Helpers.Email.SendEmail(candidacy);
                }
                else
                {
                    return Json(new Result()
                    {
                        Status = false,
                        Error = "Une erreur est survenue lors de l'ajout en base de votre candidature"
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult VerifyContact(String email)
        {
            int result = GetIdContact(email);
            if(result > -1)
            {
                return Json(new Result()
                {
                    Status = false,
                    Error = "Vous avez déjà émise une candidature avec cette adresse email"
                }, JsonRequestBehavior.AllowGet);
            }


            return Json(new Result(), JsonRequestBehavior.AllowGet);
        }


        private bool AddContact(Contact contact) 
        {
            bool result = false;

            if (contact != null)
            {
                using (SqlCommand command = new SqlCommand(String.Empty, connectionDbLocal))
                {
                    command.CommandText = "INSERT INTO [Contact] VALUES(@LastName, @Firstname, @Email, @PhoneNumber, @Address, @City, @ZIP)";

                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@Firstname", contact.FirstName);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", contact.Address);
                    command.Parameters.AddWithValue("@City", contact.City);
                    command.Parameters.AddWithValue("@ZIP", contact.ZIP);

                    result = (command.ExecuteNonQuery() > 0);
                }
            }

            return result;
        }


        private int GetIdContact(String email)
        {
            int result = -1;

            using (SqlCommand command = new SqlCommand(String.Empty, connectionDbLocal))
            {
                command.CommandText = "SELECT Id FROM [Contact] WHERE Email=@email";
                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return Convert.ToInt32(reader.GetValue(0));
                    }
                }
            }

            return result;
        }
    }
}
