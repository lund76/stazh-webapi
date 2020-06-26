using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Stazh.Core.Data;
using Stazh.Core.Data.Entities;

namespace Stazh.Data.Persistence.Testing.IntegrationTesting
{
    public class StazhDataIntegrationTests
    {
        private IUnitOfWork _unitOfWork;
        private StazhDataContext _stazhDataContext;
        private Item _currentItem;

        [OneTimeSetUp]
        public void Setup()
        {
            _stazhDataContext  = new StazhDataContext($@"Server=.\SQLEXPRESS;Database=Stazh_db_{Guid.NewGuid().ToString()};Trusted_Connection=True;");
            _stazhDataContext.Database.Migrate();
            _unitOfWork = new UnitOfWork(_stazhDataContext);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _stazhDataContext.Database.EnsureDeleted();
        }
        [Test, Order(1)]
        [Category("CRUD")]
        public void InsertItemAndRetriveItAgain()
        {
            _unitOfWork.Items.Add(new Item{Created = DateTime.UtcNow,Description = "A"});
            _unitOfWork.Complete();
            _currentItem = _unitOfWork.Items.Get(1);
            Assert.True(_currentItem.Description == "A");
        }

        [Test]
        [Category("CRUD"),Order(2)]
        public void DeleteItemTest()
        {
            _unitOfWork.Items.Remove(_currentItem);
            _unitOfWork.Complete();
            
            Assert.Null( _unitOfWork.Items.Get(1)) ;

        }

       
    }
}