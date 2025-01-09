using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace jobApplicationTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSeededStatusesAndContractTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Statuses");

            migrationBuilder.InsertData(
                table: "ContractTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4c5111e5-2d5b-4dda-b9e9-ff315f8e30e9"), "Part-Time" },
                    { new Guid("a6faf712-a40a-4c49-bc7c-2d069ac1f136"), "Full-Time (Employment Contract)" },
                    { new Guid("bf7f575e-8917-4a63-8eb9-cced30249d4c"), "Freelance" },
                    { new Guid("c9456204-9005-411c-af7a-f7bbee972229"), "Commission Contract" },
                    { new Guid("d707a21a-5c36-4bdc-b78c-420705fa948e"), "Internship" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1e6765d7-b23a-41a6-a488-58dd73ad553b"), "Wishlist" },
                    { new Guid("4bd007bc-1632-40ed-a990-2e670ab5ddc1"), "Applied" },
                    { new Guid("6f857884-154b-44df-aff4-355c2fe93c54"), "Interviewing" },
                    { new Guid("c0634a52-de6a-4c70-b701-dc7e96902314"), "Offer" },
                    { new Guid("efa5e3c7-e26d-45c0-a79d-8fc32cddfe86"), "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("4c5111e5-2d5b-4dda-b9e9-ff315f8e30e9"));

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("a6faf712-a40a-4c49-bc7c-2d069ac1f136"));

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("bf7f575e-8917-4a63-8eb9-cced30249d4c"));

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("c9456204-9005-411c-af7a-f7bbee972229"));

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("d707a21a-5c36-4bdc-b78c-420705fa948e"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("1e6765d7-b23a-41a6-a488-58dd73ad553b"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("4bd007bc-1632-40ed-a990-2e670ab5ddc1"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("6f857884-154b-44df-aff4-355c2fe93c54"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("c0634a52-de6a-4c70-b701-dc7e96902314"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("efa5e3c7-e26d-45c0-a79d-8fc32cddfe86"));

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Statuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
