using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantCashService.ResponseClasses
{
    public class IcPayStatusResponse
    {
        public string Result_Flag { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string Error_Description { get; set; }

        public string ICTC_Number { get; set; }
        public string Agent_OrderNumber { get; set; }
        public string Transaction_Status { get; set; }
        public string Status_Description { get; set; }
        public string Extra_Flag1 { get; set; }

        public override string ToString()
        {
            if(this.Result_Flag.Equals("0"))
            {
                return (this.Error_Code + " - " + this.Error_Message);
            }
            else
            {
                return (this.ICTC_Number + ", Status=" + this.Transaction_Status + ", Description=" + this.Status_Description);
            }            
        }

    }
}
