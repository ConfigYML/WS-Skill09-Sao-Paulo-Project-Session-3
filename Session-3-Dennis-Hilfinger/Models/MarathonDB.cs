using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Session_3_Dennis_Hilfinger;

public partial class MarathonDB : DbContext
{
    public MarathonDB()
    {
    }

    public MarathonDB(DbContextOptions<MarathonDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Charity> Charities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Marathon> Marathons { get; set; }

    public virtual DbSet<RaceKitOption> RaceKitOptions { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<RegistrationEvent> RegistrationEvents { get; set; }

    public virtual DbSet<RegistrationStatus> RegistrationStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Runner> Runners { get; set; }

    public virtual DbSet<Sponsorship> Sponsorships { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffPosition> StaffPositions { get; set; }

    public virtual DbSet<StaffTimesheet> StaffTimesheets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Volunteer> Volunteers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarathonSkillsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Charity>(entity =>
        {
            entity.HasKey(e => e.CharityId).HasName("pk_Charity");

            entity.ToTable("Charity");

            entity.Property(e => e.CharityDescription).HasMaxLength(2000);
            entity.Property(e => e.CharityLogo).HasMaxLength(50);
            entity.Property(e => e.CharityName).HasMaxLength(100);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode).HasName("pk_Country");

            entity.ToTable("Country");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.CountryFlag).HasMaxLength(100);
            entity.Property(e => e.CountryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("pk_Event");

            entity.ToTable("Event");

            entity.Property(e => e.EventId)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EventName).HasMaxLength(50);
            entity.Property(e => e.EventTypeId)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__EventType__5BE2A6F2");

            entity.HasOne(d => d.Marathon).WithMany(p => p.Events)
                .HasForeignKey(d => d.MarathonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__MarathonI__5CD6CB2B");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("pk_EventType");

            entity.ToTable("EventType");

            entity.Property(e => e.EventTypeId)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.EventTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Gender1).HasName("pk_Gender");

            entity.ToTable("Gender");

            entity.Property(e => e.Gender1)
                .HasMaxLength(10)
                .HasColumnName("Gender");
        });

        modelBuilder.Entity<Marathon>(entity =>
        {
            entity.HasKey(e => e.MarathonId).HasName("pk_Marathon");

            entity.ToTable("Marathon");

            entity.Property(e => e.MarathonId).ValueGeneratedOnAdd();
            entity.Property(e => e.CityName).HasMaxLength(80);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.MarathonName).HasMaxLength(80);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Marathons)
                .HasForeignKey(d => d.CountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Marathon__Countr__5FB337D6");
        });

        modelBuilder.Entity<RaceKitOption>(entity =>
        {
            entity.HasKey(e => e.RaceKitOptionId).HasName("pk_RaceKitOption");

            entity.ToTable("RaceKitOption");

            entity.Property(e => e.RaceKitOptionId)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RaceKitOption1)
                .HasMaxLength(80)
                .HasColumnName("RaceKitOption");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("pk_Registration");

            entity.ToTable("Registration");

            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RaceKitOptionId)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.RegistrationDateTime).HasColumnType("datetime");
            entity.Property(e => e.SponsorshipTarget).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Charity).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.CharityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Chari__5AEE82B9");

            entity.HasOne(d => d.RaceKitOption).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.RaceKitOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__RaceK__59063A47");

            entity.HasOne(d => d.RegistrationStatus).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.RegistrationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Regis__59FA5E80");

            entity.HasOne(d => d.Runner).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.RunnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Runne__5812160E");
        });

        modelBuilder.Entity<RegistrationEvent>(entity =>
        {
            entity.HasKey(e => e.RegistrationEventId).HasName("pk_RegistrationEvent");

            entity.ToTable("RegistrationEvent");

            entity.Property(e => e.EventId)
                .HasMaxLength(6)
                .IsFixedLength();

            entity.HasOne(d => d.Event).WithMany(p => p.RegistrationEvents)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Event__5EBF139D");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationEvents)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Regis__5DCAEF64");
        });

        modelBuilder.Entity<RegistrationStatus>(entity =>
        {
            entity.HasKey(e => e.RegistrationStatusId).HasName("pk_RegistrationStatus");

            entity.ToTable("RegistrationStatus");

            entity.Property(e => e.RegistrationStatusId).ValueGeneratedOnAdd();
            entity.Property(e => e.RegistrationStatus1)
                .HasMaxLength(80)
                .HasColumnName("RegistrationStatus");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("pk_Role");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Runner>(entity =>
        {
            entity.HasKey(e => e.RunnerId).HasName("pk_Runner");

            entity.ToTable("Runner");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Runners)
                .HasForeignKey(d => d.CountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Runner__CountryC__5441852A");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Runners)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Runner__Email__52593CB8");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Runners)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Runner__Gender__534D60F1");
        });

        modelBuilder.Entity<Sponsorship>(entity =>
        {
            entity.HasKey(e => e.SponsorshipId).HasName("pk_Sponsorship");

            entity.ToTable("Sponsorship");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SponsorName).HasMaxLength(150);

            entity.HasOne(d => d.Registration).WithMany(p => p.Sponsorships)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sponsorsh__Regis__571DF1D5");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB1760EACADB");

            entity.Property(e => e.StaffId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Firstname).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Lastname).HasMaxLength(50);

            entity.HasOne(d => d.Position).WithMany(p => p.Staff)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK_Staff_ToStaffPosition");
        });

        modelBuilder.Entity<StaffPosition>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__tmp_ms_x__60BB9A799DB35E6F");

            entity.ToTable("StaffPosition");

            entity.Property(e => e.PositionId).ValueGeneratedNever();
            entity.Property(e => e.PayPeriod).HasMaxLength(50);
            entity.Property(e => e.PayRate).HasMaxLength(50);
            entity.Property(e => e.PositionDescription).HasMaxLength(255);
            entity.Property(e => e.PositionName).HasMaxLength(100);
        });

        modelBuilder.Entity<StaffTimesheet>(entity =>
        {
            entity.HasKey(e => e.TimesheetId).HasName("PK__tmp_ms_x__848CBE2D9B7D6E1D");

            entity.ToTable("StaffTimesheet");

            entity.Property(e => e.TimesheetId).ValueGeneratedNever();
            entity.Property(e => e.EndDateTime).HasColumnType("datetime");
            entity.Property(e => e.PayAmount).HasMaxLength(50);
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffTimesheets)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffTimesheet_ToStaff");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("pk_User");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(80);
            entity.Property(e => e.LastName).HasMaxLength(80);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RoleId)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleId__5165187F");
        });

        modelBuilder.Entity<Volunteer>(entity =>
        {
            entity.HasKey(e => e.VolunteerId).HasName("pk_Volunteer");

            entity.ToTable("Volunteer");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.FirstName).HasMaxLength(80);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(80);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Volunteers)
                .HasForeignKey(d => d.CountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Volunteer__Count__5629CD9C");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Volunteers)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Volunteer__Gende__5535A963");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
