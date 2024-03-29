﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Storage;

namespace Storage.Migrations
{
    [DbContext(typeof(StorageContext))]
    [Migration("20210614091001_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Storage.Entities.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CardDeckForeignKey")
                        .HasColumnType("uuid");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<int>("ShuffledPosition")
                        .HasColumnType("integer");

                    b.Property<int>("Suit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CardDeckForeignKey");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Storage.Entities.CardDeck", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsShuffled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CardDeck");
                });

            modelBuilder.Entity("Storage.Entities.Card", b =>
                {
                    b.HasOne("Storage.Entities.CardDeck", "CardDeck")
                        .WithMany()
                        .HasForeignKey("CardDeckForeignKey");

                    b.Navigation("CardDeck");
                });
#pragma warning restore 612, 618
        }
    }
}
