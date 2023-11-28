using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstantCashService.ResponseClasses
{
    public class ReconcileResponses
    {
        public string Result_Flag { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string Error_Description { get; set; }
        public List<Transaction> TransactionList { get; set; }
    }

    public class Transaction
    {
        public string Transaction_Type { get; set; }
        public string ICTC_Number { get; set; }
        public string Agent_OrderNumber { get; set; }
        public string Agent_UserID { get; set; }
        public string Agent_Code { get; set; }
        public string Agent_Name { get; set; }
        public string Mode_Description { get; set; }
        public string Remitter_Name { get; set; }
        public string Remitter_Address { get; set; }
        public string Originating_Country { get; set; }
        public string Remitter_TelNo { get; set; }
        public string Remitter_MobileNo { get; set; }
        public string Message_PayeeBranch { get; set; }
        public string Remitter_ID_Type { get; set; }
        public string Remitter_IDDtl1 { get; set; }
        public string Transaction_Date { get; set; }
        public string Transaction_Time { get; set; }
        public string Customer_Princ_Amount { get; set; }
        public string FC_Amount { get; set; }
        public string Settlement_Rate { get; set; }
        public string Total_Commission { get; set; }
        public string Agent_Share { get; set; }
        public string Settlement_Amount { get; set; }
        public string Paying_Currency { get; set; }
        public string Beneficiary_Name { get; set; }
        public string Expected_BenefID { get; set; }
        public string Bank_Account_Number { get; set; }
        public string Bank_Name { get; set; }
        public string Bank_Address { get; set; }
        public string Receiving_Agent_Code { get; set; }
        public string Receiving_Agent_Name { get; set; }
        public string Beneficiary_Address { get; set; }
        public string Beneficiary_Country { get; set; }
        public string Beneficiary_TelNo { get; set; }
        public string Beneficiary_MobileNo { get; set; }
        public string Purpose_Remit { get; set; }
        public string Perc_Refund_Comm { get; set; }
        public string Branch_Code { get; set; }
        public string Transaction_Status { get; set; }
        public string Transaction_Status_Description { get; set; }
        public string Agent_Exchange_Earning { get; set; }
        public string IC_Exchange_Earning { get; set; }
        public string Trans_CancelDate { get; set; }
        public string Trans_CancelTime { get; set; }
    }
}