using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ARTHSOFT.Models
{
    public class Candidacy
    {
        public Stream CV { get; set; }
        public Stream Avatar { get; set; }
        public Contact Contact { get; set; }
        public List<Element> Formations { get; set; }
        public List<Element> Experiences { get; set; }
        public String Skills { get; set; }
        public String OtherInformation { get; set; }

        public Candidacy()
        {
            CV = new MemoryStream();
            Avatar = new MemoryStream();
            Contact = new Contact();
            Formations = new List<Element>();
            Experiences = new List<Element>();
            Skills = String.Empty;
            OtherInformation = String.Empty;
        }
    }
}