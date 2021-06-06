﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrchestratR.ServerManager.Persistence;

namespace OrchestratR.ServerManager.Persistence.MsSql.Migrations
{
    [DbContext(typeof(OrchestratorDbContext))]
    [Migration("20210606191202_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("orchestrator")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OrchestratR.ServerManager.Persistence.Entities.OrchestratedJob", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Argument")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("HeartBeatTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("ModifyAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ServerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("OrchestratR.ServerManager.Persistence.Entities.Server", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MaxWorkersCount")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ModifyAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("OrchestratR.ServerManager.Persistence.Entities.OrchestratedJob", b =>
                {
                    b.HasOne("OrchestratR.ServerManager.Persistence.Entities.Server", "Server")
                        .WithMany("OrchestratedJobs")
                        .HasForeignKey("ServerId");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("OrchestratR.ServerManager.Persistence.Entities.Server", b =>
                {
                    b.Navigation("OrchestratedJobs");
                });
#pragma warning restore 612, 618
        }
    }
}
