using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace MVC3RestAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    //[Serializable, XmlRoot("User"), DataContract(Name="User")]
    //public class User
    //{
    //    [XmlElement("Id"), DataMember]
    //    public int Id { get; set; }
    //    [XmlElement("Email"), DataMember]
    //    public string Email { get; set; }
    //    [XmlElement("Name"), DataMember]
    //    public string Name { get; set; }
    //}
}
