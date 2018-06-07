using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELSO.Model;

namespace ELSO.Data.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private dELSO3AEntities _context = new dELSO3AEntities();
        private EventRepository _eventRepo;
        private PersonRepository _peopleRepo;
        private PatternTypeRepository _patternTypeRepo;

        public PatternTypeRepository PatternTypeRepo
        {
            get
            {

                if (this._patternTypeRepo == null)
                {
                    this._patternTypeRepo = new PatternTypeRepository(_context);
                }
                return _patternTypeRepo;
            }
        }

        public PersonRepository PeopleRepository
        {
            get
            {

                if (this._peopleRepo == null)
                {
                    this._peopleRepo = new PersonRepository(_context);
                }
                return _peopleRepo;
            }
        }

        public EventRepository EventRepository
        {
            get
            {

                if (this._eventRepo == null)
                {
                    this._eventRepo = new EventRepository(_context);
                }
                return _eventRepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
