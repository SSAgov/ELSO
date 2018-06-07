namespace ELSO.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ELSO.Model;
    using System.Data.Entity.ModelConfiguration.Conventions;


    public partial class ELSOContext : DbContext
    {
        public ELSOContext()
            : base("name=ELSOContext")
        {
        }

        public virtual DbSet<DailyReccurence> DailyReccurence { get; set; }
        public virtual DbSet<WeeklyReccurence> WeeklyReccurence { get; set; }
        public virtual DbSet<MonthlyReccurence> MonthlyReccurence { get; set; }
        public virtual DbSet<Daily> Daily { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventMaxCount> EventMaxCount { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Monthly> Monthly { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonRole> PersonRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Pattern> Pattern { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<PatternType> PatternType { get; set; }
        public virtual DbSet<Weekly> Weekly { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Daily>()
                .Property(e => e.DayDescp)
                .IsUnicode(false);

            modelBuilder.Entity<Daily>()
                .HasMany(e => e.DailyReccurence)
                .WithRequired(e => e.Daily)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>()
                .Property(e => e.EventName)
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .Property(e => e.ETypeCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
               .Property(e => e.eventDescription)
               .IsFixedLength()
               .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventMaxCount)
                .WithRequired(e => e.Event)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Patterns)
                .WithRequired(e => e.Event)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Event>()
            //    .HasMany(e => e.Sessions)
            //    .WithRequired(e => e.Event)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EventMaxCount>()
                .Property(e => e.RoleCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EventType>()
                .Property(e => e.ETypeCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EventType>()
                .Property(e => e.EventDescp)
                .IsUnicode(false);

            modelBuilder.Entity<Monthly>()
                .Property(e => e.MonthDescp)
                .IsUnicode(false);

            modelBuilder.Entity<Monthly>()
                .HasMany(e => e.MonthlyReccurence)
                .WithRequired(e => e.Monthly)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Registration>()
                .Property(e => e.RoleCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.Organization)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.SSA_PIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Registrations)
                .WithRequired(e => e.Person)
                .HasForeignKey(e => e.PersonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.PersonRoles)
                .WithRequired(e => e.Person)
                .HasForeignKey(e => e.PersonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Person)
                .HasForeignKey(e => e.PersonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonRole>()
                .Property(e => e.RoleCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.RoleEventCount)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Attendance>()
                .Property(e => e.RoleCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pattern>()
                .HasMany(e => e.DailyReccurences)
                .WithRequired(e => e.Pattern)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pattern>()
                .HasMany(e => e.WeeklyReccurences)
                .WithRequired(e => e.Pattern)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pattern>()
                .HasMany(e => e.MonthlyReccurences)
                .WithRequired(e => e.Pattern)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pattern>()
                .HasMany(e => e.Sessions)
                .WithRequired(e => e.Pattern)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Session>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Session)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PatternType>()
                .Property(e => e.PatternDescp)
                .IsUnicode(false);

            modelBuilder.Entity<PatternType>()
                .HasMany(e => e.Patterns)
                .WithRequired(e => e.PatternType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Weekly>()
                .Property(e => e.WeekDescp)
                .IsUnicode(false);

            modelBuilder.Entity<Weekly>()
                .HasMany(e => e.WeeklyReccurences)
                .WithRequired(e => e.Weekly)
                .WillCascadeOnDelete(false);
        }
    }
}
