using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.Offer
{
    public class OfferDetailDto
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public int EmployerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Constants.OfferStatus IsAvaliable { get; set; }
        public string CancelReason { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
