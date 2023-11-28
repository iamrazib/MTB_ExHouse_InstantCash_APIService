using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InstantCashService.ModelClasses
{
    public class ConfirmTransaction
    {
        [XmlRoot(ElementName = "Result", Namespace = "ICWS")]
        public class Result
        {
            [XmlElement(ElementName = "Result_Flag", Namespace = "ICWS")]
            public string Result_Flag { get; set; }
            [XmlElement(ElementName = "Error_Code", Namespace = "ICWS")]
            public string Error_Code { get; set; }
            [XmlElement(ElementName = "Error_Message", Namespace = "ICWS")]
            public string Error_Message { get; set; }
            [XmlElement(ElementName = "Error_Description", Namespace = "ICWS")]
            public string Error_Description { get; set; }
        }

        [XmlRoot(ElementName = "Confirmation", Namespace = "ICWS")]
        public class Confirmation
        {
            [XmlElement(ElementName = "ICTC_Number", Namespace = "ICWS")]
            public string ICTC_Number { get; set; }
            [XmlElement(ElementName = "Confirmed", Namespace = "ICWS")]
            public string Confirmed { get; set; }
            [XmlElement(ElementName = "Description", Namespace = "ICWS")]
            public string Description { get; set; }
        }

        [XmlRoot(ElementName = "Records", Namespace = "ICWS")]
        public class Records
        {
            [XmlElement(ElementName = "Confirmation", Namespace = "ICWS")]
            public Confirmation Confirmation { get; set; }
        }

        [XmlRoot(ElementName = "ConfirmTranResult", Namespace = "ICWS")]
        public class ConfirmTranResult
        {
            [XmlElement(ElementName = "Result", Namespace = "ICWS")]
            public Result Result { get; set; }
            [XmlElement(ElementName = "Records", Namespace = "ICWS")]
            public Records Records { get; set; }
        }

        [XmlRoot(ElementName = "ConfirmTranResponse", Namespace = "ICWS")]
        public class ConfirmTranResponse
        {
            [XmlElement(ElementName = "ConfirmTranResult", Namespace = "ICWS")]
            public ConfirmTranResult ConfirmTranResult { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Body
        {
            [XmlElement(ElementName = "ConfirmTranResponse", Namespace = "ICWS")]
            public ConfirmTranResponse ConfirmTranResponse { get; set; }
        }

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
            [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Soap { get; set; }
            [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Xsi { get; set; }
            [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Xsd { get; set; }
        }

    }
}
