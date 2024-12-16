using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipHostWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HostingDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "ContractId", "EquipmentCode", "FacilityCode", "Quantity" },
                values: new object[] { 1, "E002", "F001", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "ContractId",
                keyValue: 1);
        }
    }
}
