using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AirlineManagement.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airplanes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airplanes", x => x.Id);
                    table.CheckConstraint("CK_Airplanes_Capacity", "\"Capacity\" > 0");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Post = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightNumber = table.Column<string>(type: "text", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightExecutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    AirplaneId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    OccupiedSeats = table.Column<int>(type: "integer", nullable: false),
                    FlightDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightExecutions", x => x.Id);
                    table.CheckConstraint("CK_FlightExecutions_OccupiedSeats", "\"OccupiedSeats\" >= 0");
                    table.ForeignKey(
                        name: "FK_FlightExecutions_Airplanes_AirplaneId",
                        column: x => x.AirplaneId,
                        principalTable: "Airplanes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightExecutions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightExecutions_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketSaleItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    TicketsSold = table.Column<int>(type: "integer", nullable: false),
                    PricePerTicket = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSaleItems", x => x.Id);
                    table.CheckConstraint("CK_TicketSaleItems_PricePerTicket", "\"PricePerTicket\" >= 0");
                    table.CheckConstraint("CK_TicketSaleItems_TicketsSold", "\"TicketsSold\" >= 0");
                    table.ForeignKey(
                        name: "FK_TicketSaleItems_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketSaleItems_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlightExecutionCrewMembers",
                columns: table => new
                {
                    FlightExecutionId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightExecutionCrewMembers", x => new { x.FlightExecutionId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_FlightExecutionCrewMembers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightExecutionCrewMembers_FlightExecutions_FlightExecution~",
                        column: x => x.FlightExecutionId,
                        principalTable: "FlightExecutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightExecutionCrewMembers_EmployeeId",
                table: "FlightExecutionCrewMembers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightExecutions_AirplaneId",
                table: "FlightExecutions",
                column: "AirplaneId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightExecutions_EmployeeId",
                table: "FlightExecutions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightExecutions_FlightId",
                table: "FlightExecutions",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSaleItems_EmployeeId",
                table: "TicketSaleItems",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSaleItems_FlightId",
                table: "TicketSaleItems",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightExecutionCrewMembers");

            migrationBuilder.DropTable(
                name: "TicketSaleItems");

            migrationBuilder.DropTable(
                name: "FlightExecutions");

            migrationBuilder.DropTable(
                name: "Airplanes");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
