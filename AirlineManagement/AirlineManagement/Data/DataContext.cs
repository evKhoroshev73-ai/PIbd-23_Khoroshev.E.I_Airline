using AirlineManagement.Entities;
using AirlineManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AirlineManagement.Data;

public sealed class DataContext(IConnectionString connectionString) : DbContext
{
    public DbSet<Airplane> Airplanes => Set<Airplane>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<FlightExecution> FlightExecutions => Set<FlightExecution>();
    public DbSet<TicketSaleItem> TicketSaleItems => Set<TicketSaleItem>();
    public DbSet<FlightExecutionCrewMember> FlightExecutionCrewMembers => Set<FlightExecutionCrewMember>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString.Value);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airplane>().ToTable("Airplanes", table =>
            table.HasCheckConstraint("CK_Airplanes_Capacity", "\"Capacity\" > 0"));

        modelBuilder.Entity<FlightExecution>(entity =>
        {
            entity.ToTable("FlightExecutions", table =>
                table.HasCheckConstraint("CK_FlightExecutions_OccupiedSeats", "\"OccupiedSeats\" >= 0"));
            entity.HasOne<Flight>().WithMany().HasForeignKey(e => e.FlightId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Airplane>().WithMany().HasForeignKey(e => e.AirplaneId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Employee>().WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.CrewMembers).WithOne(e => e.FlightExecution)
                .HasForeignKey(e => e.FlightExecutionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FlightExecutionCrewMember>(entity =>
        {
            entity.HasKey(e => new { e.FlightExecutionId, e.EmployeeId });
            entity.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TicketSaleItem>(entity =>
        {
            entity.ToTable("TicketSaleItems", table =>
            {
                table.HasCheckConstraint("CK_TicketSaleItems_TicketsSold", "\"TicketsSold\" >= 0");
                table.HasCheckConstraint("CK_TicketSaleItems_PricePerTicket", "\"PricePerTicket\" >= 0");
            });
            entity.HasOne<Flight>().WithMany().HasForeignKey(e => e.FlightId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Employee>().WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
