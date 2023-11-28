using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InstantCashService.ModelClasses
{
    public class IcPayStatus
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

        [XmlRoot(ElementName = "Details", Namespace = "ICWS")]
        public class Details
        {
            [XmlElement(ElementName = "ICTC_Number", Namespace = "ICWS")]
            public string ICTC_Number { get; set; }
            [XmlElement(ElementName = "Agent_OrderNumber", Namespace = "ICWS")]
            public string Agent_OrderNumber { get; set; }
            [XmlElement(ElementName = "Transaction_Status", Namespace = "ICWS")]
            public string Transaction_Status { get; set; }
            [XmlElement(ElementName = "Status_Description", Namespace = "ICWS")]
            public string Status_Description { get; set; }
            [XmlElement(ElementName = "Extra_Flag1", Namespace = "ICWS")]
            public string Extra_Flag1 { get; set; }
        }

        [XmlRoot(ElementName = "GetIctcPayStatusResult", Namespace = "ICWS")]
        public class GetIctcPayStatusResult
        {
            [XmlElement(ElementName = "Result", Namespace = "ICWS")]
            public Result Result { get; set; }
            [XmlElement(ElementName = "Details", Namespace = "ICWS")]
            public Details Details { get; set; }
        }

        [XmlRoot(ElementName = "GetIctcPayStatusResponse", Namespace = "ICWS")]
        public class GetIctcPayStatusResponse
        {
            [XmlElement(ElementName = "GetIctcPayStatusResult", Namespace = "ICWS")]
            public GetIctcPayStatusResult GetIctcPayStatusResult { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Body
        {
            [XmlElement(ElementName = "GetIctcPayStatusResponse", Namespace = "ICWS")]
            public GetIctcPayStatusResponse GetIctcPayStatusResponse { get; set; }
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
