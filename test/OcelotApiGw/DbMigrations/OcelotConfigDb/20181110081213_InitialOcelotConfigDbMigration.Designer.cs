﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ocelot.Provider.SqlServer;

namespace OcelotApiGw.DbMigrations.OcelotConfigDb
{
    [DbContext(typeof(OcelotConfigDbContext))]
    [Migration("20181110081213_InitialOcelotConfigDbMigration")]
    partial class InitialOcelotConfigDbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ocelot.Provider.SqlServer.OcelotConfigurationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Payload");

                    b.Property<string>("Section");

                    b.HasKey("Id");

                    b.ToTable("Ocelot_Configs");
                });
#pragma warning restore 612, 618
        }
    }
}