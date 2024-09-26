﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TD1_code.Models.EntityFramework;

#nullable disable

namespace TD1_code.Migrations
{
    [DbContext(typeof(DBContexte))]
    [Migration("20240925201610_CrationDBContexteProduit")]
    partial class CrationDBContexteProduit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TD1_code.Models.EntityFramework.Marque", b =>
                {
                    b.Property<int>("IdMarque")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idMarque");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdMarque"));

                    b.Property<string>("NomMarque")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nomMarque");

                    b.HasKey("IdMarque")
                        .HasName("pk_produit2");

                    b.ToTable("Marque");
                });

            modelBuilder.Entity("TD1_code.Models.EntityFramework.Produit", b =>
                {
                    b.Property<int>("IdMarque")
                        .HasColumnType("integer")
                        .HasColumnName("idMarque");

                    b.Property<int>("IdTypeProduit")
                        .HasColumnType("integer")
                        .HasColumnName("idTypeProduit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("IdProduit")
                        .HasColumnType("integer")
                        .HasColumnName("IdProduit");

                    b.Property<string>("NomPhoto")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nomPhoto");

                    b.Property<string>("NomProduit")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nomProduit");

                    b.Property<int>("StockMax")
                        .HasColumnType("integer")
                        .HasColumnName("stockMax");

                    b.Property<int>("StockMin")
                        .HasColumnType("integer")
                        .HasColumnName("stockMin");

                    b.Property<int>("StockReel")
                        .HasColumnType("integer")
                        .HasColumnName("stockReel");

                    b.Property<string>("UriPhoto")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("uriPhoto");

                    b.HasKey("IdMarque", "IdTypeProduit")
                        .HasName("pk_produit");

                    b.HasIndex("IdTypeProduit");

                    b.ToTable("Produit");
                });

            modelBuilder.Entity("TD1_code.Models.EntityFramework.TypeProduit", b =>
                {
                    b.Property<int>("IdTypeProduit")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idTypeProduit");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdTypeProduit"));

                    b.Property<string>("NomTypeProduit")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nomTypeProduit");

                    b.HasKey("IdTypeProduit")
                        .HasName("pk_typeProduit");

                    b.ToTable("TypeProduit");
                });

            modelBuilder.Entity("TD1_code.Models.EntityFramework.Produit", b =>
                {
                    b.HasOne("TD1_code.Models.EntityFramework.Marque", "IdMarqueNavigation")
                        .WithMany("Produits")
                        .HasForeignKey("IdMarque")
                        .IsRequired()
                        .HasConstraintName("fk_produit_marque");

                    b.HasOne("TD1_code.Models.EntityFramework.TypeProduit", "IdTypeProduitNavigation")
                        .WithMany("Produits")
                        .HasForeignKey("IdTypeProduit")
                        .IsRequired()
                        .HasConstraintName("fk_produit_typeProduit");

                    b.Navigation("IdMarqueNavigation");

                    b.Navigation("IdTypeProduitNavigation");
                });

            modelBuilder.Entity("TD1_code.Models.EntityFramework.Marque", b =>
                {
                    b.Navigation("Produits");
                });

            modelBuilder.Entity("TD1_code.Models.EntityFramework.TypeProduit", b =>
                {
                    b.Navigation("Produits");
                });
#pragma warning restore 612, 618
        }
    }
}