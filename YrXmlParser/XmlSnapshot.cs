using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FlyableHours
{
    class XmlSnapshot
    {
        public XmlSnapshot(DateTime timeStamp, XmlDocument xml)
        {
            TimeStamp = timeStamp;
            XmlDoc = xml;
        }

        public XmlDocument XmlDoc { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
