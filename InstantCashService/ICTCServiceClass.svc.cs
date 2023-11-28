using InstantCashService.Global.Definitions.securityCode;
using InstantCashService.Global.Definitions.ServiceID;
using InstantCashService.Manager;
using InstantCashService.ModelClasses;
using InstantCashService.ResponseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;

/*
 *  LIVE code
 *  Code: 18-May-2021
 *  Modified:  08-Sep-2021 by Razibul
 */

namespace InstantCashService
{

    public class ICTCServiceClass : ICTCService
    {
        // Initialization for Log purpose
        LogManager LogObj = new LogManager();
        string RequestCode = null;
        int LogID = 0;
        int isAuthenticated = 0;
        // end 

        public OutstandingRemittanceResponse DownloadAcTxnResult(string securityCode)
        {
            string Agent_UserID = Global.Definitions.ICTCCredentials.credValue.Agent_UserID;
            string Agent_CorrespondentID = Global.Definitions.ICTCCredentials.credValue.Agent_CorrespondentID;
            string User_Password = Global.Definitions.ICTCCredentials.credValue.User_Password;
            string requestUri = Global.Definitions.webServiceUrl.serviceUrlValue.serviceUrl;
            OutstandingRemittanceResponse outstndRemit = new OutstandingRemittanceResponse();

            LogID = 0;
            isAuthenticated = 0;
            string exHouseName = Global.Definitions.ExchangeHouse.ExHName.ictc;
            string soapRequest = "";

            try
            {
                RequestCode = "SecurityCode:" + securityCode;
                LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetOutstandingRemittance, isAuthenticated, RequestCode, "", "");
                string responseFromServer = "";
                        
                if (string.Equals(securityCode, secValue.AccessCode))
                {
                    isAuthenticated = 1;
                    RequestCode = "SecurityCode:Passed";

                    LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetOutstandingRemittance, isAuthenticated, RequestCode, "", "");

                    try
                    {
                        string RQsoap = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:icws=""ICWS"">
                               <soapenv:Header>
                                  <icws:AuthHeader>
                                     <icws:Agent_UserID>$$AgentUserID$$</icws:Agent_UserID>
                                     <icws:User_Password>$$UserPassword$$</icws:User_Password>
                                     <icws:Agent_CorrespondentID>$$AgentCorrespondentID$$</icws:Agent_CorrespondentID>
                                  </icws:AuthHeader>
                               </soapenv:Header>
                               <soapenv:Body>
                                  <icws:GetOutstandingRemittance/>
                               </soapenv:Body>
                            </soapenv:Envelope>";

                        soapRequest = RQsoap.Replace("$$AgentUserID$$", Agent_UserID);
                        soapRequest = soapRequest.Replace("$$UserPassword$$", User_Password).Replace("$$AgentCorrespondentID$$", Agent_CorrespondentID);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12
                               | SecurityProtocolType.Ssl3;

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
                        //req.Proxy = new WebProxy(Global.Definitions.webServiceUrl.serviceUrlValue.ProxyHost, Global.Definitions.webServiceUrl.serviceUrlValue.ProxyPort);

                        req.ContentType = "text/xml";
                        req.Method = "POST";

                        using (Stream stm = req.GetRequestStream())
                        {
                            using (StreamWriter stmw = new StreamWriter(stm))
                            {
                                stmw.Write(soapRequest);
                            }
                        }

                        WebResponse response = req.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        using (StreamReader streader = new StreamReader(responseStream))
                        {
                            responseFromServer = streader.ReadToEnd();
                            streader.Close();
                        }

                        responseFromServer = responseFromServer.Replace("#", "").Replace("&", "").Replace("Å", "");

                        //--------------- deserialize
                        var serializer = new XmlSerializer(typeof(OutstandingRemit.Envelope));
                        OutstandingRemit.Envelope res;

                        using (TextReader reader = new StringReader(responseFromServer))
                        {
                            res = (OutstandingRemit.Envelope)serializer.Deserialize(reader);

                            if (res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Result_Flag.Equals("1"))
                            {
                                int recCount = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Records.OutstandingRemittance.Count;

                                if (recCount > 0)
                                {
                                    outstndRemit.Result_Flag = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Result_Flag;
                                    outstndRemit.Error_Code = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Error_Code;
                                    outstndRemit.Error_Message = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Error_Message;
                                    outstndRemit.Error_Description = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Error_Description;

                                    List<ModelClasses.OutstandingRemit.OutstandingRemittance> OutstandingRemittance = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Records.OutstandingRemittance;
                                    List<OutstandingRemits> outRemitObjList = new List<OutstandingRemits>();

                                    for (int count = 0; count < OutstandingRemittance.Count; count++)
                                    {
                                        OutstandingRemits outRemitObj = new OutstandingRemits();

                                        outRemitObj.ICTC_Number = OutstandingRemittance[count].ICTC_Number;
                                        outRemitObj.Agent_OrderNumber = OutstandingRemittance[count].Agent_OrderNumber;
                                        outRemitObj.Remitter_Name = OutstandingRemittance[count].Remitter_Name;
                                        outRemitObj.Remitter_Address = OutstandingRemittance[count].Remitter_Address;
                                        outRemitObj.Remitter_IDType = OutstandingRemittance[count].Remitter_IDType;
                                        outRemitObj.Remitter_IDDtl = OutstandingRemittance[count].Remitter_IDDtl;
                                        outRemitObj.Originating_Country = OutstandingRemittance[count].Originating_Country;
                                        outRemitObj.Delivery_Mode = OutstandingRemittance[count].Delivery_Mode;
                                        outRemitObj.Paying_Amount = OutstandingRemittance[count].Paying_Amount;
                                        outRemitObj.PayingAgent_CommShare = OutstandingRemittance[count].PayingAgent_CommShare;
                                        outRemitObj.Paying_Currency = OutstandingRemittance[count].Paying_Currency;
                                        outRemitObj.Paying_Agent = OutstandingRemittance[count].Paying_Agent;
                                        outRemitObj.Paying_AgentName = OutstandingRemittance[count].Paying_AgentName;
                                        outRemitObj.Beneficiary_Name = OutstandingRemittance[count].Beneficiary_Name;
                                        outRemitObj.Beneficiary_Address = OutstandingRemittance[count].Beneficiary_Address;
                                        outRemitObj.Beneficiary_City = OutstandingRemittance[count].Beneficiary_City;
                                        outRemitObj.Destination_Country = OutstandingRemittance[count].Destination_Country;
                                        outRemitObj.Beneficiary_TelNo = OutstandingRemittance[count].Beneficiary_TelNo;
                                        outRemitObj.Beneficiary_MobileNo = OutstandingRemittance[count].Beneficiary_MobileNo;
                                        outRemitObj.Expected_BenefID = OutstandingRemittance[count].Expected_BenefID;
                                        outRemitObj.Bank_Address = OutstandingRemittance[count].Bank_Address;
                                        outRemitObj.Bank_Account_Number = OutstandingRemittance[count].Bank_Account_Number;
                                        outRemitObj.Bank_Name = OutstandingRemittance[count].Bank_Name;
                                        outRemitObj.Purpose_Remit = OutstandingRemittance[count].Purpose_Remit;
                                        outRemitObj.Message_PayeeBranch = OutstandingRemittance[count].Message_PayeeBranch;
                                        outRemitObj.Bank_BranchCode = OutstandingRemittance[count].Bank_BranchCode; // for other bank, this field will contain routing number
                                        outRemitObj.Settlement_Rate = OutstandingRemittance[count].Settlement_Rate;
                                        outRemitObj.PrinSettlement_Amount = OutstandingRemittance[count].PrinSettlement_Amount;
                                        outRemitObj.Transaction_SentDate = OutstandingRemittance[count].Transaction_SentDate;
                                        outRemitObj.Remitter_Nationality = OutstandingRemittance[count].Remitter_Nationality;
                                        outRemitObj.Remitter_DOB = OutstandingRemittance[count].Remitter_DOB;
                                        outRemitObj.Remitter_City = OutstandingRemittance[count].Remitter_City;

                                        outRemitObjList.Add(outRemitObj);
                                    } //for end

                                    outstndRemit.outstandingRemittanceList = outRemitObjList;

                                } //if (recCount > 0) END

                            } // END Result_Flag.Equals("1")
                            else
                            {
                                outstndRemit.Result_Flag = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Result_Flag;
                                outstndRemit.Error_Code = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Error_Code;
                                outstndRemit.Error_Message = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Error_Message;
                                outstndRemit.Error_Description = res.Body.GetOutstandingRemittanceResponse.GetOutstandingRemittanceResult.Result.Error_Description;
                            }

                        } // END of using 
                    }
                    catch (Exception Ex)
                    {
                        outstndRemit.Result_Flag = "0";
                        outstndRemit.Error_Code = Ex.Message;
                        outstndRemit.Error_Message = Ex.StackTrace;
                        LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetOutstandingRemittance, isAuthenticated, RequestCode, outstndRemit.Error_Code, outstndRemit.Error_Message);
                    }

