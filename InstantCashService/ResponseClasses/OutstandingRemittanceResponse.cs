using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantCashService.ResponseClasses
{
    public class OutstandingRemittanceResponse
    {
        public string Result_Flag { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string Error_Description { get; set; }

        public List<OutstandingRemits> outstandingRemittanceList { get; set; }
    }      

    public class OutstandingRemits
    {
        public string ICTC_Number { get; set; }
        public string Agent_OrderNumber { get; set; }
        public string Remitter_Name { get; set; }
        public string Remitter_Address { get; set; }
        public string Remitter_IDType { get; set; }
        public string Remitter_IDDtl { get; set; }
        public string Originating_Country { get; set; }
        public string Delivery_Mode { get; set; }
        public string Paying_Amount { get; set; }
        public string PayingAgent_CommShare { get; set; }
        public string Paying_Currency { get; set; }
        public string Paying_Agent { get; set; }
        public string Paying_AgentName { get; set; }
        public string Beneficiary_Name { get; set; }
        public string Beneficiary_Address { get; set; }
        public string Beneficiary_City { get; set; }
        public string Destination_Country { get; set; }
        public string Beneficiary_TelNo { get; set; }
        public string Beneficiary_MobileNo { get; set; }
        public string Expected_BenefID { get; set; }
        public string Bank_Address { get; set; }
        public string Bank_Account_Number { get; set; }
        public string Bank_Name { get; set; }
        public string Purpose_Remit { get; set; }
        public string Message_PayeeBranch { get; set; }
        public string Bank_BranchCode { get; set; }
        public string Settlement_Rate { get; set; }
        public string PrinSettlement_Amount { get; set; }
        public string Transaction_SentDate { get; set; }
        public string Remitter_Nationality { get; set; }
        public string Remitter_DOB { get; set; }
        public string Remitter_City { get; set; }
    }
}
