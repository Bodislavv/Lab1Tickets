using System;
using System.Collections.Generic;
using TicketsDomain.Model;
using Microsoft.EntityFrameworkCore;

//namespace TicketsDomain.Model;
namespace TicketsInfrastructure;

public partial class DbticketsContext : DbContext
{
    public DbticketsContext()
    {
    }

    public DbticketsContext(DbContextOptions<DbticketsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Bohdan\\SQLEXPRESS; Database=DBTickets; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.TeamANavigation).WithMany(p => p.GameTeamANavigations)
                .HasForeignKey(d => d.TeamA)
                .HasConstraintName("FK_Games_TeamA");

            entity.HasOne(d => d.TeamBNavigation).WithMany(p => p.GameTeamBNavigations)
                .HasForeignKey(d => d.TeamB)
                .HasConstraintName("FK_Games_TeamB");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Games)
                .HasForeignKey(d => d.TournamentId)
                .HasConstraintName("FK_Games_Tournaments");

            entity.HasOne(d => d.Venue).WithMany(p => p.Games)
                .HasForeignKey(d => d.VenueId)
                .HasConstraintName("FK_Games_Venues");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Teams_Countries");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Seat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Customner).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CustomnerId)
                .HasConstraintName("FK_Tickets_Customers");

            entity.HasOne(d => d.Game).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Tickets_Games");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Venues)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Venues_Countries");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
