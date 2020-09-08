using ARTHSOFT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace ARTHSOFT.Controllers.Helpers
{
    public class Email
    {
        public const String Recipient = "rharthsoft@yopmail.com";

        public static async Task<bool> SendEmail(Candidacy candidacy)
        {
            bool result = false;

            try
            {
                MailMessage message = new MailMessage(candidacy.Contact.Email, Recipient);
                message.Subject = "Nouvelle candidature";
                message.IsBodyHtml = true;
                message.Body = GetBody().Replace("[CANDIDACYNAME]", String.Concat(candidacy.Contact.LastName, " ", candidacy.Contact.FirstName));

                candidacy.Avatar.Seek(0, SeekOrigin.Begin);
                candidacy.CV.Seek(0, SeekOrigin.Begin);

                message.Attachments.Add(new Attachment(candidacy.CV, "CV.pdf"));
                message.Attachments.Add(new Attachment(candidacy.Avatar, "avatar.png"));

                //Définir un SMTP par défaut afin de pouvoir envoyer les emails
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("toan.arthsoft@gmail.com", "toan14arthsoft");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                await smtp.SendMailAsync(message);
            }
            catch (Exception e) 
            { 
                Console.WriteLine(e.ToString());
            }

            return result;
        }

        private static String GetBody()
        {
            return @"<!DOCTYPE html>
                    <html xmlns='http://www.w3.org/1999/xhtml'>
                        <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>
                            <title>Nouvelle candidature </title>
                        </head>
                        <body style='margin: 0; padding: 0;'>            
                            <table border='0' cellpadding='0' cellspacing='0' width='100%'>                   
                                <tr>                   
                                    <td style='padding: 10px 0 30px 0;'>                    
                                        <table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border: 0; border-radius: 15px; box-shadow: 0 10px 30px 0 rgba(172, 168, 168, 0.43);overflow: hidden;'>                               
                                            <tr>
                                               <td align='left' bgcolor='#262626' style='padding: 20px 10px 20px 20px; color: #153643; font-weight: bold; font-family: Arial, sans-serif;'>
                                                  <img src='https://entreprise.pole-emploi.fr/static/img/minisite/odLLAEBLU86feqkeSoHj9qW1JkU9nQGT.png' width='32' height='32' style='display: inline-block;' />                                             
                                                  <span style='display: inline-block; position: relative; top: -10px; left: 10px; color: white'> ARTHSOFT - CANDIDATURE </span>
                                               </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor='#ffffff' style='padding: 40px 30px 40px 30px;'>
                                                    <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                                        <tr>
                                                            <td style='color: #153643; font-family: Arial, sans-serif; font-size: 24px;'>
                                                                <b> Une nouvelle candidature vient d'arrivée</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style='padding: 20px 0 30px 0; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;'>
                                                                [CANDIDACYNAME] vient de déposer sa candidature pour nous rejoindre.<br/><br/>
                                                                Pour plus d'informations, accède au compte administateur de la plateforme de soumission de CV. 
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td bgcolor='#262626' style='padding: 30px 30px 30px 30px;'>
                                                   <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                                        <tr>
                                                           <td style='color: white; font-family: Arial, sans-serif; font-size: 14px;' width='75%'>
                                                                &reg; ARTHSOFT 2020 <br/>
                                                           </td>
                                                           <td align='right' width='25%>
                                                               <table border='0' cellpadding='0' cellspacing='0'>
                                                                   <tr>
                                                                       <td style='font -family: Arial, sans-serif; font-size: 12px; font-weight: bold;'></td>
                                                                       <td style='font-size: 0; line-height: 0;' width='20'> &nbsp;</td>
                                                                       <td style='font-family: Arial, sans-serif; font-size: 12px; font-weight: bold;'></td>
                                                                   </tr>
                                                               </table>
                                                            </td>
                                                          </tr>
                                                    </table>
                                                 </td>
                                              </tr>
                                           </table>
                                        </td>
                                     </tr>
                                  </table>
                               </body>
                          </html> ";
        }


    }
}