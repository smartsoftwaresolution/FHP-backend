using FHP.dtos.FHP.Offer;
using FHP.models.FHP.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IOfferManager
    {
        Task AddAsync(AddOfferModel model);
        Task Edit(AddOfferModel model);
        Task<(List<OfferDetailDto> offer, int totalCount)> GetAllAsync(int page, int pageSize, string? search,int employeeId,int employerId);
        Task<OfferDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);

        // Task<string> OfferAcceptRejectAsync(int id, int jobId, int employeeId);
        Task<string> OfferAcceptRejectAsync(SetOfferStatusModel model);
    }
}
