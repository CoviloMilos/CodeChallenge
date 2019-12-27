﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Task.Api.Repositories;

namespace Task.Api.Migrations
{
    [DbContext(typeof(TaskDataContext))]
    partial class TaskDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Task.Api.Models.Case", b =>
                {
                    b.Property<int>("CaseId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CaseNum")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("nextval('seq_case_casenum')");

                    b.Property<string>("FirstInputParameter")
                        .IsRequired();

                    b.Property<string>("SecondInputParameter");

                    b.Property<bool>("SecretTest");

                    b.Property<Guid>("TaskGuid");

                    b.Property<string>("ValidReturnValue")
                        .IsRequired();

                    b.HasKey("CaseId");

                    b.HasIndex("TaskGuid");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("Task.Api.Models.TaskModel", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Explanation");

                    b.Property<string>("FirstInputParameterDataType")
                        .IsRequired();

                    b.Property<bool>("IsProdcution");

                    b.Property<string>("MethodName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ReturnDataType")
                        .IsRequired();

                    b.Property<string>("SecondInputParameterDataType");

                    b.Property<Guid>("TaskGuid");

                    b.Property<int>("TaskNum")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("nextval('seq_task_tasknum')");

                    b.HasKey("TaskId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Task.Api.Models.Case", b =>
                {
                    b.HasOne("Task.Api.Models.TaskModel", "Task")
                        .WithMany("Cases")
                        .HasForeignKey("TaskGuid")
                        .HasPrincipalKey("TaskGuid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
