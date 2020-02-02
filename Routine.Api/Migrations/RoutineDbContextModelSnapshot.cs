﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Routine.Api.Data;

namespace Routine.Api.Migrations
{
    [DbContext(typeof(RoutineDbContext))]
    partial class RoutineDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("Routine.Api.Entities.Company", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Introduction")
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            ID = new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                            Introduction = "Great Company",
                            Name = "Microsoft"
                        },
                        new
                        {
                            ID = new Guid("03d39713-c42a-41e8-8529-8ab22c843d09"),
                            Introduction = "Don't be evil",
                            Name = "Google"
                        },
                        new
                        {
                            ID = new Guid("d36fcf89-99a3-44ef-ad2a-9af87dba3134"),
                            Introduction = "Fubao Company",
                            Name = "Alipapa"
                        });
                });

            modelBuilder.Entity("Routine.Api.Entities.Employee", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeeNo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            ID = new Guid("89b477db-3546-4cb0-9c6f-3a5d5fb16443"),
                            CompanyId = new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                            DateOfBirth = new DateTime(1975, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "MSFT222",
                            FirstName = "Log",
                            Gender = 1,
                            LastName = "Hill"
                        },
                        new
                        {
                            ID = new Guid("259e7699-3b1c-41b2-94d6-35f696a06b91"),
                            CompanyId = new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                            DateOfBirth = new DateTime(1975, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "MSFT223",
                            FirstName = "Li",
                            Gender = 2,
                            LastName = "Emily"
                        },
                        new
                        {
                            ID = new Guid("b28ce9be-8e2d-4bdd-99c9-22f69be1ef66"),
                            CompanyId = new Guid("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                            DateOfBirth = new DateTime(2002, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "MSFT224",
                            FirstName = "Log",
                            Gender = 1,
                            LastName = "Simon"
                        },
                        new
                        {
                            ID = new Guid("7a6578a8-f97d-4d58-8568-b4456c529e8f"),
                            CompanyId = new Guid("03d39713-c42a-41e8-8529-8ab22c843d09"),
                            DateOfBirth = new DateTime(399, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "Go224",
                            FirstName = "李",
                            Gender = 1,
                            LastName = "白"
                        },
                        new
                        {
                            ID = new Guid("97123943-68eb-4613-ab9c-7a8469beeab4"),
                            CompanyId = new Guid("03d39713-c42a-41e8-8529-8ab22c843d09"),
                            DateOfBirth = new DateTime(409, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "Go225",
                            FirstName = "杜",
                            Gender = 1,
                            LastName = "甫"
                        },
                        new
                        {
                            ID = new Guid("722b8517-87ae-4f88-b435-3acfa22183c1"),
                            CompanyId = new Guid("d36fcf89-99a3-44ef-ad2a-9af87dba3134"),
                            DateOfBirth = new DateTime(183, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "Albb225",
                            FirstName = "曹",
                            Gender = 1,
                            LastName = "操"
                        },
                        new
                        {
                            ID = new Guid("901f2404-15c4-494e-8167-66df3aaee144"),
                            CompanyId = new Guid("d36fcf89-99a3-44ef-ad2a-9af87dba3134"),
                            DateOfBirth = new DateTime(193, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "Albb226",
                            FirstName = "孙",
                            Gender = 2,
                            LastName = "尚香"
                        });
                });

            modelBuilder.Entity("Routine.Api.Entities.Employee", b =>
                {
                    b.HasOne("Routine.Api.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
