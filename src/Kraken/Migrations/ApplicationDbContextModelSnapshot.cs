using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Kraken.Models;

namespace Kraken.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("Relational:DefaultSchema", "kraken")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kraken.Models.ApplicationUser", b =>
                {
                    b.Property<string>("UserName");

                    b.Property<string>("OctopusApiKey");

                    b.HasKey("UserName");
                });

            modelBuilder.Entity("Kraken.Models.ProjectBatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Kraken.Models.ProjectBatchItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LatestDeploymentId")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("LatestReleaseId")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("LatestTaskId")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("NugetPackageId")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int>("ProjectBatchId");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("ProjectVersion")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Kraken.Models.ProjectBatchItem", b =>
                {
                    b.HasOne("Kraken.Models.ProjectBatch")
                        .WithMany()
                        .HasForeignKey("ProjectBatchId");
                });
        }
    }
}
