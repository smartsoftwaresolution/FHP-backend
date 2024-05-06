using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.Contract
{
    public class AddContractModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string Description { get; set; }
        public string EmployeeSignature { get; set; }
        public string EmployerSignature { get; set; }
        public int? RequestToChangeContract { get; set; }
        public bool IsRequestToChangeAccepted { get; set; }
        public bool IsSignedByEmployee { get; set; }
        public bool IsSignedByEmployer { get; set; }
        public string Title { get; set; }

    }
}