                    string downloadedRec = "";
                    if (outstndRemit.Result_Flag.Equals("1"))
                    {
                        downloadedRec = "Downloaded Record=" + outstndRemit.outstandingRemittanceList.Count;
                    }
                    else
                    {
                        downloadedRec = "Downloaded Record=0";
                    }
                    LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetOutstandingRemittance, isAuthenticated, RequestCode, soapRequest, responseFromServer);

                } // END (string.Equals(securityCode, secValue.AccessCode))

            }
            catch (Exception Ex)
            {
                string errorCode = "E003 : Process Error";
                string errorMsg = Ex.Message;
                string errorStackTrace = Ex.StackTrace;
                LogObj.InsertIntoExHouseAPIErrorLog("DownloadAcTxnResult", errorCode, errorMsg, errorStackTrace);

                outstndRemit.Result_Flag = "0";
                outstndRemit.Error_Code = "E005";
                outstndRemit.Error_Message = "No Records";
            }

            return outstndRemit;
        }

        public ConfirmTranResponse ConfirmTransaction(string securityCode, string IctcNumber, string statusFlag, string remarks)
        {
            string Agent_UserID = Global.Definitions.ICTCCredentials.credValue.Agent_UserID;
            string Agent_CorrespondentID = Global.Definitions.ICTCCredentials.credValue.Agent_CorrespondentID;
            string User_Password = Global.Definitions.ICTCCredentials.credValue.User_Password;
            string requestUri = Global.Definitions.webServiceUrl.serviceUrlValue.serviceUrl;
            ConfirmTranResponse confirmTranResp = new ConfirmTranResponse();

            LogID = 0;
            isAuthenticated = 0;
            string exHouseName = Global.Definitions.ExchangeHouse.ExHName.ictc;
            string confirmTranSoap = "";


            try
            {
                RequestCode = "SecurityCode:" + securityCode;
                LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ConfirmTran, isAuthenticated, RequestCode, "", "");

                if (string.Equals(securityCode, secValue.AccessCode))
                {
                    isAuthenticated = 1;
                    RequestCode = "SecurityCode:Passed";
                    LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ConfirmTran, isAuthenticated, RequestCode, "", "");

                    try
                    {
                        string RQsoap = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:icws=""ICWS"">
                               <soapenv:Header>
                                  <icws:AuthHeader>
                                     <icws:Agent_UserID>$$AgentUserID$$</icws:Agent_UserID>
                                     <icws:User_Password>$$UserPassword$$</icws:User_Password>
                                     <icws:Agent_CorrespondentID>$$AgentCorrespondentID$$</icws:Agent_CorrespondentID>
                                  </icws:AuthHeader>
                               </soapenv:Header>
                               <soapenv:Body>
                                  <icws:ConfirmTran>
                                     <icws:Input>
                                        <icws:Records>
                                           <icws:Confirm>
                                              <icws:ICTC_Number>$$ICTC_Number$$</icws:ICTC_Number>
                                              <icws:Status_Flag>$$Status_Flag$$</icws:Status_Flag>
                                              <icws:Remarks>$$Remarks$$</icws:Remarks>
                                           </icws:Confirm>
                                        </icws:Records>
                                     </icws:Input>
                                  </icws:ConfirmTran>
                               </soapenv:Body>
                            </soapenv:Envelope>";

                        confirmTranSoap = RQsoap.Replace("$$AgentUserID$$", Agent_UserID).Replace("$$UserPassword$$", User_Password).Replace("$$AgentCorrespondentID$$", Agent_CorrespondentID);
                        confirmTranSoap = confirmTranSoap.Replace("$$ICTC_Number$$", IctcNumber).Replace("$$Status_Flag$$", statusFlag).Replace("$$Remarks$$", remarks);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12
                               | SecurityProtocolType.Ssl3;

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
                        //req.Proxy = new WebProxy(Global.Definitions.webServiceUrl.serviceUrlValue.ProxyHost, Global.Definitions.webServiceUrl.serviceUrlValue.ProxyPort);

                        req.ContentType = "text/xml";
                        req.Method = "POST";

                        using (Stream stm = req.GetRequestStream())
                        {
                            using (StreamWriter stmw = new StreamWriter(stm))
                            {
                                stmw.Write(confirmTranSoap);
                            }
                        }

                        string responseFromServer = "";
                        WebResponse response = req.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        using (StreamReader streader = new StreamReader(responseStream))
                        {
                            responseFromServer = streader.ReadToEnd();
                            streader.Close();
                        }

                        //--------------- deserialize
                        var serializer = new XmlSerializer(typeof(ConfirmTransaction.Envelope));
                        ConfirmTransaction.Envelope res;

                        using (TextReader reader = new StringReader(responseFromServer))
                        {
                            res = (ConfirmTransaction.Envelope)serializer.Deserialize(reader);

                            try
                            {
                                confirmTranResp.Result_Flag = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Result_Flag;
                                confirmTranResp.Error_Code = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Error_Code;
                                confirmTranResp.Error_Message = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Error_Message;
                                confirmTranResp.Error_Description = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Error_Description;

                                confirmTranResp.ICTC_Number = res.Body.ConfirmTranResponse.ConfirmTranResult.Records.Confirmation.ICTC_Number;
                                confirmTranResp.Confirmed = res.Body.ConfirmTranResponse.ConfirmTranResult.Records.Confirmation.Confirmed.ToLower();
                                confirmTranResp.Description = res.Body.ConfirmTranResponse.ConfirmTranResult.Records.Confirmation.Description;
                            }
                            catch (Exception ec)
                            {
                                confirmTranResp.Result_Flag = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Result_Flag;
                                confirmTranResp.Error_Code = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Error_Code;
                                confirmTranResp.Error_Message = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Error_Message;
                                confirmTranResp.Error_Description = res.Body.ConfirmTranResponse.ConfirmTranResult.Result.Error_Description;
                            }

                            LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ConfirmTran, isAuthenticated, RequestCode, confirmTranSoap, responseFromServer);

                        } // END of using
                    }
                    catch (Exception Ex)
                    {
                        confirmTranResp.Result_Flag = "0";
                        confirmTranResp.Error_Code = Ex.Message;
                        confirmTranResp.Error_Message = Ex.StackTrace;

                        LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ConfirmTran, isAuthenticated, RequestCode, confirmTranResp.Error_Code, confirmTranResp.Error_Message);
                    }

                } // if end [string.Equals(securityCode, secValue.AccessCode)]

            } //try End
            catch (Exception exc)
            {
                confirmTranResp.Result_Flag = "0";
                confirmTranResp.Error_Code = "E003";
                confirmTranResp.Error_Message = "Process Error";

                string errorCode = "E003 : Process Error";
                string errorMsg = exc.Message;
                string errorStackTrace = exc.StackTrace;
                LogObj.InsertIntoExHouseAPIErrorLog("ConfirmTransaction", errorCode, errorMsg, errorStackTrace);
            }

            return confirmTranResp;
        }

        public IcPayStatusResponse GetIctcStatus(string securityCode, string IctcNumber)
        {
            string Agent_UserID = Global.Definitions.ICTCCredentials.credValue.Agent_UserID;
            string Agent_CorrespondentID = Global.Definitions.ICTCCredentials.credValue.Agent_CorrespondentID;
            string User_Password = Global.Definitions.ICTCCredentials.credValue.User_Password;
            string requestUri = Global.Definitions.webServiceUrl.serviceUrlValue.serviceUrl;
            IcPayStatusResponse icPayStatResp = new IcPayStatusResponse();

            LogID = 0;
            isAuthenticated = 0;
            string exHouseName = Global.Definitions.ExchangeHouse.ExHName.ictc;

            try
            {
                RequestCode = "SecurityCode:" + securityCode;
                LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetIctcPayStatus, isAuthenticated, RequestCode, "", "");

                if (string.Equals(securityCode, secValue.AccessCode))
                {
                    isAuthenticated = 1;
                    RequestCode = "SecurityCode:Passed";
                    LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetIctcPayStatus, isAuthenticated, RequestCode, "", "");

                    try
                    {
                        string RQsoap = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:icws=""ICWS"">
                                   <soapenv:Header>
                                      <icws:AuthHeader>
                                         <icws:Agent_UserID>$$AgentUserID$$</icws:Agent_UserID>
                                         <icws:User_Password>$$UserPassword$$</icws:User_Password>
                                         <icws:Agent_CorrespondentID>$$AgentCorrespondentID$$</icws:Agent_CorrespondentID>
                                      </icws:AuthHeader>
                                   </soapenv:Header>
                                   <soapenv:Body>
                                      <icws:GetIctcPayStatus>
                                         <icws:Input>
                                            <icws:ICTC_Number>$$ICTC_Number$$</icws:ICTC_Number>
                                            <icws:Agent_OrderNumber></icws:Agent_OrderNumber>
                                         </icws:Input>
                                      </icws:GetIctcPayStatus>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

                        string payStatusSoap = RQsoap.Replace("$$ICTC_Number$$", IctcNumber);
                        payStatusSoap = payStatusSoap.Replace("$$AgentUserID$$", Agent_UserID).Replace("$$UserPassword$$", User_Password).Replace("$$AgentCorrespondentID$$", Agent_CorrespondentID);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12
                               | SecurityProtocolType.Ssl3;

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
                        //req.Proxy = new WebProxy(Global.Definitions.webServiceUrl.serviceUrlValue.ProxyHost, Global.Definitions.webServiceUrl.serviceUrlValue.ProxyPort);
                        req.ContentType = "text/xml";
                        req.Method = "POST";

                        using (Stream stm = req.GetRequestStream())
                        {
                            using (StreamWriter stmw = new StreamWriter(stm))
                            {
                                stmw.Write(payStatusSoap);
                            }
                        }

                        string responseFromServer = "";
                        WebResponse response = req.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        using (StreamReader streader = new StreamReader(responseStream))
                        {
                            responseFromServer = streader.ReadToEnd();
                            streader.Close();
                        }

                        //------- deserialize --------
                        var serializer = new XmlSerializer(typeof(IcPayStatus.Envelope));
                        IcPayStatus.Envelope res;

                        using (TextReader reader = new StringReader(responseFromServer))
                        {
                            res = (IcPayStatus.Envelope)serializer.Deserialize(reader);

                            if (res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Result_Flag.Equals("1"))
                            {
                                icPayStatResp.Result_Flag = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Result_Flag;
                                icPayStatResp.Error_Code = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Error_Code;
                                icPayStatResp.Error_Message = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Error_Message;
                                icPayStatResp.Error_Description = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Error_Description;

                                icPayStatResp.ICTC_Number = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Details.ICTC_Number;
                                icPayStatResp.Agent_OrderNumber = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Details.Agent_OrderNumber;
                                icPayStatResp.Transaction_Status = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Details.Transaction_Status;
                                icPayStatResp.Status_Description = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Details.Status_Description;
                                icPayStatResp.Extra_Flag1 = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Details.Extra_Flag1;
                            }
                            else
                            {
                                icPayStatResp.Result_Flag = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Result_Flag;
                                icPayStatResp.Error_Code = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Error_Code;
                                icPayStatResp.Error_Message = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Error_Message;
                                icPayStatResp.Error_Description = res.Body.GetIctcPayStatusResponse.GetIctcPayStatusResult.Result.Error_Description;
                            }

                            LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetIctcPayStatus, isAuthenticated, RequestCode, payStatusSoap, responseFromServer);

                        } // END using

                    } // END try
                    catch (Exception Ex)
                    {
                        icPayStatResp.Result_Flag = "0";
                        icPayStatResp.Error_Code = Ex.Message;
                        icPayStatResp.Error_Message = Ex.StackTrace;

                        LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.GetIctcPayStatus, isAuthenticated, RequestCode, icPayStatResp.Error_Code, icPayStatResp.Error_Message);
                    }

                } // if end [string.Equals(securityCode, secValue.AccessCode)]

            } //try End
            catch (Exception exc)
            {
                icPayStatResp.Result_Flag = "0";

                string errorCode = "E003 : Process Error";
                string errorMsg = exc.Message;
                string errorStackTrace = exc.StackTrace;
                LogObj.InsertIntoExHouseAPIErrorLog("IctcPayStatus", errorCode, errorMsg, errorStackTrace);
            }

            return icPayStatResp;
        }


        public CashTxnDetails ReceivePayment(string securityCode, string IctcNumber)
        {
            string Agent_UserID = Global.Definitions.ICTCCredentials.credValue.Agent_UserID;
            string Agent_CorrespondentID = Global.Definitions.ICTCCredentials.credValue.Agent_CorrespondentID;
            string User_Password = Global.Definitions.ICTCCredentials.credValue.User_Password;
            string requestUri = Global.Definitions.webServiceUrl.serviceUrlValue.serviceUrl;
            CashTxnDetails cashTxn = new CashTxnDetails();

            LogID = 0;
            isAuthenticated = 0;
            string exHouseName = Global.Definitions.ExchangeHouse.ExHName.ictc;

            try
            {
                RequestCode = "SecurityCode:" + securityCode;
                LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ReceivePayment, isAuthenticated, RequestCode, "", "");

                if (string.Equals(securityCode, secValue.AccessCode))
                {
                    isAuthenticated = 1;
                    RequestCode = "SecurityCode:Passed";
                    LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ReceivePayment, isAuthenticated, RequestCode, "", "");

                    try
                    {
                        string RQsoap = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:icws=""ICWS"">
                                   <soapenv:Header>
                                      <icws:AuthHeader>
                                         <icws:Agent_UserID>$$AgentUserID$$</icws:Agent_UserID>
                                         <icws:User_Password>$$UserPassword$$</icws:User_Password>
                                         <icws:Agent_CorrespondentID>$$AgentCorrespondentID$$</icws:Agent_CorrespondentID>
                                      </icws:AuthHeader>
                                   </soapenv:Header>
                                   <soapenv:Body>
                                      <icws:ReceivePayment>
                                         <icws:Input>
                                            <icws:ICTC_Number>$$ICTC_Number$$</icws:ICTC_Number>
                                            <icws:Agent_OrderNumber></icws:Agent_OrderNumber>
                                         </icws:Input>
                                      </icws:ReceivePayment>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

                        string rcvPmntSoap = RQsoap.Replace("$$ICTC_Number$$", IctcNumber);
                        rcvPmntSoap = rcvPmntSoap.Replace("$$AgentUserID$$", Agent_UserID).Replace("$$UserPassword$$", User_Password).Replace("$$AgentCorrespondentID$$", Agent_CorrespondentID);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12
                               | SecurityProtocolType.Ssl3;

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
                        //req.Proxy = new WebProxy(Global.Definitions.webServiceUrl.serviceUrlValue.ProxyHost, Global.Definitions.webServiceUrl.serviceUrlValue.ProxyPort);
                        req.ContentType = "text/xml";
                        req.Method = "POST";

                        using (Stream stm = req.GetRequestStream())
                        {
                            using (StreamWriter stmw = new StreamWriter(stm))
                            {
                                stmw.Write(rcvPmntSoap);
                            }
                        }

                        string responseFromServer = "";
                        WebResponse response = req.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        using (StreamReader streader = new StreamReader(responseStream))
                        {
                            responseFromServer = streader.ReadToEnd();
                            streader.Close();
                        }

                        //------- deserialize --------
                        var serializer = new XmlSerializer(typeof(ReceivePayments.Envelope));
                        ReceivePayments.Envelope res;

                        using (TextReader reader = new StringReader(responseFromServer))
                        {
                            res = (ReceivePayments.Envelope)serializer.Deserialize(reader);

                            if (res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Result_Flag.Equals("1"))
                            {
                                ReceivePayments.Details cashDetails = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Details;

                                cashTxn.Result_Flag = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Result_Flag;
                                cashTxn.Error_Code = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Error_Code;
                                cashTxn.Error_Message = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Error_Message;
                                cashTxn.Error_Description = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Error_Description;

                                cashTxn.ICTC_Number = cashDetails.ICTC_Number;
                                cashTxn.Agent_OrderNumber = cashDetails.Agent_OrderNumber;
                                cashTxn.Remitter_Name = cashDetails.Remitter_Name;
                                cashTxn.Remitter_Address = cashDetails.Remitter_Address;
                                cashTxn.Remitter_IDType = cashDetails.Remitter_IDType;
                                cashTxn.Remitter_IDDtl = cashDetails.Remitter_IDDtl;
                                cashTxn.Originating_Country = cashDetails.Originating_Country;
                                cashTxn.Delivery_Mode = cashDetails.Delivery_Mode;
                                cashTxn.Paying_Amount = cashDetails.Paying_Amount;
                                cashTxn.PayingAgent_CommShare = cashDetails.PayingAgent_CommShare;
                                cashTxn.Paying_Currency = cashDetails.Paying_Currency;
                                cashTxn.Paying_Agent = cashDetails.Paying_Agent;
                                cashTxn.Paying_AgentName = cashDetails.Paying_AgentName;
                                cashTxn.Beneficiary_Name = cashDetails.Beneficiary_Name;
                                cashTxn.Beneficiary_Address = cashDetails.Beneficiary_Address;
                                cashTxn.Beneficiary_City = cashDetails.Beneficiary_City;
                                cashTxn.Destination_Country = cashDetails.Destination_Country;
                                cashTxn.Beneficiary_TelNo = cashDetails.Beneficiary_TelNo;
                                cashTxn.Beneficiary_MobileNo = cashDetails.Beneficiary_MobileNo;
                                cashTxn.Expected_BenefID = cashDetails.Expected_BenefID;
                                cashTxn.Bank_Address = cashDetails.Bank_Address;
                                cashTxn.Bank_Account_Number = cashDetails.Bank_Account_Number;
                                cashTxn.Bank_Name = cashDetails.Bank_Name;
                                cashTxn.Purpose_Remit = cashDetails.Purpose_Remit;
                                cashTxn.Message_PayeeBranch = cashDetails.Message_PayeeBranch;
                                cashTxn.Bank_BranchCode = cashDetails.Bank_BranchCode;
                                cashTxn.Settlement_Rate = cashDetails.Settlement_Rate;
                                cashTxn.PrinSettlement_Amount = cashDetails.PrinSettlement_Amount;
                                cashTxn.Transaction_SentDate = cashDetails.Transaction_SentDate;
                            }
                            else
                            {
                                cashTxn.Result_Flag = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Result_Flag;
                                cashTxn.Error_Code = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Error_Code;
                                cashTxn.Error_Message = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Error_Message;
                                cashTxn.Error_Description = res.Body.ReceivePaymentResponse.ReceivePaymentResult.Result.Error_Description;
                            }

                            LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ReceivePayment, isAuthenticated, RequestCode, rcvPmntSoap, responseFromServer);

                        } //using end

                    }// END try
                    catch (Exception Ex)
                    {
                        cashTxn.Result_Flag = "0";
                        cashTxn.Error_Code = Ex.Message;
                        cashTxn.Error_Message = Ex.StackTrace;

                        LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.ReceivePayment, isAuthenticated, RequestCode, cashTxn.Error_Code, cashTxn.Error_Message);
                    }

                } // END (string.Equals(securityCode, secValue.AccessCode))

            } //try End
            catch (Exception exc)
            {
                cashTxn.Result_Flag = "0";

                string errorCode = "E003 : Process Error";
                string errorMsg = exc.Message;
                string errorStackTrace = exc.StackTrace;
                LogObj.InsertIntoExHouseAPIErrorLog("ReceivePayment", errorCode, errorMsg, errorStackTrace);
            }

            return cashTxn;
        }

        public UnlockTranResponse UnlockTransaction(string securityCode, string IctcNumber)
        {
            string Agent_UserID = Global.Definitions.ICTCCredentials.credValue.Agent_UserID;
            string Agent_CorrespondentID = Global.Definitions.ICTCCredentials.credValue.Agent_CorrespondentID;
            string User_Password = Global.Definitions.ICTCCredentials.credValue.User_Password;
            string requestUri = Global.Definitions.webServiceUrl.serviceUrlValue.serviceUrl;
            UnlockTranResponse unlockTxnResp = new UnlockTranResponse();

            LogID = 0;
            isAuthenticated = 0;
            string exHouseName = Global.Definitions.ExchangeHouse.ExHName.ictc;

            try
            {
                RequestCode = "SecurityCode:" + securityCode;
                LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.UnlockIcTran, isAuthenticated, RequestCode, "", "");

                if (string.Equals(securityCode, secValue.AccessCode))
                {
                    isAuthenticated = 1;
                    RequestCode = "SecurityCode:Passed";
                    LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.UnlockIcTran, isAuthenticated, RequestCode, "", "");

                    try
                    {
                        string RQsoap = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:icws=""ICWS"">
                                   <soapenv:Header>
                                      <icws:AuthHeader>
                                         <icws:Agent_UserID>$$AgentUserID$$</icws:Agent_UserID>
                                         <icws:User_Password>$$UserPassword$$</icws:User_Password>
                                         <icws:Agent_CorrespondentID>$$AgentCorrespondentID$$</icws:Agent_CorrespondentID>
                                      </icws:AuthHeader>
                                   </soapenv:Header>
                                   <soapenv:Body>
                                      <icws:UnlockIcTran>
                                         <icws:Input>
                                            <icws:ICTC_Number>$$ICTC_Number$$</icws:ICTC_Number>
                                         </icws:Input>
                                      </icws:UnlockIcTran>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

                        string unlockSoap = RQsoap.Replace("$$ICTC_Number$$", IctcNumber);
                        unlockSoap = unlockSoap.Replace("$$AgentUserID$$", Agent_UserID).Replace("$$UserPassword$$", User_Password).Replace("$$AgentCorrespondentID$$", Agent_CorrespondentID);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12
                               | SecurityProtocolType.Ssl3;

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
                        //req.Proxy = new WebProxy(Global.Definitions.webServiceUrl.serviceUrlValue.ProxyHost, Global.Definitions.webServiceUrl.serviceUrlValue.ProxyPort);
                        req.ContentType = "text/xml";
                        req.Method = "POST";

                        using (Stream stm = req.GetRequestStream())
                        {
                            using (StreamWriter stmw = new StreamWriter(stm))
                            {
                                stmw.Write(unlockSoap);
                            }
                        }

                        string responseFromServer = "";
                        WebResponse response = req.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        using (StreamReader streader = new StreamReader(responseStream))
                        {
                            responseFromServer = streader.ReadToEnd();
                            streader.Close();
                        }

                        //------- deserialize --------
                        var serializer = new XmlSerializer(typeof(UnlockICTransaction.Envelope));
                        UnlockICTransaction.Envelope res;

                        using (TextReader reader = new StringReader(responseFromServer))
                        {
                            res = (UnlockICTransaction.Envelope)serializer.Deserialize(reader);

                            unlockTxnResp.Result_Flag = res.Body.UnlockIcTranResponse.UnlockIcTranResult.Result.Result_Flag;
                            unlockTxnResp.Error_Code = res.Body.UnlockIcTranResponse.UnlockIcTranResult.Result.Error_Code;
                            unlockTxnResp.Error_Message = res.Body.UnlockIcTranResponse.UnlockIcTranResult.Result.Error_Message;
                            unlockTxnResp.Error_Description = res.Body.UnlockIcTranResponse.UnlockIcTranResult.Result.Error_Description;

                            LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.UnlockIcTran, isAuthenticated, RequestCode, unlockSoap, responseFromServer);

                        } // END using

                    } // END try
                    catch (Exception Ex)
                    {
                        unlockTxnResp.Result_Flag = "0";
                        unlockTxnResp.Error_Code = Ex.Message;
                        unlockTxnResp.Error_Message = Ex.StackTrace;

                        LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.UnlockIcTran, isAuthenticated, RequestCode, unlockTxnResp.Error_Code, unlockTxnResp.Error_Message);
                    }

                } // if end [string.Equals(securityCode, secValue.AccessCode)]
            } //try End
            catch (Exception exc)
            {
                unlockTxnResp.Result_Flag = "0";

                string errorCode = "E003 : Process Error";
                string errorMsg = exc.Message;
                string errorStackTrace = exc.StackTrace;
                LogObj.InsertIntoExHouseAPIErrorLog("UnlockIcTran", errorCode, errorMsg, errorStackTrace);
            }

            return unlockTxnResp;
        }


        public ReconcileResponses ReconcileTxn(string securityCode, string dateFrom, string dateTo, string reportLevel)
        {
            string Agent_UserID = Global.Definitions.ICTCCredentials.credValue.Agent_UserID;
            string Agent_CorrespondentID = Global.Definitions.ICTCCredentials.credValue.Agent_CorrespondentID;
            string User_Password = Global.Definitions.ICTCCredentials.credValue.User_Password;
            string requestUri = Global.Definitions.webServiceUrl.serviceUrlValue.serviceUrl;
            ReconcileResponses reconcileResponses = new ReconcileResponses();

            LogID = 0;
            isAuthenticated = 0;
            string exHouseName = Global.Definitions.ExchangeHouse.ExHName.ictc;

            try
            {
                RequestCode = "SecurityCode:" + securityCode;
                LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.Reconcile, isAuthenticated, RequestCode, "", "");

                if (string.Equals(securityCode, secValue.AccessCode))
                {
                    isAuthenticated = 1;
                    RequestCode = "SecurityCode:Passed";
                    LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.Reconcile, isAuthenticated, RequestCode, "", "");

                    try
                    {
                        string RQsoap = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:icws=""ICWS"">
                                   <soapenv:Header>
                                      <icws:AuthHeader>
                                         <icws:Agent_UserID>$$AgentUserID$$</icws:Agent_UserID>
                                         <icws:User_Password>$$UserPassword$$</icws:User_Password>
                                         <icws:Agent_CorrespondentID>$$AgentCorrespondentID$$</icws:Agent_CorrespondentID>
                                      </icws:AuthHeader>
                                   </soapenv:Header>
                                   <soapenv:Body>
                                      <icws:Reconcile>
                                         <icws:Input>
                                            <icws:Date_From>$$FROM_DATE$$</icws:Date_From>
                                            <icws:Date_To>$$TO_DATE$$</icws:Date_To>
                                            <icws:Report_Level>$$REPORT_LEVEL$$</icws:Report_Level>
                                         </icws:Input>
                                      </icws:Reconcile>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

                        string reconcileSoap = RQsoap.Replace("$$FROM_DATE$$", dateFrom);
                        reconcileSoap = reconcileSoap.Replace("$$TO_DATE$$", dateTo).Replace("$$REPORT_LEVEL$$", reportLevel);
                        reconcileSoap = reconcileSoap.Replace("$$AgentUserID$$", Agent_UserID).Replace("$$UserPassword$$", User_Password).Replace("$$AgentCorrespondentID$$", Agent_CorrespondentID);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12
                               | SecurityProtocolType.Ssl3;

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
                        //req.Proxy = new WebProxy(Global.Definitions.webServiceUrl.serviceUrlValue.ProxyHost, Global.Definitions.webServiceUrl.serviceUrlValue.ProxyPort);
                        req.ContentType = "text/xml";
                        req.Method = "POST";

                        using (Stream stm = req.GetRequestStream())
                        {
                            using (StreamWriter stmw = new StreamWriter(stm))
                            {
                                stmw.Write(reconcileSoap);
                            }
                        }

                        string responseFromServer = "";
                        WebResponse response = req.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        using (StreamReader streader = new StreamReader(responseStream))
                        {
                            responseFromServer = streader.ReadToEnd();
                            streader.Close();
                        }

                        //------- deserialize --------
                        var serializer = new XmlSerializer(typeof(Reconcilliation.Envelope));
                        Reconcilliation.Envelope res;

                        using (TextReader reader = new StringReader(responseFromServer))
                        {
                            res = (Reconcilliation.Envelope)serializer.Deserialize(reader);
                            int recCount = res.Body.ReconcileResponse.ReconcileResult.Records.Transaction.Count;

                            if (recCount > 0)
                            {
                                if (res.Body.ReconcileResponse.ReconcileResult.Result.Result_Flag.Equals("1"))
                                {
                                    reconcileResponses.Result_Flag = res.Body.ReconcileResponse.ReconcileResult.Result.Result_Flag;
                                    reconcileResponses.Error_Code = res.Body.ReconcileResponse.ReconcileResult.Result.Error_Code;
                                    reconcileResponses.Error_Message = res.Body.ReconcileResponse.ReconcileResult.Result.Error_Message;
                                    reconcileResponses.Error_Description = res.Body.ReconcileResponse.ReconcileResult.Result.Error_Description;

                                    List<ModelClasses.Reconcilliation.Transaction> reconTrans = res.Body.ReconcileResponse.ReconcileResult.Records.Transaction;
                                    List<Transaction> reconRemitObjList = new List<Transaction>();

                                    for (int count = 0; count < reconTrans.Count; count++)
                                    {
                                        Transaction reconRemitObj = new Transaction();

                                        reconRemitObj.Transaction_Type = reconTrans[count].Transaction_Type;
                                        reconRemitObj.ICTC_Number = reconTrans[count].ICTC_Number;
                                        reconRemitObj.Agent_OrderNumber = reconTrans[count].Agent_OrderNumber;
                                        reconRemitObj.Agent_UserID = reconTrans[count].Agent_UserID;
                                        reconRemitObj.Agent_Code = reconTrans[count].Agent_Code;
                                        reconRemitObj.Agent_Name = reconTrans[count].Agent_Name;
                                        reconRemitObj.Mode_Description = reconTrans[count].Mode_Description;
                                        reconRemitObj.Remitter_Name = reconTrans[count].Remitter_Name;
                                        reconRemitObj.Remitter_Address = reconTrans[count].Remitter_Address;
                                        reconRemitObj.Originating_Country = reconTrans[count].Originating_Country;
                                        reconRemitObj.Remitter_TelNo = reconTrans[count].Remitter_TelNo;
                                        reconRemitObj.Remitter_MobileNo = reconTrans[count].Remitter_MobileNo;
                                        reconRemitObj.Message_PayeeBranch = reconTrans[count].Message_PayeeBranch;
                                        reconRemitObj.Remitter_ID_Type = reconTrans[count].Remitter_ID_Type;
                                        reconRemitObj.Remitter_IDDtl1 = reconTrans[count].Remitter_IDDtl1;
                                        reconRemitObj.Transaction_Date = reconTrans[count].Transaction_Date;
                                        reconRemitObj.Transaction_Time = reconTrans[count].Transaction_Time;
                                        reconRemitObj.Customer_Princ_Amount = reconTrans[count].Customer_Princ_Amount;
                                        reconRemitObj.FC_Amount = reconTrans[count].FC_Amount;
                                        reconRemitObj.Settlement_Rate = reconTrans[count].Settlement_Rate;
                                        reconRemitObj.Total_Commission = reconTrans[count].Total_Commission;
                                        reconRemitObj.Agent_Share = reconTrans[count].Agent_Share;
                                        reconRemitObj.Settlement_Amount = reconTrans[count].Settlement_Amount;
                                        reconRemitObj.Paying_Currency = reconTrans[count].Paying_Currency;
                                        reconRemitObj.Beneficiary_Name = reconTrans[count].Beneficiary_Name;
                                        reconRemitObj.Expected_BenefID = reconTrans[count].Expected_BenefID;
                                        reconRemitObj.Bank_Account_Number = reconTrans[count].Bank_Account_Number;
                                        reconRemitObj.Bank_Name = reconTrans[count].Bank_Name;
                                        reconRemitObj.Bank_Address = reconTrans[count].Bank_Address;
                                        reconRemitObj.Receiving_Agent_Code = reconTrans[count].Receiving_Agent_Code;
                                        reconRemitObj.Receiving_Agent_Name = reconTrans[count].Receiving_Agent_Name;
                                        reconRemitObj.Beneficiary_Address = reconTrans[count].Beneficiary_Address;
                                        reconRemitObj.Beneficiary_Country = reconTrans[count].Beneficiary_Country;
                                        reconRemitObj.Beneficiary_TelNo = reconTrans[count].Beneficiary_TelNo;
                                        reconRemitObj.Beneficiary_MobileNo = reconTrans[count].Beneficiary_MobileNo;
                                        reconRemitObj.Purpose_Remit = reconTrans[count].Purpose_Remit;
                                        reconRemitObj.Perc_Refund_Comm = reconTrans[count].Perc_Refund_Comm;
                                        reconRemitObj.Branch_Code = reconTrans[count].Branch_Code;
                                        reconRemitObj.Transaction_Status = reconTrans[count].Transaction_Status;
                                        reconRemitObj.Transaction_Status_Description = reconTrans[count].Transaction_Status_Description;
                                        reconRemitObj.Agent_Exchange_Earning = reconTrans[count].Agent_Exchange_Earning;
                                        reconRemitObj.IC_Exchange_Earning = reconTrans[count].IC_Exchange_Earning;
                                        reconRemitObj.Trans_CancelDate = reconTrans[count].Trans_CancelDate;
                                        reconRemitObj.Trans_CancelTime = reconTrans[count].Trans_CancelTime;

                                        reconRemitObjList.Add(reconRemitObj);
                                    } //for end

                                    reconcileResponses.TransactionList = reconRemitObjList;
                                } // if flag=1 END

                            } // if recCount=1 END

                        } // using END

                    } // END try
                    catch (Exception Ex)
                    {
                        reconcileResponses.Result_Flag = "0";
                        reconcileResponses.Error_Code = Ex.Message;
                        reconcileResponses.Error_Message = Ex.StackTrace;

                        LogID = LogObj.RQRSLogger(LogID, exHouseName, ICTCServiceClassValue.Reconcile, isAuthenticated, RequestCode, reconcileResponses.Error_Code, reconcileResponses.Error_Message);
                    }

                } // if end [string.Equals(securityCode, secValue.AccessCode)]

            }
            catch (Exception exc)
            {
                reconcileResponses.Result_Flag = "0";

                string errorCode = "E003 : Process Error";
                string errorMsg = exc.Message;
                string errorStackTrace = exc.StackTrace;
                LogObj.InsertIntoExHouseAPIErrorLog("ReconcileTxn", errorCode, errorMsg, errorStackTrace);
            }

            return reconcileResponses;
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
