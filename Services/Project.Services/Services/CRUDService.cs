using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.Models;
using Project.Services.Interfaces;

namespace Project.Services.Services
{
    public class CRUDService<PureClass, ServiceModel> : BaseService, ICRUDService<ServiceModel>
        where PureClass : BaseClass
        where ServiceModel : class
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;

        public CRUDService(DbContext context, IMapper mapper)
            : base(context)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public virtual async Task<ServiceModel> CreateAsync(ServiceModel serviceModel)
        {
            PureClass pureClass = this._mapper.Map<PureClass>(serviceModel);

            Guid id = Guid.NewGuid();
            pureClass.Id = id;

            await this._context.AddAsync(pureClass);
            await base.SaveChangesAsync();

            return this._mapper.Map<ServiceModel>(pureClass);
        }

        public virtual async Task<ServiceModel> GetByIdAsync(Guid id)
        {
            PureClass pureClass = await this.GetPureClassByIdAsync(id);

            return this._mapper.Map<ServiceModel>(pureClass);
        }

        public virtual async Task<ServiceModel> UpdateAsync(ServiceModel serviceModel)
        {
            PureClass pureClass = this._mapper.Map<PureClass>(serviceModel);

            this._context.Set<PureClass>().Update(pureClass);
            await base.SaveChangesAsync();

            return this._mapper.Map<ServiceModel>(pureClass);
        }

        public virtual async Task<ServiceModel> DeleteAsync(Guid id)
        {
            PureClass pureClass = await this.GetPureClassByIdAsync(id);

            return this._mapper.Map<ServiceModel>(pureClass);
        }

        private async Task<PureClass> GetPureClassByIdAsync(Guid id)
        {
            return await this._context
                .Set<PureClass>()
                .FindAsync(id);
        }
    }
}
