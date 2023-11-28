using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantCashService.ResponseClasses
{
    public class UnlockTranResponse
    {
        public string Result_Flag { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string Error_Description { get; set; }

        public override string ToString()
        {
            return ("Result=" + this.Result_Flag + ", Error_Message=" + this.Error_Message + ", Error_Description=" + this.Error_Description);
        }
    }
}
