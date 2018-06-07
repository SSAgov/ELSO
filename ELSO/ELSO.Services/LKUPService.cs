using ELSO.Data;
using ELSO.Data.Repositories;
using ELSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ELSO.Services
{
    public class LKUPService
    {
        private UnitOfWork _uow;
        public LKUPService()
        {
            _uow = new UnitOfWork();
        }

        // TODO: will need to create a better way to retrieve reference tables through repository pattern
        public IEnumerable<PatternType> GetPatternByID(int id)
        {
            var repo =  _uow.PatternTypeRepo.GetWithRawSql($"Select * from RCURPATRN_TYP_REF where [RCURPATRN_TYP_UID] = {id}").Cast<PatternType>();
            return repo;
        }
        // TODO: will need to create a better way to retrieve reference tables through repository pattern
        public PatternType GetPatternByDesc(string desc)
        {
            return _uow.PatternTypeRepo.GetByDescription(desc);
        }
    }
}
