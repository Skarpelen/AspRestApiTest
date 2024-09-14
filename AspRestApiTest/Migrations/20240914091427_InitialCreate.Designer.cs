﻿// <auto-generated />
using System;
using AspRestApiTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AspRestApiTest.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240914091427_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AspRestApiTest.Data.Models.ExceptionJournal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BodyParameters")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<string>("ExceptionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("QueryParameters")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StackTrace")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ExceptionJournals");
                });

            modelBuilder.Entity("AspRestApiTest.Data.Models.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentNodeId")
                        .HasColumnType("integer");

                    b.Property<int>("TreeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentNodeId");

                    b.HasIndex("TreeId");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("AspRestApiTest.Data.Models.Tree", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Trees");
                });

            modelBuilder.Entity("AspRestApiTest.Data.Models.Node", b =>
                {
                    b.HasOne("AspRestApiTest.Data.Models.Node", "ParentNode")
                        .WithMany("ChildNodes")
                        .HasForeignKey("ParentNodeId");

                    b.HasOne("AspRestApiTest.Data.Models.Tree", "Tree")
                        .WithMany("Nodes")
                        .HasForeignKey("TreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentNode");

                    b.Navigation("Tree");
                });

            modelBuilder.Entity("AspRestApiTest.Data.Models.Node", b =>
                {
                    b.Navigation("ChildNodes");
                });

            modelBuilder.Entity("AspRestApiTest.Data.Models.Tree", b =>
                {
                    b.Navigation("Nodes");
                });
#pragma warning restore 612, 618
        }
    }
}
