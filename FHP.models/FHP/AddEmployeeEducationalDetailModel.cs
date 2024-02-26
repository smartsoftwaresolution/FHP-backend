using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP
{
    public class AddEmployeeEducationalDetailModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Education { get; set; }
        public string NameOfBoardOrUniversity { get; set; }
        public int YearOfCompletion { get; set; }
        public double? MarksObtained { get; set; }
        public double GPA { get; set; }
    }
}
