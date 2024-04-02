using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.Contract
{
    public class ContractDetailDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public DateTime Duration { get; set; }
        public string Description { get; set; }
        public string EmployeeSignature { get; set; }
        public string EmployerSignature { get; set; }
        public DateTime? StartContract { get; set; }
        public int? RequestToChangeContract { get; set; }
        public bool IsRequestToChangeAccepted { get; set; }
        public bool IsSignedByEmployee { get; set; }
        public bool IsSignedByEmployer { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string Title { get; set; }

    }
}
