using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace jobApplicationTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class contractTypesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "ContractTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("156ff3cc-fd57-4f3d-80d9-7607e8aca482"), "Internship" },
                    { new Guid("2d678970-09bb-47c1-9d1a-a3cd9cb307b1"), "Freelance" },
                    { new Guid("3053a70e-9408-41ba-9732-1c6ffab93a2e"), "Part-Time" },
                    { new Guid("48f7283f-5629-4039-a492-4613321810dd"), "Full-Time" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0c54c354-4958-480a-a757-3d6cd214bc7f"), "Interviewing" },
                    { new Guid("0d05157a-0519-4034-9716-316fe203af3a"), "Rejected" },
                    { new Guid("17be4434-0cc8-48dc-ba2a-deadcc97f814"), "Wishlist" },
                    { new Guid("355bce13-f344-49f5-b198-b049751a6fc8"), "Applied" },
                    { new Guid("7979b18c-4eeb-4b29-9d33-89b996c431b1"), "Offer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("156ff3cc-fd57-4f3d-80d9-7607e8aca482"));

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("2d678970-09bb-47c1-9d1a-a3cd9cb307b1"));

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("3053a70e-9408-41ba-9732-1c6ffab93a2e"));

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: new Guid("48f7283f-5629-4039-a492-4613321810dd"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("0c54c354-4958-480a-a757-3d6cd214bc7f"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("0d05157a-0519-4034-9716-316fe203af3a"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("17be4434-0cc8-48dc-ba2a-deadcc97f814"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("355bce13-f344-49f5-b198-b049751a6fc8"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("7979b18c-4eeb-4b29-9d33-89b996c431b1"));

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
    }
}
