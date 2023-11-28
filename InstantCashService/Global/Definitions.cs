using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstantCashService.Global.Definitions.webServiceUrl
{
    public sealed class serviceUrlValue
    {
        public const string serviceUrl = "https://icint.wallstreet.ae/IcIntegration/IcWebService.asmx";
        public const string ProxyHost = @"192.168.51.61";
        public const int ProxyPort = 80;
    }
}

namespace InstantCashService.Global.Definitions.securityCode
{
    public sealed class secValue
    {
        public const string AccessCode = "ICTC@@#@#160800";

    }
}

namespace InstantCashService.Global.Definitions.ICTCCredentials
{
    public sealed class credValue
    {        
        public const string Agent_UserID = "BD00011";
        public const string Agent_CorrespondentID = "BD0015";
        public const string User_Password = "ESZ001";
    }
}

namespace InstantCashService.Global.Definitions.ExchangeHouse
{
    public sealed class ExHName
    {
        public const string ictc = "InstantCash";
    }
}

namespace InstantCashService.Global.Definitions.ServiceID
{
    public sealed class ICTCServiceClassValue
    {
        public const int GetOutstandingRemittance = 1;
        public const int ConfirmTran = 2;
        public const int GetIctcPayStatus = 3;
        public const int Reconcile = 4;
        public const int ReceivePayment = 5;
        public const int UnlockIcTran = 6;
    }
}

