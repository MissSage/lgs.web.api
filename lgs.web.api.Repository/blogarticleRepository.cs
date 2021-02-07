using lgs.web.api.IRepository;
using lgs.web.api.IRepository.UnitOfWork;
using lgs.web.api.Model.Models;
using lgs.web.api.Repository.Base;

namespace lgs.web.api.Repository
{
	/// <summary>
	/// blogarticleRepository
	/// </summary>
    public class blogarticleRepository : BaseRepository<blogarticle>, IblogarticleRepository
    {
        public blogarticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}