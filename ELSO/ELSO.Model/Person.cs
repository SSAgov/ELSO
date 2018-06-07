namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRSN")]
    public partial class Person : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            Registrations = new HashSet<Registration>();
            PersonRoles = new HashSet<PersonRole>();
            Attendances = new HashSet<Attendance>();
            Organization = "SSA";
        }

        [Key]
        [Column("PRSN_UID")]
        public int Id { get; set; }

        [Required]
        [StringLength(62),Column("ORG_NM")]
        public string Organization { get; set; }

        [Required]
        [StringLength(62),Column("FNM")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(62),Column("LNM")]
        public string LastName { get; set; }

        [StringLength(126),Column("EMAILADDR")]
        public string Email { get; set; }

        [StringLength(30),Column("PHNNUM")]
        public string Phone { get; set; }

        public bool LGCY_ADMINR_SW { get; set; }

        [StringLength(6)]
        public string SSA_PIN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Registration> Registrations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonRole> PersonRoles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
