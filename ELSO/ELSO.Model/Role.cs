namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRSN_ROLE_CD_REF")]
    public partial class Role : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            RoleEventCount = new HashSet<EventMaxCount>();
            Registrations = new HashSet<Registration>();
            PersonRoles = new HashSet<PersonRole>();
            Attendances = new HashSet<Attendance>();
        }

        [Key]
        [StringLength(4)]
        [Column("PRSN_ROLE_CD")]
        public string Code { get; set; }

        [Column("EVNT_SCP_SW")]
        public bool IsEventRole { get; set; }

        [Required]
        [StringLength(254)]
        [Column("PRSN_ROLE_CDESC")]
        public string Description { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventMaxCount> RoleEventCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Registration> Registrations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonRole> PersonRoles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
