using lgs.web.api.IServices.BASE;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using System.Threading.Tasks;

namespace lgs.web.api.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<MessageModel<string>> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();
    }
}
