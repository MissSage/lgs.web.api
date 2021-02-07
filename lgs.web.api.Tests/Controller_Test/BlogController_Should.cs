using Autofac;
using lgs.web.api.Controllers;
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace lgs.web.api.Tests
{
    public class BlogController_Should
    {
        Mock<IblogarticleServices> mockBlogSev = new Mock<IblogarticleServices>();
        Mock<ILogger<BlogController>> mockLogger = new Mock<ILogger<BlogController>>();
        BlogController blogController;

        private IblogarticleServices blogarticleServices;
        DI_Test dI_Test = new DI_Test();



        public BlogController_Should()
        {
            mockBlogSev.Setup(r => r.Query());


            var container = dI_Test.DICollections();
            blogarticleServices = container.Resolve<IblogarticleServices>();
            blogController = new BlogController(blogarticleServices, mockLogger.Object);
        }

        [Fact]
        public void TestEntity()
        {
            blogarticle blogarticle = new blogarticle();

            Assert.True(blogarticle.Id >= 0);
        }
        [Fact]
        public async void GetBlogsTest()
        {
            object blogs =await blogController.Get(1);

            Assert.NotNull(blogs);
        }

        [Fact]
        public async void PostTest()
        {
            blogarticle blogarticle = new blogarticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint :test controller addEntity",

            };

            var res = await blogController.Post(blogarticle);

            Assert.True(res.success);

            var data = res.response;

            Assert.NotNull(data);
        }
    }
}
