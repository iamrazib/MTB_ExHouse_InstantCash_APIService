using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantCashService.ResponseClasses
{
    public class ConfirmTranResponse
    {
        public string Result_Flag { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string Error_Description { get; set; }

        public string ICTC_Number { get; set; }
        public string Confirmed { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return this.ICTC_Number + ", Confirmed:" + this.Confirmed + ", Desc:" + this.Description;
        }
    }
}
