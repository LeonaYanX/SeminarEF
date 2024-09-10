﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SeminarEF.Db.Models;

#nullable disable

namespace SeminarEF.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    [Migration("20240910081417_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SeminarEF.Db.Models.Messages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FromUserId")
                        .HasColumnType("integer")
                        .HasColumnName("from_user_id");

                    b.Property<bool>("IsReceived")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<int?>("ToUserId")
                        .HasColumnType("integer")
                        .HasColumnName("to_user_id");

                    b.HasKey("Id")
                        .HasName("messages_pkey");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("messages", (string)null);
                });

            modelBuilder.Entity("SeminarEF.Db.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("users_pkey");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("SeminarEF.Db.Models.Messages", b =>
                {
                    b.HasOne("SeminarEF.Db.Models.Users", "FromUser")
                        .WithMany("FromMessages")
                        .HasForeignKey("FromUserId")
                        .HasConstraintName("messages_from_user_id_fkey");

                    b.HasOne("SeminarEF.Db.Models.Users", "ToUser")
                        .WithMany("ToMessages")
                        .HasForeignKey("ToUserId");

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("SeminarEF.Db.Models.Users", b =>
                {
                    b.Navigation("FromMessages");

                    b.Navigation("ToMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
