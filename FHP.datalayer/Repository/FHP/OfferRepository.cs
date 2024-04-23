using FHP.dtos.FHP.Offer;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.Offer;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;

namespace FHP.datalayer.Repository.FHP
{
    public class OfferRepository : IOfferRepository
    {
        private readonly DataContext _dataContext;

        public OfferRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Offer entity)
        {
            await _dataContext.Offers.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }
                   
        public void Edit(Offer entity)
        {
            _dataContext.Offers.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<Offer> GetAsync(int id)
        {
            return await _dataContext.Offers.Where(s => s.Id == id).FirstOrDefaultAsync();
        }


        public async Task<(List<OfferDetailDto> offer, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int employeeId, int employerId)
        {

            var query = from  s in _dataContext.Offers
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { offer = s };

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.offer.EmployerId.ToString().Contains(search) ||
                                         s.offer.EmployeeId.ToString().Contains(search) ||
                                         s.offer.Title.Contains(search) ||
                                         s.offer.Description.Contains(search));
            }

            if(employeeId > 0)
            {
                query = query.Where(e => e.offer.EmployeeId == employeeId);
            }

            if(employerId > 0)
            {
                query = query.Where(e => e.offer.EmployerId == employerId);
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(s => s.offer.Id);

            if(page > 0 && pageSize  > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var data = await query.Select(s => new OfferDetailDto
                                                    {
                                                                Id = s.offer.Id,
                                                                JobId = s.offer.JobId,
                                                                EmployeeId = s.offer.EmployeeId,
                                                                EmployerId = s.offer.EmployerId,
                                                                Title = s.offer.Title,
                                                                Description = s.offer.Description,
                                                              //  IsAccepted = s.offer.IsAccepted,
                                                              IsAvaliable = s.offer.IsAvaliable,
                                                              CancelReason = s.offer.CancelReason,
                                                                Status = s.offer.Status,
                                                                CreatedOn = s.offer.CreatedOn,
                                                                UpdatedOn = s.offer.UpdatedOn,
                                                    })
                                                   .AsNoTracking()
                                                   .ToListAsync();


            return (data, totalCount);
        }

        public async Task<OfferDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.Offers
                          where s.Status != Constants.RecordStatus.Deleted && 
                          s.Id == id

                          select new OfferDetailDto
                                                   {
                                                      Id = s.Id,
                                                      JobId = s.JobId,
                                                      EmployeeId = s.EmployeeId,
                                                      EmployerId = s.EmployerId,
                                                      Title = s.Title,
                                                      Description = s.Description,
                                                     // IsAccepted = s.IsAccepted,
                                                     IsAvaliable = s.IsAvaliable,
                                                    CancelReason = s.CancelReason,
                                                      Status = s.Status,
                                                      CreatedOn = s.CreatedOn,
                                                      UpdatedOn = s.UpdatedOn,
                                                   })
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.Offers.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;  
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<string> OfferAcceptRejectAsync(SetOfferStatusModel model)
        {
            string result = string.Empty;

            var data = await _dataContext.Offers.Where(s => s.EmployeeId == model.EmployeeId && s.EmployerId == model.EmployerId && s.JobId == model.JobId).FirstOrDefaultAsync();

            if(model.IsAvaliable == Constants.OfferStatus.Accepted)
            {
                data.IsAvaliable = Constants.OfferStatus.Accepted;
                data.CancelReason = model.CancelReason;
                data.UpdatedOn = Utility.GetDateTime();
                result = "Accepted";
            }

            else
            {
                data.IsAvaliable = Constants.OfferStatus.Rejected;
                data.CancelReason = model.CancelReason;
                data.UpdatedOn = Utility.GetDateTime();
                result = "Rejected";
            }

            _dataContext.Offers.Update(data);
            await _dataContext.SaveChangesAsync();
            return result; 
        }

        /* public async Task<string> OfferAcceptRejectAsync(int id, int jobId, int employeeId)
         {
             string result = string.Empty;

             var data = await _dataContext.Offers.Where(s => s.Id == id && s.JobId == jobId && s.EmployeeId == employeeId).FirstOrDefaultAsync();

             if(data.IsAccepted == false)
             {
                 data.IsAccepted = true;
                 result = "Accepted";
             }
             else
             {
                 data.IsAccepted = false;
                 result = "Reject";
             }

             _dataContext.Offers.Update(data);
             await _dataContext.SaveChangesAsync();
             return result;
         }*/
    }
}
