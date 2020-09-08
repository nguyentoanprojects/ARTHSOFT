using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARTHSOFT.Controllers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARTHSOFT.Controllers.Helpers.Tests
{
    [TestClass()]
    public class EmailTests
    {
        [TestMethod()]
        public void SendEmailTest()
        {
            Controllers.Helpers.Email.SendEmail(new Models.Candidacy());
        }
    }
}