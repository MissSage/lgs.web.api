using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using Xunit;
using System;
using System.Linq;
using Autofac;

namespace lgs.web.api.Tests
{
    public class blogarticleService_Should
    {

        private IblogarticleServices blogarticleServices;
        DI_Test dI_Test = new DI_Test();


        public blogarticleService_Should()
        {
            //mockBlogRep.Setup(r => r.Query());

            var container = dI_Test.DICollections();

            blogarticleServices = container.Resolve<IblogarticleServices>();

        }


        [Fact]
        public void blogarticleServices_Test()
        {
            Assert.NotNull(blogarticleServices);
        }


        //[Fact]
        //public async void Get_Blogs_Test()
        //{
        //    var data = await blogarticleServices.GetBlogs();

        //    Assert.True(data.Any());
        //}

        [Fact]
        public async void Add_Blog_Test()
        {
            blogarticle blogarticle = new blogarticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint test title",
                bcontent = "xuint test content",
                bsubmitter = 1,
            };

            var BId = await blogarticleServices.Add(blogarticle);

            Assert.True(BId > 0);
        }

        [Fact]
        public async void Delete_Blog_Test()
        {
            var deleteModel = (await blogarticleServices.Query(d => d.btitle == "xuint test title")).FirstOrDefault();

            Assert.NotNull(deleteModel);

            var IsDel = await blogarticleServices.Delete(deleteModel);

            Assert.True(IsDel);
        }
    }
}
