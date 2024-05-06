using FHP.dtos.FHP.Offer;
using FHP.entity.FHP;
using FHP.models.FHP.Offer;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IOfferRepository
    {
        Task AddAsync(Offer entity);
        void Edit(Offer entity);
        Task<Offer> GetAsync(int id);
        Task<(List<OfferDetailDto> offer, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int employeeId, int employerId);
        Task<OfferDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        // Task<string> OfferAcceptRejectAsync(int id, int jobId, int employeeId);

        Task<string> OfferAcceptRejectAsync(SetOfferStatusModel model);
    }
}
