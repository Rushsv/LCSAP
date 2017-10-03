namespace LCSAP_LC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LCSAPDbContext : DbContext
    {
        public LCSAPDbContext()
            : base("name=LCSAPDbContext")
        {
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Campus> Campuses { get; set; }
        public virtual DbSet<Communication> Communications { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CRNSection> CRNSections { get; set; }
        public virtual DbSet<MessageNote> MessageNotes { get; set; }
        public virtual DbSet<SessionStyle> SessionStyles { get; set; }
        public virtual DbSet<SessionTime> SessionTimes { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentSession> StudentSessions { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }
        public virtual DbSet<TutorSession> TutorSessions { get; set; }
        public virtual DbSet<TutorType> TutorTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
                .Property(e => e.Area_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.StudentSessions)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campus>()
                .Property(e => e.Campus_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Campus>()
                .Property(e => e.Campus_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Campus>()
                .HasMany(e => e.StudentSessions)
                .WithRequired(e => e.Campus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campus>()
                .HasMany(e => e.TutorSessions)
                .WithRequired(e => e.Campus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Communication>()
                .Property(e => e.Tutor_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Course_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Course_Title)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Subject_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CRNSections)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRNSection>()
                .Property(e => e.Course_Id)
                .IsUnicode(false);

            modelBuilder.Entity<CRNSection>()
                .Property(e => e.Term_Id)
                .IsUnicode(false);

            modelBuilder.Entity<CRNSection>()
                .HasMany(e => e.StudentSessions)
                .WithRequired(e => e.CRNSection)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRNSection>()
                .HasMany(e => e.Students)
                .WithMany(e => e.CRNSections)
                .Map(m => m.ToTable("StudentCRNs").MapLeftKey("CRN_Id").MapRightKey("Student_Id"));

            modelBuilder.Entity<MessageNote>()
                .Property(e => e.Message_Description)
                .IsUnicode(false);

            modelBuilder.Entity<SessionStyle>()
                .Property(e => e.Style_Description)
                .IsUnicode(false);

            modelBuilder.Entity<SessionStyle>()
                .HasMany(e => e.TutorSessions)
                .WithRequired(e => e.SessionStyle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Student_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Student_FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Student_MName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Student_LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Student_Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Student_Major)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.StudentSessions)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudentSession>()
                .Property(e => e.Student_Id)
                .IsUnicode(false);

            modelBuilder.Entity<StudentSession>()
                .Property(e => e.Campus_Id)
                .IsUnicode(false);

            modelBuilder.Entity<StudentSession>()
                .HasMany(e => e.SessionTimes)
                .WithRequired(e => e.StudentSession)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .Property(e => e.Subject_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Subject>()
                .Property(e => e.Subject_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Tutors)
                .WithMany(e => e.Subjects)
                .Map(m => m.ToTable("TutorSubjects").MapLeftKey("Subject_Id").MapRightKey("Tutor_Id"));

            modelBuilder.Entity<Term>()
                .Property(e => e.Term_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Term>()
                .Property(e => e.Term_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Term>()
                .HasMany(e => e.CRNSections)
                .WithRequired(e => e.Term)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tutor>()
                .Property(e => e.Tutor_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Tutor>()
                .Property(e => e.Tutor_Fname)
                .IsUnicode(false);

            modelBuilder.Entity<Tutor>()
                .Property(e => e.Tutor_Lname)
                .IsUnicode(false);

            modelBuilder.Entity<Tutor>()
                .Property(e => e.Tutor_PayRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Tutor>()
                .HasMany(e => e.TutorSessions)
                .WithRequired(e => e.Tutor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TutorSession>()
                .Property(e => e.Session_Cost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TutorSession>()
                .Property(e => e.Tutor_Id)
                .IsUnicode(false);

            modelBuilder.Entity<TutorSession>()
                .Property(e => e.Campus_Id)
                .IsUnicode(false);

            modelBuilder.Entity<TutorSession>()
                .HasMany(e => e.SessionTimes)
                .WithRequired(e => e.TutorSession)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TutorType>()
                .Property(e => e.TutorType_description)
                .IsUnicode(false);

            modelBuilder.Entity<TutorType>()
                .HasMany(e => e.Tutors)
                .WithRequired(e => e.TutorType)
                .WillCascadeOnDelete(false);
        }
    }
}
