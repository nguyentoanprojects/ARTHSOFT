using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARTHSOFT.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARTHSOFT.Controllers.Tests
{
    [TestClass()]
    public class CandidatControllerTests
    {
        public CandidatController db { get; set; }

        public CandidatControllerTests()
        {
            db = new CandidatController();
        }

        [TestMethod()]
        public void CandidatControllerTest()
        {
            db.Apply();
        }

        [TestMethod()]
        public void VerifyContactTest()
        {
            db.VerifyContact("aa@bb.fr");
        }
    }
}