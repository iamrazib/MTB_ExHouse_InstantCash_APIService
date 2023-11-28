using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InstantCashService.ModelClasses
{
    public class OutstandingRemit
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

        [XmlRoot(ElementName = "OutstandingRemittance", Namespace = "ICWS")]
        public class OutstandingRemittance
        {
            [XmlElement(ElementName = "ICTC_Number", Namespace = "ICWS")]
            public string ICTC_Number { get; set; }
            [XmlElement(ElementName = "Agent_OrderNumber", Namespace = "ICWS")]
            public string Agent_OrderNumber { get; set; }
            [XmlElement(ElementName = "Remitter_Name", Namespace = "ICWS")]
            public string Remitter_Name { get; set; }
            [XmlElement(ElementName = "Remitter_Address", Namespace = "ICWS")]
            public string Remitter_Address { get; set; }
            [XmlElement(ElementName = "Remitter_IDType", Namespace = "ICWS")]
            public string Remitter_IDType { get; set; }
            [XmlElement(ElementName = "Remitter_IDDtl", Namespace = "ICWS")]
            public string Remitter_IDDtl { get; set; }
            [XmlElement(ElementName = "Originating_Country", Namespace = "ICWS")]
            public string Originating_Country { get; set; }
            [XmlElement(ElementName = "Delivery_Mode", Namespace = "ICWS")]
            public string Delivery_Mode { get; set; }
            [XmlElement(ElementName = "Paying_Amount", Namespace = "ICWS")]
            public string Paying_Amount { get; set; }
            [XmlElement(ElementName = "PayingAgent_CommShare", Namespace = "ICWS")]
            public string PayingAgent_CommShare { get; set; }
            [XmlElement(ElementName = "Paying_Currency", Namespace = "ICWS")]
            public string Paying_Currency { get; set; }
            [XmlElement(ElementName = "Paying_Agent", Namespace = "ICWS")]
            public string Paying_Agent { get; set; }
            [XmlElement(ElementName = "Paying_AgentName", Namespace = "ICWS")]
            public string Paying_AgentName { get; set; }
            [XmlElement(ElementName = "Beneficiary_Name", Namespace = "ICWS")]
            public string Beneficiary_Name { get; set; }
            [XmlElement(ElementName = "Beneficiary_Address", Namespace = "ICWS")]
            public string Beneficiary_Address { get; set; }
            [XmlElement(ElementName = "Beneficiary_City", Namespace = "ICWS")]
            public string Beneficiary_City { get; set; }
            [XmlElement(ElementName = "Destination_Country", Namespace = "ICWS")]
            public string Destination_Country { get; set; }
            [XmlElement(ElementName = "Beneficiary_TelNo", Namespace = "ICWS")]
            public string Beneficiary_TelNo { get; set; }
            [XmlElement(ElementName = "Beneficiary_MobileNo", Namespace = "ICWS")]
            public string Beneficiary_MobileNo { get; set; }
            [XmlElement(ElementName = "Expected_BenefID", Namespace = "ICWS")]
            public string Expected_BenefID { get; set; }
            [XmlElement(ElementName = "Bank_Address", Namespace = "ICWS")]
            public string Bank_Address { get; set; }
            [XmlElement(ElementName = "Bank_Account_Number", Namespace = "ICWS")]
            public string Bank_Account_Number { get; set; }
            [XmlElement(ElementName = "Bank_Name", Namespace = "ICWS")]
            public string Bank_Name { get; set; }
            [XmlElement(ElementName = "Purpose_Remit", Namespace = "ICWS")]
            public string Purpose_Remit { get; set; }
            [XmlElement(ElementName = "Message_PayeeBranch", Namespace = "ICWS")]
            public string Message_PayeeBranch { get; set; }
            [XmlElement(ElementName = "Bank_BranchCode", Namespace = "ICWS")]
            public string Bank_BranchCode { get; set; }
            [XmlElement(ElementName = "Settlement_Rate", Namespace = "ICWS")]
            public string Settlement_Rate { get; set; }
            [XmlElement(ElementName = "PrinSettlement_Amount", Namespace = "ICWS")]
            public string PrinSettlement_Amount { get; set; }
            [XmlElement(ElementName = "Transaction_SentDate", Namespace = "ICWS")]
            public string Transaction_SentDate { get; set; }
            [XmlElement(ElementName = "Remitter_Nationality", Namespace = "ICWS")]
            public string Remitter_Nationality { get; set; }
            [XmlElement(ElementName = "Remitter_DOB", Namespace = "ICWS")]
            public string Remitter_DOB { get; set; }
            [XmlElement(ElementName = "Remitter_City", Namespace = "ICWS")]
            public string Remitter_City { get; set; }
        }

        [XmlRoot(ElementName = "Records", Namespace = "ICWS")]
        public class Records
        {
            [XmlElement(ElementName = "OutstandingRemittance", Namespace = "ICWS")]
            public List<OutstandingRemittance> OutstandingRemittance { get; set; }
        }

        [XmlRoot(ElementName = "GetOutstandingRemittanceResult", Namespace = "ICWS")]
        public class GetOutstandingRemittanceResult
        {
            [XmlElement(ElementName = "Result", Namespace = "ICWS")]
            public Result Result { get; set; }
            [XmlElement(ElementName = "Records", Namespace = "ICWS")]
            public Records Records { get; set; }
        }

        [XmlRoot(ElementName = "GetOutstandingRemittanceResponse", Namespace = "ICWS")]
        public class GetOutstandingRemittanceResponse
        {
            [XmlElement(ElementName = "GetOutstandingRemittanceResult", Namespace = "ICWS")]
            public GetOutstandingRemittanceResult GetOutstandingRemittanceResult { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Body
        {
            [XmlElement(ElementName = "GetOutstandingRemittanceResponse", Namespace = "ICWS")]
            public GetOutstandingRemittanceResponse GetOutstandingRemittanceResponse { get; set; }
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
