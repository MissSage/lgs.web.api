using lgs.web.api.IRepository;
using lgs.web.api.IRepository.UnitOfWork;
using lgs.web.api.Model.Models;
using lgs.web.api.Repository.Base;

namespace lgs.web.api.Repository
{
	/// <summary>
	/// blogarticle_tagsRepository
	/// </summary>
    public class blogarticle_tagsRepository : BaseRepository<blogarticle_tags>, Iblogarticle_tagsRepository
    {
        public blogarticle_tagsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}