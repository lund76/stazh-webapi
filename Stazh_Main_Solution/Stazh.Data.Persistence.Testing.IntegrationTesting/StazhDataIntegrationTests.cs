using System;
using System.Collections.Generic;
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
            _stazhDataContext = new StazhDataContext($@"Server=.\SQLEXPRESS;Database=Stazh_db_{Guid.NewGuid().ToString()};Trusted_Connection=True;");
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
        public void InsertItemAndRetrieveItAgain()
        {
            _unitOfWork.Items.Add(new Item { Created = DateTime.UtcNow, Description = "A" });
            _unitOfWork.Complete();
            _currentItem = _unitOfWork.Items.Get(1);
            Assert.True(_currentItem.Description == "A");
        }

        [Test]
        [Category("CRUD"), Order(2)]
        public void DeleteItemTest()
        {
            _unitOfWork.Items.Remove(_currentItem);
            _unitOfWork.Complete();

            Assert.Null(_unitOfWork.Items.Get(1));

        }

        [Test]
        public void CrateItemWithChildren()
        {

            var user = new User { ExternalUniqueId = "user1" };
            var parentItem = new Item { Description = "alpha", Owner = user };
            var itemChildren = new HashSet<Item>
            {
                new Item {Description = "x"}, new Item {Description = "y"}, new Item {Description = "z"}
            };

            parentItem.Children = itemChildren;

            _unitOfWork.Items.Add(parentItem);
            _unitOfWork.Complete();

            var itm = _unitOfWork.Items.Find(u => u.Owner.ExternalUniqueId == "user1").FirstOrDefault();
            Assert.AreEqual(itm.Owner.ExternalUniqueId, "user1");
            Assert.AreEqual(itm.Children.Single(c => c.Description == "y").Description, "y");
        }

    }
}