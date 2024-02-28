using FHP.entity.FHP;
using FHP.models.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class EmployeeEducationalDetailFactory
    {
        public static EmployeeEducationalDetail Create(AddEmployeeEducationalDetailModel model)
        {
            var data = new EmployeeEducationalDetail
            {
                UserId = model.UserId,
                Education = model.Education,
                NameOfBoardOrUniversity = model.NameOfBoardOrUniversity,
                YearOfCompletion = model.YearOfCompletion,
                MarksObtained = model.MarksObtained,
                GPA = model.GPA,
                Status = Constants.RecordStatus.Active,
                CreatedOn=Utility.GetDateTime(),
            };
            return data;
        }

        public static void Update(EmployeeEducationalDetail entity, AddEmployeeEducationalDetailModel model)
        {
            entity.Education = model.Education;
            entity.NameOfBoardOrUniversity = model.NameOfBoardOrUniversity;
            entity.YearOfCompletion = model.YearOfCompletion;
            entity.MarksObtained = model.MarksObtained;
            entity. GPA = model.GPA;
            entity.UpdatedOn = Utility.GetDateTime();
        }
    }
}
