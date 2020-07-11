using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Contenter.Models
{

    [XmlRoot("message")]
    public class ResponseMessage
    {
        [XmlAttribute("state")]
        public string State { get; set; }

        [XmlElement("code")]
        public string Code { get; set; }

        [XmlElement("desc")]
        public string Desc { get; set; }

        [XmlElement("ex")]
        public Exception Ex { get; set; }
    }
}