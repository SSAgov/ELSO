using Microsoft.VisualStudio.TestTools.UnitTesting;
using ELSO.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELSO.Model;
using ELSO.Data;
namespace ELSO.Data.Repositories.Tests
{

    [TestClass()]
    public class EventTest
    {
        private IUnitOfWork _uow;
        private EventRepository _repo;
        private IEnumerable<Event> _meetings; 
        //
        [TestMethod()]
        public void GetAll()
        {
            // TODO : meets to be more meaningful
            var uow = new UnitOfWork(new DbContextFactory());
            var repo = new EventRepository(uow);
            var meetings = repo.GetAll();
            Assert.IsNotNull(meetings);
        }
        [TestMethod()]
        public void Save()
        { 
            int result = _repo.Save(createEvent());
            Assert.Equals(1, result);
        }
        private Event createEvent()
        {
            return new Event
            {
                Category = Category(),
                CatetoryId = Category().Id,
                CreatedDate = DateTime.Now,
                EventName = "Test Event1",
                Id = 1,
                OrganizerId = 90,
                ModifiedDate = DateTime.Now,
                Location = "SSA Crash site",
                EventEndDate = DateTime.Today.AddDays(1),
                EventStartDate = DateTime.Now
                
            };
        }
        private IEnumerable<Event> GetAllEvents()
        {
            _uow = new UnitOfWork(new DbContextFactory());
            _repo = new EventRepository(_uow);
            var meetings = _repo.GetAll();
            return _meetings =  meetings;
        }
        private EventCategory Category()
        {
            return new EventCategory
            {
                CreatedDate = DateTime.Now,
                Description = "Training",
                Id = 11,
                ModifiedDate = DateTime.Now
            };
        }
    }
}