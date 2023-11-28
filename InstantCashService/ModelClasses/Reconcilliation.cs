using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace InstantCashService.ModelClasses
{
    public class Reconcilliation
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

        [XmlRoot(ElementName = "Transaction", Namespace = "ICWS")]
        public class Transaction
        {
            [XmlElement(ElementName = "Transaction_Type", Namespace = "ICWS")]
            public string Transaction_Type { get; set; }
            [XmlElement(ElementName = "ICTC_Number", Namespace = "ICWS")]
            public string ICTC_Number { get; set; }
            [XmlElement(ElementName = "Agent_OrderNumber", Namespace = "ICWS")]
            public string Agent_OrderNumber { get; set; }
            [XmlElement(ElementName = "Agent_UserID", Namespace = "ICWS")]
            public string Agent_UserID { get; set; }
            [XmlElement(ElementName = "Agent_Code", Namespace = "ICWS")]
            public string Agent_Code { get; set; }
            [XmlElement(ElementName = "Agent_Name", Namespace = "ICWS")]
            public string Agent_Name { get; set; }
            [XmlElement(ElementName = "Mode_Description", Namespace = "ICWS")]
            public string Mode_Description { get; set; }
            [XmlElement(ElementName = "Remitter_Name", Namespace = "ICWS")]
            public string Remitter_Name { get; set; }
            [XmlElement(ElementName = "Remitter_Address", Namespace = "ICWS")]
            public string Remitter_Address { get; set; }
            [XmlElement(ElementName = "Originating_Country", Namespace = "ICWS")]
            public string Originating_Country { get; set; }
            [XmlElement(ElementName = "Remitter_TelNo", Namespace = "ICWS")]
            public string Remitter_TelNo { get; set; }
            [XmlElement(ElementName = "Remitter_MobileNo", Namespace = "ICWS")]
            public string Remitter_MobileNo { get; set; }
            [XmlElement(ElementName = "Message_PayeeBranch", Namespace = "ICWS")]
            public string Message_PayeeBranch { get; set; }
            [XmlElement(ElementName = "Remitter_ID_Type", Namespace = "ICWS")]
            public string Remitter_ID_Type { get; set; }
            [XmlElement(ElementName = "Remitter_IDDtl1", Namespace = "ICWS")]
            public string Remitter_IDDtl1 { get; set; }
            [XmlElement(ElementName = "Transaction_Date", Namespace = "ICWS")]
            public string Transaction_Date { get; set; }
            [XmlElement(ElementName = "Transaction_Time", Namespace = "ICWS")]
            public string Transaction_Time { get; set; }
            [XmlElement(ElementName = "Customer_Princ_Amount", Namespace = "ICWS")]
            public string Customer_Princ_Amount { get; set; }
            [XmlElement(ElementName = "FC_Amount", Namespace = "ICWS")]
            public string FC_Amount { get; set; }
            [XmlElement(ElementName = "Settlement_Rate", Namespace = "ICWS")]
            public string Settlement_Rate { get; set; }
            [XmlElement(ElementName = "Total_Commission", Namespace = "ICWS")]
            public string Total_Commission { get; set; }
            [XmlElement(ElementName = "Agent_Share", Namespace = "ICWS")]
            public string Agent_Share { get; set; }
            [XmlElement(ElementName = "Settlement_Amount", Namespace = "ICWS")]
            public string Settlement_Amount { get; set; }
            [XmlElement(ElementName = "Paying_Currency", Namespace = "ICWS")]
            public string Paying_Currency { get; set; }
            [XmlElement(ElementName = "Beneficiary_Name", Namespace = "ICWS")]
            public string Beneficiary_Name { get; set; }
            [XmlElement(ElementName = "Expected_BenefID", Namespace = "ICWS")]
            public string Expected_BenefID { get; set; }
            [XmlElement(ElementName = "Bank_Account_Number", Namespace = "ICWS")]
            public string Bank_Account_Number { get; set; }
            [XmlElement(ElementName = "Bank_Name", Namespace = "ICWS")]
            public string Bank_Name { get; set; }
            [XmlElement(ElementName = "Bank_Address", Namespace = "ICWS")]
            public string Bank_Address { get; set; }
            [XmlElement(ElementName = "Receiving_Agent_Code", Namespace = "ICWS")]
            public string Receiving_Agent_Code { get; set; }
            [XmlElement(ElementName = "Receiving_Agent_Name", Namespace = "ICWS")]
            public string Receiving_Agent_Name { get; set; }
            [XmlElement(ElementName = "Beneficiary_Address", Namespace = "ICWS")]
            public string Beneficiary_Address { get; set; }
            [XmlElement(ElementName = "Beneficiary_Country", Namespace = "ICWS")]
            public string Beneficiary_Country { get; set; }
            [XmlElement(ElementName = "Beneficiary_TelNo", Namespace = "ICWS")]
            public string Beneficiary_TelNo { get; set; }
            [XmlElement(ElementName = "Beneficiary_MobileNo", Namespace = "ICWS")]
            public string Beneficiary_MobileNo { get; set; }
            [XmlElement(ElementName = "Purpose_Remit", Namespace = "ICWS")]
            public string Purpose_Remit { get; set; }
            [XmlElement(ElementName = "Perc_Refund_Comm", Namespace = "ICWS")]
            public string Perc_Refund_Comm { get; set; }
            [XmlElement(ElementName = "Branch_Code", Namespace = "ICWS")]
            public string Branch_Code { get; set; }
            [XmlElement(ElementName = "Transaction_Status", Namespace = "ICWS")]
            public string Transaction_Status { get; set; }
            [XmlElement(ElementName = "Transaction_Status_Description", Namespace = "ICWS")]
            public string Transaction_Status_Description { get; set; }
            [XmlElement(ElementName = "Agent_Exchange_Earning", Namespace = "ICWS")]
            public string Agent_Exchange_Earning { get; set; }
            [XmlElement(ElementName = "IC_Exchange_Earning", Namespace = "ICWS")]
            public string IC_Exchange_Earning { get; set; }
            [XmlElement(ElementName = "Trans_CancelDate", Namespace = "ICWS")]
            public string Trans_CancelDate { get; set; }
            [XmlElement(ElementName = "Trans_CancelTime", Namespace = "ICWS")]
            public string Trans_CancelTime { get; set; }
        }

        [XmlRoot(ElementName = "Records", Namespace = "ICWS")]
        public class Records
        {
            [XmlElement(ElementName = "Transaction", Namespace = "ICWS")]
            public List<Transaction> Transaction { get; set; }
        }

        [XmlRoot(ElementName = "ReconcileResult", Namespace = "ICWS")]
        public class ReconcileResult
        {
            [XmlElement(ElementName = "Result", Namespace = "ICWS")]
            public Result Result { get; set; }
            [XmlElement(ElementName = "Records", Namespace = "ICWS")]
            public Records Records { get; set; }
        }

        [XmlRoot(ElementName = "ReconcileResponse", Namespace = "ICWS")]
        public class ReconcileResponse
        {
            [XmlElement(ElementName = "ReconcileResult", Namespace = "ICWS")]
            public ReconcileResult ReconcileResult { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Body
        {
            [XmlElement(ElementName = "ReconcileResponse", Namespace = "ICWS")]
            public ReconcileResponse ReconcileResponse { get; set; }
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