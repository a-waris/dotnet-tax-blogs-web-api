﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Taxbox.Infrastructure.Context;

#nullable disable

namespace Taxbox.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Taxbox.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(254)");
                    
                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(254)");
                    
                    b.Property<string>("DisplayPictureUrl")
                        .HasColumnType("nvarchar(2048)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("687d9fd5-2752-4a96-93d5-0f33a49913c6"),
                            Email = "admin@taxbox.com",
                            Password = "$2a$11$oNUUOd3qRUAGD55pG/VBR.p3kn4kgdk5qJ6ArL5UybYMI0kFOlX4i",
                            Role = "Admin",
                            FirstName = "Admin",
                            LastName = "User",
                            DisplayPictureUrl = ""
                        },
                        new
                        {
                            Id = new Guid("6648c89f-e894-42bb-94f0-8fd1059c86b4"),
                            Email = "user@taxbox.com",
                            Password = "$2a$11$COfh6eYz/zaenoTtBexF7ueQmxbUo5PJJPdyR/HYoqDmolhWpZ3ui",
                            Role = "User",
                            FirstName = "Normal",
                            LastName = "User",
                            DisplayPictureUrl = ""
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
