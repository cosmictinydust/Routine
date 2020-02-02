using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class AddEmployeeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: new Guid("259e7699-3b1c-41b2-94d6-35f696a06b91"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: new Guid("722b8517-87ae-4f88-b435-3acfa22183c1"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: new Guid("7a6578a8-f97d-4d58-8568-b4456c529e8f"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: new Guid("89b477db-3546-4cb0-9c6f-3a5d5fb16443"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: new Guid("901f2404-15c4-494e-8167-66df3aaee144"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: new Guid("97123943-68eb-4613-ab9c-7a8469beeab4"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: new Guid("b28ce9be-8e2d-4bdd-99c9-22f69be1ef66"));
        }
    }
}
