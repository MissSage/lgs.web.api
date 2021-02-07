using lgs.web.api.IRepository;
using lgs.web.api.IRepository.UnitOfWork;
using lgs.web.api.Model.Models;
using lgs.web.api.Repository.Base;

namespace lgs.web.api.Repository
{
	/// <summary>
	/// blogarticle_commentsRepository
	/// </summary>
    public class blogarticle_commentsRepository : BaseRepository<blogarticle_comments>, Iblogarticle_commentsRepository
    {
        public blogarticle_commentsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}