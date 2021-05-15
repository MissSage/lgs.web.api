using lgs.web.api.IRepository;
using lgs.web.api.IRepository.UnitOfWork;
using lgs.web.api.Model.Models;
using lgs.web.api.Repository.Base;

namespace lgs.web.api.Repository
{
	/// <summary>
	/// blogarticle_prefersRepository
	/// </summary>
    public class blogarticle_prefersRepository : BaseRepository<blogarticle_prefers>, Iblogarticle_prefersRepository
    {
        public blogarticle_prefersRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}