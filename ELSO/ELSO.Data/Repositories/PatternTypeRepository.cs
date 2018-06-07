using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ELSO.Model;

namespace ELSO.Data.Repositories
{
    /// <summary>
    /// Specific queries for events
    /// </summary>
    public class PatternTypeRepository : Repository<PatternType>
    {
        public PatternTypeRepository(dELSO3AEntities context)
            : base(context)
        {
        }
     

        /// <summary>
        /// Get an event by its unique id
        /// </summary>
        /// <param name="desc">event id</param>
        /// <returns>a single event</returns>
        public PatternType GetByDescription(string desc)
        {
            var type = _context.PatternType
               .Where(s => s.PatternDescp.ToLower() == desc.ToLower())              
               .FirstOrDefault();
            return type;
        }     
      
    }
}
