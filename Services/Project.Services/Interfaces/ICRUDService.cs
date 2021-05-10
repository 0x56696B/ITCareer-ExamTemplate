using System;
using System.Threading.Tasks;

namespace Project.Services.Interfaces
{
    public interface ICRUDService<TServiceModel>
        where TServiceModel : class
    {
        Task<TServiceModel> CreateAsync(TServiceModel serviceModel);

        Task<TServiceModel> GetByIdAsync(Guid id);

        Task<TServiceModel> UpdateAsync(TServiceModel serviceModel);

        Task<TServiceModel> DeleteAsync(Guid id);
    }
}
