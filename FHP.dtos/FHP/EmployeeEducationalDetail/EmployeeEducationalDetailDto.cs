using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.EmployeeEducationalDetail
{
    public class EmployeeEducationalDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Education { get; set; }
        public string NameOfBoardOrUniversity { get; set; }
        public int YearOfCompletion { get; set; }
        public double? MarksObtained { get; set; }
        public double GPA { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
