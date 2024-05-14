using FHP.entity.FHP;
using FHP.models.FHP.Offer;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class OfferFactory
    {
        public static Offer Create(AddOfferModel model)
        {
            var data = new Offer
            {
                JobId = model.JobId,
                EmployeeId = model.EmployeeId,
                EmployerId = model.EmployerId,
                Title = model.Title,
                Description = model.Description,
               // IsAccepted = model.IsAccepted,
               IsAvaliable = Constants.OfferStatus.Pending,
              // CancelReason = model.CancelReason,
                Status = Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime(),
            };

            return data;
        }

        public static void update(Offer entity,AddOfferModel model)
        {
            entity.JobId = model.JobId;
            entity.EmployeeId = model.EmployeeId;
            entity.EmployerId = model.EmployerId;
            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.UpdatedOn = Utility.GetDateTime();   
        }
    }
}
