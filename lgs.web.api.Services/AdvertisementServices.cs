using lgs.web.api.IRepository.Base;
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;

namespace lgs.web.api.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        IBaseRepository<Advertisement> _dal;
        public AdvertisementServices(IBaseRepository<Advertisement> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public void ReturnExp()
        {

            int a = 1;
            int b = 0;

            int c = a / b;
        }

        //public IAdvertisementRepository dal = new AdvertisementRepository();
        //public int Sum(int i, int j)
        //{
        //    return base.Sum(i, j);

        //}


        //public int Add(Advertisement model)
        //{
        //    return base.Add(model);
        //}

        //public bool Delete(Advertisement model)
        //{
        //    return base.Delete(model);
        //}

        //public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        //{
        //    return base.Query(whereExpression);

        //}

        //public bool Update(Advertisement model)
        //{
        //    return base.Update(model);
        //}

    }
}
