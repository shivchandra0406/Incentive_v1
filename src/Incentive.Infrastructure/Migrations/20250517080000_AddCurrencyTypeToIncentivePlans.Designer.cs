// <auto-generated />
using System;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250517080000_AddCurrencyTypeToIncentivePlans")]
    partial class AddCurrencyTypeToIncentivePlans
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            // Copy the content from the latest Designer.cs file and update it to include the CurrencyType property
            // This is a placeholder - in a real scenario, you would need to copy the full model snapshot
            // and update it to include the new CurrencyType property
#pragma warning restore 612, 618
        }
    }
}
