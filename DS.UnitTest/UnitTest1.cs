using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Moq;
using DS.Bll.Models;
using DS.Data.Pocos;
using DS.Data.Repository.Interfaces;
using Xunit;
using DS.Extensions;
using DS.Bll.Interfaces;
using DS.Helper.Interfaces;

namespace DS.UnitTest
{
    public class UnitTest1
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;

        private readonly IMapper _mapper;

        private readonly Mock<IUtilityService> _utility;

        private readonly Mock<IAttachment> _attachment;

        private readonly Mock<IElasticSearch<CaSearchViewModel>> _elastic;

        public UnitTest1()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<MappingProfile>()));
            _utility = new Mock<IUtilityService>();
            _attachment = new Mock<IAttachment>();
            _elastic = new Mock<IElasticSearch<CaSearchViewModel>>();
        }

        [Fact]
        public void TestGetData()
        {
            var caModel = new DS.Data.Pocos.Ca
            {
                Id = 9999999,
                RequestFor = "System",
                Amount = 99999999
            };

            _unitOfWork.Setup(repo => repo.GetRepository<Ca>().GetById(caModel.Id)).Returns(caModel);

            var service = new DS.Bll.CaBll(_unitOfWork.Object, _mapper, _attachment.Object, _utility.Object, _elastic.Object);

            var result = service.Get(9999999);

            Assert.Equal(result.Id, caModel.Id);
            
        }

    }
}