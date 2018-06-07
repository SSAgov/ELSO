using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELSO.Model
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }      
        //public int Id { get; set; }
        [Column("INSRT_TS", TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        [Column("LU_TS", TypeName = "datetime2")]
        public DateTime ModifiedDate { get; set; }
    }
}
