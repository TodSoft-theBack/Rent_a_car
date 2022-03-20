using System;
using Rent_a_car.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Rent_a_car
{
    public partial class Rent_a_carDBContext : DbContext
    {
        public Rent_a_carDBContext()
        {
        }

        public Rent_a_carDBContext(DbContextOptions<Rent_a_carDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Rent_a_carDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cars>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PricePerDay).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasMaxLength(4);
            });

            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.DateOfReservation).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__CarID__29572725");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__UserI__2A4B4B5E");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Egn)
                    .HasName("UQ__Users__C1902746182ECBAF")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Egn)
                    .IsRequired()
                    .HasColumnName("EGN")
                    .HasMaxLength(10);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);

            this.SeedCars(modelBuilder);
            this.SeedUsers(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void SeedCars(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cars>().HasData(
                new Cars()
                {
                    Id = 1,
                    Model = "SampleModel1",
                    Brand = "SampleBrand1",
                    Year = 2000,
                    PassengersCount = 4,
                    Description = "Sample car 1",
                    PricePerDay = 100
                },
                new Cars()
                {
                    Id = 2,
                    Model = "SampleModel2",
                    Brand = "SampleBrand2",
                    Year = 1980,
                    PassengersCount = 4,
                    Description = "Sample car 2",
                    PricePerDay = 60
                },
                new Cars()
                {
                    Id = 3,
                    Model = "SampleModel3",
                    Brand = "SampleBrand3",
                    Year = 2005,
                    PassengersCount = 4,
                    Description = "Sample car 3",
                    PricePerDay = 75
                });
        }
        private void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(
                new Users()
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "123",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    Email = "admin@admin.com",
                    Egn = "035435446",
                    Phone = "066778899",
                    IsAdmin = 1
                },
                new Users()
                {
                    Id = 2,
                    UserName = "user",
                    Password = "123",
                    FirstName = "User",
                    LastName = "Userov",
                    Email = "user@user.com",
                    Egn = "034834890",
                    Phone = "099887766",
                    IsAdmin = 0
                });
        }
    }
}
