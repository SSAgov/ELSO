//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ANNL_RCURPATRN_MNTHS")]
    public partial class MonthlyReccurence : BaseEntity
    {
        [Key]
        [Column("MNTH_UID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column("RCURPATRN_UID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReccurenceId { get; set; }
       
        public virtual Monthly MNTH_REF { get; set; }
        public virtual Pattern Pattern { get; set; }
    }
}