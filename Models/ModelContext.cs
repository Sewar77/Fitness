using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyFitnessLife.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Membershipplan> Membershipplans { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Trainerassignment> Trainerassignments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }
    public object UserFeedback { get; internal set; }
    public object Profiles { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=Fitness; Password=Test321;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("FITNESS")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Bankid).HasName("SYS_C008077");

            entity.ToTable("BANK");

            entity.Property(e => e.Bankid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("BANKID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Card)
                .HasColumnType("NUMBER")
                .HasColumnName("CARD");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Feedbackid).HasName("SYS_C008122");

            entity.ToTable("FEEDBACKS");

            entity.Property(e => e.Feedbackid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("FEEDBACKID");
            entity.Property(e => e.Approved)
                .HasPrecision(1)
                .HasDefaultValueSql("0")
                .HasColumnName("APPROVED");
            entity.Property(e => e.Feedbacktext)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("FEEDBACKTEXT");
            entity.Property(e => e.Submittedat)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("SUBMITTEDAT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008123");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Invoiceid).HasName("SYS_C008116");

            entity.ToTable("INVOICES");

            entity.Property(e => e.Invoiceid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("INVOICEID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Invoicedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATE")
                .HasColumnName("INVOICEDATE");
            entity.Property(e => e.Pdfpath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PDFPATH");
            entity.Property(e => e.Subscriptionid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SUBSCRIPTIONID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Subscriptionid)
                .HasConstraintName("SYS_C008117");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008118");
        });

        modelBuilder.Entity<Membershipplan>(entity =>
        {
            entity.HasKey(e => e.Planid).HasName("SYS_C008090");

            entity.ToTable("MEMBERSHIPPLANS");

            entity.Property(e => e.Planid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PLANID");
            entity.Property(e => e.Createdat)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CREATEDAT");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Durationinmonths)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("DURATIONINMONTHS");
            entity.Property(e => e.Planname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PLANNAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008073");

            entity.ToTable("ROLES");

            entity.HasIndex(e => e.Rolename, "SYS_C008074").IsUnique();

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Subscriptionid).HasName("SYS_C008097");

            entity.ToTable("SUBSCRIPTIONS");

            entity.Property(e => e.Subscriptionid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SUBSCRIPTIONID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Enddate)
                .HasColumnType("DATE")
                .HasColumnName("ENDDATE");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'Pending' ")
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Planid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PLANID");
            entity.Property(e => e.Startdate)
                .HasColumnType("DATE")
                .HasColumnName("STARTDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Plan).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.Planid)
                .HasConstraintName("SYS_C008099");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008098");
        });

        modelBuilder.Entity<Trainerassignment>(entity =>
        {
            entity.HasKey(e => e.Assignmentid).HasName("SYS_C008109");

            entity.ToTable("TRAINERASSIGNMENTS");

            entity.Property(e => e.Assignmentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ASSIGNMENTID");
            entity.Property(e => e.Assignedat)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("ASSIGNEDAT");
            entity.Property(e => e.Memberid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MEMBERID");
            entity.Property(e => e.Trainerid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAINERID");

            entity.HasOne(d => d.Member).WithMany(p => p.TrainerassignmentMembers)
                .HasForeignKey(d => d.Memberid)
                .HasConstraintName("SYS_C008111");

            entity.HasOne(d => d.Trainer).WithMany(p => p.TrainerassignmentTrainers)
                .HasForeignKey(d => d.Trainerid)
                .HasConstraintName("SYS_C008110");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008082");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Username, "SYS_C008083").IsUnique();

            entity.HasIndex(e => e.Email, "SYS_C008084").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Birthdate)
                .HasColumnType("DATE")
                .HasColumnName("BIRTHDATE");
            entity.Property(e => e.Createdat)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CREATEDAT");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GENDER");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("SYS_C008085");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.Visitorid).HasName("SYS_C008138");

            entity.ToTable("VISITOR");

            entity.Property(e => e.Visitorid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("VISITORID");
            entity.Property(e => e.Devicetype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DEVICETYPE");
            entity.Property(e => e.Hasregistered)
                .HasPrecision(1)
                .HasDefaultValueSql("0")
                .HasColumnName("HASREGISTERED");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IP_ADDRESS");
            entity.Property(e => e.Pagevisited)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PAGEVISITED");
            entity.Property(e => e.Referralsource)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REFERRALSOURCE");
            entity.Property(e => e.Referralurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("REFERRALURL");
            entity.Property(e => e.Sessionid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SESSIONID");
            entity.Property(e => e.Useragent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USERAGENT");
            entity.Property(e => e.Visittime)
                .HasDefaultValueSql("SYSDATE")
                .HasColumnType("DATE")
                .HasColumnName("VISITTIME");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.Workoutid).HasName("SYS_C008103");

            entity.ToTable("WORKOUTS");

            entity.Property(e => e.Workoutid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("WORKOUTID");
            entity.Property(e => e.Createdat)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CREATEDAT");
            entity.Property(e => e.Goals)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GOALS");
            entity.Property(e => e.Memberid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MEMBERID");
            entity.Property(e => e.Trainerid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAINERID");

            entity.HasOne(d => d.Member).WithMany(p => p.WorkoutMembers)
                .HasForeignKey(d => d.Memberid)
                .HasConstraintName("SYS_C008105");

            entity.HasOne(d => d.Trainer).WithMany(p => p.WorkoutTrainers)
                .HasForeignKey(d => d.Trainerid)
                .HasConstraintName("SYS_C008104");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
