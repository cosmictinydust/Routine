using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class initialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Country = table.Column<string>(maxLength: 50, nullable: true),
                    Industry = table.Column<string>(maxLength: 50, nullable: true),
                    Product = table.Column<string>(maxLength: 100, nullable: true),
                    Introduction = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    EmployeeNo = table.Column<string>(maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "ID", "Country", "Industry", "Introduction", "Name", "Product" },
                values: new object[] { new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"), "USA", "Software", "Great Company", "Microsoft", "Windows,Office" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "ID", "Country", "Industry", "Introduction", "Name", "Product" },
                values: new object[] { new Guid("03d39713-c42a-41e8-8529-8ab22c843d09"), "USA", "Internet", "Don't be evil", "Google", "Andoird,Google Search Engine" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "ID", "Country", "Industry", "Introduction", "Name", "Product" },
                values: new object[] { new Guid("d36fcf89-99a3-44ef-ad2a-9af87dba3134"), "China", "Internet", "Fubao Company", "Alipapa", "Tabao,Alibaba" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("89b477db-3546-4cb0-9c6f-3a5d5fb16443"), new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"), new DateTime(1975, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT222", "Log", 1, "Hill" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("259e7699-3b1c-41b2-94d6-35f696a06b91"), new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"), new DateTime(1975, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT223", "Li", 2, "Emily" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("b28ce9be-8e2d-4bdd-99c9-22f69be1ef66"), new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"), new DateTime(2002, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT224", "Log", 1, "Simon" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("7a6578a8-f97d-4d58-8568-b4456c529e8f"), new Guid("03d39713-c42a-41e8-8529-8ab22c843d09"), new DateTime(399, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Go224", "李", 1, "白" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("97123943-68eb-4613-ab9c-7a8469beeab4"), new Guid("03d39713-c42a-41e8-8529-8ab22c843d09"), new DateTime(409, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Go225", "杜", 1, "甫" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("722b8517-87ae-4f88-b435-3acfa22183c1"), new Guid("d36fcf89-99a3-44ef-ad2a-9af87dba3134"), new DateTime(183, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Albb225", "曹", 1, "操" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("901f2404-15c4-494e-8167-66df3aaee144"), new Guid("d36fcf89-99a3-44ef-ad2a-9af87dba3134"), new DateTime(193, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Albb226", "孙", 2, "尚香" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
