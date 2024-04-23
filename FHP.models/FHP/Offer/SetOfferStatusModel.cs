using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.Offer
{
    public class SetOfferStatusModel
    {
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public int EmployerId { get; set; }
        public Constants.OfferStatus IsAvaliable { get; set; }
        public string CancelReason { get; set; }    

    }
}
