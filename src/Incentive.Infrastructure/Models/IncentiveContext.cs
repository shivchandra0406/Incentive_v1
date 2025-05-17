using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Incentive.Infrastructure.Models;

public partial class IncentiveContext : DbContext
{
    public IncentiveContext(DbContextOptions<IncentiveContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Deal> Deals { get; set; }

    public virtual DbSet<DealActivity> DealActivities { get; set; }

    public virtual DbSet<IncentiveEarning> IncentiveEarnings { get; set; }

    public virtual DbSet<IncentivePlan> IncentivePlans { get; set; }

    public virtual DbSet<IncentiveRule> IncentiveRules { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamMember> TeamMembers { get; set; }

    public virtual DbSet<TieredIncentiveTier> TieredIncentiveTiers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deal>(entity =>
        {
            entity.ToTable("Deals", "IncentiveManagement");

            entity.HasIndex(e => e.IncentiveRuleId, "IX_Deals_IncentiveRuleId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CurrencyType).HasMaxLength(50);
            entity.Property(e => e.CustomerAddress).HasMaxLength(500);
            entity.Property(e => e.CustomerEmail).HasMaxLength(100);
            entity.Property(e => e.CustomerName).HasMaxLength(200);
            entity.Property(e => e.CustomerPhone).HasMaxLength(50);
            entity.Property(e => e.DealName).HasMaxLength(200);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.PaidAmount).HasPrecision(18, 2);
            entity.Property(e => e.ReferralCommission).HasPrecision(18, 2);
            entity.Property(e => e.ReferralEmail).HasMaxLength(100);
            entity.Property(e => e.ReferralName).HasMaxLength(200);
            entity.Property(e => e.ReferralPhone).HasMaxLength(50);
            entity.Property(e => e.RemainingAmount).HasPrecision(18, 2);
            entity.Property(e => e.Source).HasMaxLength(100);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxPercentage).HasPrecision(5, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(d => d.IncentiveRule).WithMany(p => p.Deals)
                .HasForeignKey(d => d.IncentiveRuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DealActivity>(entity =>
        {
            entity.ToTable("DealActivities", "IncentiveManagement");

            entity.HasIndex(e => e.DealId, "IX_DealActivities_DealId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ActivityDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(d => d.Deal).WithMany(p => p.DealActivities).HasForeignKey(d => d.DealId);
        });

        modelBuilder.Entity<IncentiveEarning>(entity =>
        {
            entity.ToTable("IncentiveEarnings", "IncentiveManagement");

            entity.HasIndex(e => e.DealId, "IX_IncentiveEarnings_DealId");

            entity.HasIndex(e => e.IncentiveRuleId, "IX_IncentiveEarnings_IncentiveRuleId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Deal).WithMany(p => p.IncentiveEarnings)
                .HasForeignKey(d => d.DealId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IncentiveRule).WithMany(p => p.IncentiveEarnings)
                .HasForeignKey(d => d.IncentiveRuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<IncentivePlan>(entity =>
        {
            entity.ToTable("IncentivePlans", "IncentiveManagement");

            entity.HasIndex(e => e.ProjectId, "IX_IncentivePlans_ProjectId");

            entity.HasIndex(e => e.TeamId, "IX_IncentivePlans_TeamId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AwardValue).HasPrecision(18, 2);
            entity.Property(e => e.GiftDescription).HasMaxLength(500);
            entity.Property(e => e.IncentiveValue).HasPrecision(18, 2);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.PlanDiscriminator).HasMaxLength(21);
            entity.Property(e => e.PlanName).HasMaxLength(200);
            entity.Property(e => e.ProjectBasedIncentivePlanMetricType).HasColumnName("ProjectBasedIncentivePlan_MetricType");
            entity.Property(e => e.ProjectBasedIncentivePlanTargetValue)
                .HasPrecision(18, 2)
                .HasColumnName("ProjectBasedIncentivePlan_TargetValue");
            entity.Property(e => e.Role).HasMaxLength(100);
            entity.Property(e => e.RoleBasedIncentivePlanCalculationType).HasColumnName("RoleBasedIncentivePlan_CalculationType");
            entity.Property(e => e.RoleBasedIncentivePlanIncentiveAfterExceedingTarget).HasColumnName("RoleBasedIncentivePlan_IncentiveAfterExceedingTarget");
            entity.Property(e => e.RoleBasedIncentivePlanIncentiveValue)
                .HasPrecision(18, 2)
                .HasColumnName("RoleBasedIncentivePlan_IncentiveValue");
            entity.Property(e => e.RoleBasedIncentivePlanMetricType).HasColumnName("RoleBasedIncentivePlan_MetricType");
            entity.Property(e => e.RoleBasedIncentivePlanTargetValue)
                .HasPrecision(18, 2)
                .HasColumnName("RoleBasedIncentivePlan_TargetValue");
            entity.Property(e => e.Salary).HasPrecision(5, 2);
            entity.Property(e => e.SalaryPercentage).HasPrecision(5, 2);
            entity.Property(e => e.TargetBasedIncentivePlanCalculationType).HasColumnName("TargetBasedIncentivePlan_CalculationType");
            entity.Property(e => e.TargetBasedIncentivePlanIncentiveAfterExceedingTarget).HasColumnName("TargetBasedIncentivePlan_IncentiveAfterExceedingTarget");
            entity.Property(e => e.TargetBasedIncentivePlanIncentiveValue)
                .HasPrecision(18, 2)
                .HasColumnName("TargetBasedIncentivePlan_IncentiveValue");
            entity.Property(e => e.TargetBasedIncentivePlanIncludeSalaryInTarget).HasColumnName("TargetBasedIncentivePlan_IncludeSalaryInTarget");
            entity.Property(e => e.TargetBasedIncentivePlanMetricType).HasColumnName("TargetBasedIncentivePlan_MetricType");
            entity.Property(e => e.TargetBasedIncentivePlanTargetType).HasColumnName("TargetBasedIncentivePlan_TargetType");
            entity.Property(e => e.TargetBasedIncentivePlanTargetValue)
                .HasPrecision(18, 2)
                .HasColumnName("TargetBasedIncentivePlan_TargetValue");
            entity.Property(e => e.TargetValue).HasPrecision(18, 2);
            entity.Property(e => e.TieredIncentivePlanMetricType).HasColumnName("TieredIncentivePlan_MetricType");

            entity.HasOne(d => d.Project).WithMany(p => p.IncentivePlans)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Team).WithMany(p => p.IncentivePlans)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<IncentiveRule>(entity =>
        {
            entity.ToTable("IncentiveRules", "IncentiveManagement");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Commission).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsIncludeSalary).HasDefaultValue(true);
            entity.Property(e => e.MaximumIncentiveAmount).HasPrecision(18, 2);
            entity.Property(e => e.MinimumSalesThreshold).HasPrecision(18, 2);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Salary).HasPrecision(18, 2);
            entity.Property(e => e.TargetValue).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments", "IncentiveManagement");

            entity.HasIndex(e => e.DealId, "IX_Payments_DealId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionReference).HasMaxLength(100);

            entity.HasOne(d => d.Deal).WithMany(p => p.Payments).HasForeignKey(d => d.DealId);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project", "IncentiveManagement");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Teams", "IncentiveManagement");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.ToTable("TeamMembers", "IncentiveManagement");

            entity.HasIndex(e => e.TeamId, "IX_TeamMembers_TeamId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Role).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Team).WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TieredIncentiveTier>(entity =>
        {
            entity.ToTable("TieredIncentiveTiers", "IncentiveManagement");

            entity.HasIndex(e => e.TieredIncentivePlanId, "IX_TieredIncentiveTiers_TieredIncentivePlanId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FromValue).HasPrecision(18, 2);
            entity.Property(e => e.IncentiveValue).HasPrecision(18, 2);
            entity.Property(e => e.ToValue).HasPrecision(18, 2);

            entity.HasOne(d => d.TieredIncentivePlan).WithMany(p => p.TieredIncentiveTiers).HasForeignKey(d => d.TieredIncentivePlanId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
