using lgs.web.api.IRepository;
using lgs.web.api.IRepository.UnitOfWork;
using lgs.web.api.Model.Models;
using lgs.web.api.Repository.Base;

namespace lgs.web.api.Repository
{
	/// <summary>
	/// user_prefersRepository
	/// </summary>
    public class user_prefersRepository : BaseRepository<user_prefers>, Iuser_prefersRepository
    {
        public user_prefersRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}