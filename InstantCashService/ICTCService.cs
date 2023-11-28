using InstantCashService.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InstantCashService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICTCService
    {

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        

        [OperationContract]
        OutstandingRemittanceResponse DownloadAcTxnResult(string securityCode);
                
        [OperationContract]
        ConfirmTranResponse ConfirmTransaction(string securityCode, string IctcNumber, string statusFlag, string remarks);

        [OperationContract]
        IcPayStatusResponse GetIctcStatus(string securityCode, string IctcNumber);

        [OperationContract]
        CashTxnDetails ReceivePayment(string securityCode, string IctcNumber);
                
        [OperationContract]
        UnlockTranResponse UnlockTransaction(string securityCode, string IctcNumber);
                
        [OperationContract]
        ReconcileResponses ReconcileTxn(string securityCode, string dateFrom, string dateTo, string reportLevel);
        
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
