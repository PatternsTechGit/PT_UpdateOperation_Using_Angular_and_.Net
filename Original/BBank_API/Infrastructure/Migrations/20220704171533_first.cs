using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "ProfilePicUrl" },
                values: new object[] { "aa45e3c9-261d-41fe-a1b0-5b4dcf79cfd3", "rassmasood@hotmail.com", "Raas", "Masood", "https://res.cloudinary.com/demo/image/upload/w_400,h_400,c_crop,g_face,r_max/w_200/lady.jpg" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountNumber", "AccountStatus", "AccountTitle", "CurrentBalance", "UserId" },
                values: new object[] { "37846734-172e-4149-8cec-6f43d1eb3f60", "0001-1001", 1, "Raas Masood", 3500m, "aa45e3c9-261d-41fe-a1b0-5b4dcf79cfd3" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "TransactionAmount", "TransactionDate", "TransactionType" },
                values: new object[,]
                {
                    { "22d77515-7223-4f67-8978-d6e2177d80b5", "37846734-172e-4149-8cec-6f43d1eb3f60", 200m, new DateTime(2021, 10, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6274), 0 },
                    { "2d1739fd-2eb8-44f7-b3d7-12aae6b73b72", "37846734-172e-4149-8cec-6f43d1eb3f60", 3000m, new DateTime(2022, 7, 3, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6213), 0 },
                    { "55d713f8-97ff-4fa5-bf7e-bf5947931e09", "37846734-172e-4149-8cec-6f43d1eb3f60", 500m, new DateTime(2022, 4, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6254), 0 },
                    { "5922ded8-fd0f-4c37-a71a-cad0297c003b", "37846734-172e-4149-8cec-6f43d1eb3f60", 200m, new DateTime(2022, 1, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6263), 0 },
                    { "76ec0851-68f3-4c02-a291-8c9fbb745b62", "37846734-172e-4149-8cec-6f43d1eb3f60", 900m, new DateTime(2021, 8, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6280), 0 },
                    { "793474d8-ccf6-4817-ba9f-16fe274a73f3", "37846734-172e-4149-8cec-6f43d1eb3f60", 500m, new DateTime(2022, 2, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6260), 0 },
                    { "845851b7-53ca-46df-8ac2-8273ad441f1d", "37846734-172e-4149-8cec-6f43d1eb3f60", -500m, new DateTime(2021, 9, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6277), 1 },
                    { "946d746d-e8e8-4d14-b3e5-2ffd97deb272", "37846734-172e-4149-8cec-6f43d1eb3f60", -200m, new DateTime(2022, 3, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6257), 1 },
                    { "b41cc7fb-53ed-490e-ac94-1e69d9d5fead", "37846734-172e-4149-8cec-6f43d1eb3f60", 1000m, new DateTime(2020, 7, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6250), 0 },
                    { "be64f3af-fcc3-4f42-8b8b-4140217ae326", "37846734-172e-4149-8cec-6f43d1eb3f60", -100m, new DateTime(2021, 11, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6272), 1 },
                    { "cd208cd1-9f0b-4ab5-8228-5e6e27d9041a", "37846734-172e-4149-8cec-6f43d1eb3f60", -500m, new DateTime(2021, 7, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6246), 1 },
                    { "f7b1eb3e-ef16-4cd2-9288-850ede4debe2", "37846734-172e-4149-8cec-6f43d1eb3f60", -300m, new DateTime(2021, 12, 4, 22, 15, 32, 924, DateTimeKind.Local).AddTicks(6266), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
