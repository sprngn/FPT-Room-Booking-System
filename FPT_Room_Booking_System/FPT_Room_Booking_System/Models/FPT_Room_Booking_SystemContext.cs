using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace FPT_Room_Booking_System.Models
{
    public partial class FPT_Room_Booking_SystemContext : DbContext
    {
        public FPT_Room_Booking_SystemContext()
        {
        }

        public FPT_Room_Booking_SystemContext(DbContextOptions<FPT_Room_Booking_SystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<RoomType> RoomTypes { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("date")
                    .HasColumnName("booking_date");

                entity.Property(e => e.BookingUser).HasColumnName("booking_user");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.Slot).HasColumnName("slot");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.HasOne(d => d.BookingUserNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_User");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Room");

                entity.HasOne(d => d.SlotNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.Slot)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Slot_To");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(100)
                    .HasColumnName("department_name");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasIndex(e => e.RoleName, "UQ__role__783254B1B1F8A098")
                    .IsUnique();

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("room");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RoomName)
                    .HasMaxLength(100)
                    .HasColumnName("room_name");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Department");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Type");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__room_typ__2C0005981D41199F");

                entity.ToTable("room_type");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Projector).HasColumnName("projector");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(100)
                    .HasColumnName("type_name");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("slot");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.SlotName).HasColumnName("slot_name");

                entity.Property(e => e.StartTime).HasColumnName("start_time");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "UQ__users__AB6E61645E64657D")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.EmailKey)
                    .HasMaxLength(255)
                    .HasColumnName("email_key");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
