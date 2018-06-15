﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PMS.APP.Context;
using System;

namespace PMS.APP.Migrations
{
    [DbContext(typeof(PMSContext))]
    [Migration("20180314091910_DB")]
    partial class DB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PMS.APP.Models.Milestones", b =>
                {
                    b.Property<int>("MilestoneId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<bool>("IsCompleted");

                    b.Property<string>("Name");

                    b.Property<bool>("PaymentReceived");

                    b.Property<int>("ProjectId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("MilestoneId");

                    b.ToTable("Milestones");
                });

            modelBuilder.Entity("PMS.APP.Models.Projects", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Client");

                    b.Property<DateTime>("DateAssigned");

                    b.Property<DateTime>("DateCompleted");

                    b.Property<string>("Description");

                    b.Property<decimal>("EstimatedCost");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsCompleted");

                    b.Property<string>("Name");

                    b.Property<bool>("PaymentReceived");

                    b.Property<string>("ProjectType");

                    b.Property<string>("TechnologyStack");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("PMS.APP.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.Property<string>("UserType");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
