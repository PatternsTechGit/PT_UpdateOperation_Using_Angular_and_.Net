using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class firstMigration1 : Migration
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
                values: new object[] { "37846734-172e-4149-8cec-6f43d1eb3f60", "0001-1001", 0, "Raas Masood", 3500m, "aa45e3c9-261d-41fe-a1b0-5b4dcf79cfd3" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "TransactionAmount", "TransactionDate", "TransactionType" },
                values: new object[,]
                {
                    { "21819840-2ab4-4cb3-9c6a-321e29c01830", "37846734-172e-4149-8cec-6f43d1eb3f60", 200m, new DateTime(2021, 10, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6581), 0 },
                    { "2f06b197-96d7-4053-8c11-ca71da254e0b", "37846734-172e-4149-8cec-6f43d1eb3f60", -100m, new DateTime(2021, 11, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6578), 1 },
                    { "4ea695fb-b9f0-4a8c-8967-35da91acc698", "37846734-172e-4149-8cec-6f43d1eb3f60", 900m, new DateTime(2021, 8, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6587), 0 },
                    { "6b773870-f0b5-4e4e-9bae-0f8f41f497fc", "37846734-172e-4149-8cec-6f43d1eb3f60", -200m, new DateTime(2022, 3, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6565), 1 },
                    { "764f91f4-8f9b-4a1a-b3e5-c242b4a4a643", "37846734-172e-4149-8cec-6f43d1eb3f60", -500m, new DateTime(2021, 9, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6584), 1 },
                    { "7c3e5c65-243f-4f31-b8cb-e9e834bf603b", "37846734-172e-4149-8cec-6f43d1eb3f60", 500m, new DateTime(2022, 4, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6541), 0 },
                    { "8282a44c-d6f0-4893-bdaa-81ef63a4ca78", "37846734-172e-4149-8cec-6f43d1eb3f60", -500m, new DateTime(2021, 7, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6531), 1 },
                    { "9078f93c-ab10-4e6b-b7ed-c16e5c033bb2", "37846734-172e-4149-8cec-6f43d1eb3f60", 200m, new DateTime(2022, 1, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6572), 0 },
                    { "92851ceb-736b-4c63-95fe-77a68c37b089", "37846734-172e-4149-8cec-6f43d1eb3f60", 500m, new DateTime(2022, 2, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6568), 0 },
                    { "a0a5bcab-2b30-4c14-9907-836889c9ac6b", "37846734-172e-4149-8cec-6f43d1eb3f60", 3000m, new DateTime(2022, 7, 3, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6447), 0 },
                    { "b7322753-cf92-4500-97d3-80fd272cde8b", "37846734-172e-4149-8cec-6f43d1eb3f60", -300m, new DateTime(2021, 12, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6575), 1 },
                    { "eb9f60b2-1125-4a5e-a9e6-22bbe1754986", "37846734-172e-4149-8cec-6f43d1eb3f60", 1000m, new DateTime(2020, 7, 4, 21, 55, 33, 792, DateTimeKind.Local).AddTicks(6537), 0 }
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
