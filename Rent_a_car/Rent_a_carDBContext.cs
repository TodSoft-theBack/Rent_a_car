using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Rent_a_car.Entities;

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
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Rent_a_carDB;Integrated Security=True;Trusted_Connection=True;");
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

                entity.Property(e => e.Picture)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PricePerDay).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasMaxLength(4);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.DateOfReservation).HasColumnType("datetime");

                entity.Property(e => e.DropOffLocationId).HasColumnName("DropOffLocationID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.PickUpLocationId).HasColumnName("PickUpLocationID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__CarID__2F10007B");

                entity.HasOne(d => d.DropOffLocation)
                    .WithMany(p => p.ReservationsDropOffLocation)
                    .HasForeignKey(d => d.DropOffLocationId)
                    .HasConstraintName("FK__Reservati__DropO__31EC6D26");

                entity.HasOne(d => d.PickUpLocation)
                    .WithMany(p => p.ReservationsPickUpLocation)
                    .HasForeignKey(d => d.PickUpLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__PickU__30F848ED");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__UserI__300424B4");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Egn)
                    .HasName("UQ__Users__C1902746F0F4A62E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Egn)
                    .IsRequired()
                    .HasColumnName("EGN")
                    .HasMaxLength(10);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
