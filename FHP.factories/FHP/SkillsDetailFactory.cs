using FHP.entity.FHP;
using FHP.models.FHP.SkillDetail;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class SkillsDetailFactory
    {
        public static SkillsDetail Create(AddSkillsDetailModel model)
        {
            var data = new SkillsDetail
            {
                UserId = model.UserId,
                SkillName = model.SkillName,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
            };
            return data;
        }

        public static void Update(SkillsDetail entity,AddSkillsDetailModel model)
        {

            entity.UserId = model.UserId;
            entity.SkillName = model.SkillName;
            entity.UpdatedOn = Utility.GetDateTime();
        }
    }
}
