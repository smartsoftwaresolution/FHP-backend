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
    public class EmployeeSkillDetailFactory
    {
        public static EmployeeSkillDetail Create(AddEmployeeSkillDetailModel model)
        {
            var data = new EmployeeSkillDetail
            {
                UserId = model.UserId,
                SkillId = model.SkillId,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active
            };
            return data;
        }

        public static void Update(EmployeeSkillDetail entity,AddEmployeeSkillDetailModel model)
        {
            entity.UserId = model.UserId;
            entity.SkillId = model.SkillId;
            entity.UpdatedOn = Utility.GetDateTime();

        }
    }
}
