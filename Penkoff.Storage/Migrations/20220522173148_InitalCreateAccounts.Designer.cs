﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Penkoff.Storage;

#nullable disable

namespace Penkoff.Storage.Migrations
{
    [DbContext(typeof(UsersContext))]
    [Migration("20220522173148_InitalCreateAccounts")]
    partial class InitalCreateAccounts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Penkoff.Storage.Entities.DollarAccount", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.ToTable("DollarAccounts");
                });

            modelBuilder.Entity("Penkoff.Storage.Entities.EuroAccount", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.ToTable("EuroAccounts");
                });

            modelBuilder.Entity("Penkoff.Storage.Entities.RubleAccount", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.ToTable("RubleAccounts");
                });

            modelBuilder.Entity("Penkoff.Storage.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Penkoff.Storage.Entities.DollarAccount", b =>
                {
                    b.HasOne("Penkoff.Storage.Entities.User", "User")
                        .WithOne("DollarAccount")
                        .HasForeignKey("Penkoff.Storage.Entities.DollarAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Penkoff.Storage.Entities.EuroAccount", b =>
                {
                    b.HasOne("Penkoff.Storage.Entities.User", "User")
                        .WithOne("EuroAccount")
                        .HasForeignKey("Penkoff.Storage.Entities.EuroAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Penkoff.Storage.Entities.RubleAccount", b =>
                {
                    b.HasOne("Penkoff.Storage.Entities.User", "User")
                        .WithOne("RubleAccount")
                        .HasForeignKey("Penkoff.Storage.Entities.RubleAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Penkoff.Storage.Entities.User", b =>
                {
                    b.Navigation("DollarAccount")
                        .IsRequired();

                    b.Navigation("EuroAccount")
                        .IsRequired();

                    b.Navigation("RubleAccount")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
