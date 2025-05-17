DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'IncentiveManagement') THEN
        CREATE SCHEMA "IncentiveManagement";
    END IF;
END $EF$;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'Identity') THEN
        CREATE SCHEMA "Identity";
    END IF;
END $EF$;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'Tenant') THEN
        CREATE SCHEMA "Tenant";
    END IF;
END $EF$;


CREATE TABLE "IncentiveManagement"."IncentiveRules" (
    "Id" uuid NOT NULL,
    "Name" character varying(200) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Frequency" text NOT NULL,
    "AppliedTo" text NOT NULL,
    "Currency" text NOT NULL,
    "Target" text NOT NULL,
    "Incentive" text NOT NULL,
    "Salary" numeric(18,2),
    "TargetValue" numeric(18,2),
    "TargetDealCount" integer,
    "Commission" numeric(18,2),
    "StartDate" timestamp with time zone,
    "EndDate" timestamp with time zone,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "IsIncludeSalary" boolean NOT NULL DEFAULT TRUE,
    "MinimumSalesThreshold" numeric(18,2),
    "MinimumDealCountThreshold" integer,
    "MaximumIncentiveAmount" numeric(18,2),
    "TeamId" uuid,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_IncentiveRules" PRIMARY KEY ("Id")
);


CREATE TABLE "IncentiveManagement"."Project" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Location" character varying(255) NOT NULL,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone,
    "TotalValue" numeric NOT NULL,
    "IsActive" boolean NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_Project" PRIMARY KEY ("Id")
);


CREATE TABLE "Identity"."Roles" (
    "Id" text NOT NULL,
    "TenantId" text NOT NULL,
    "Description" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" text NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);


CREATE TABLE "IncentiveManagement"."Teams" (
    "Id" uuid NOT NULL,
    "Name" character varying(200) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "IsActive" boolean NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_Teams" PRIMARY KEY ("Id")
);


CREATE TABLE "Tenant"."Tenants" (
    "Id" text NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Identifier" character varying(50) NOT NULL,
    "ConnectionString" character varying(500),
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Tenants" PRIMARY KEY ("Id")
);


CREATE TABLE "Identity"."Users" (
    "Id" text NOT NULL,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "TenantId" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" text NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" text NOT NULL,
    "RefreshToken" text NOT NULL,
    "RefreshTokenExpiryTime" timestamp with time zone NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);


CREATE TABLE "IncentiveManagement"."Deals" (
    "Id" uuid NOT NULL,
    "DealName" character varying(200) NOT NULL,
    "CustomerName" character varying(200) NOT NULL,
    "CustomerEmail" character varying(100),
    "CustomerPhone" character varying(50),
    "CustomerAddress" character varying(500),
    "TotalAmount" numeric(18,2) NOT NULL,
    "PaidAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "RemainingAmount" numeric(18,2) NOT NULL,
    "CurrencyType" character varying(50) NOT NULL,
    "TaxPercentage" numeric(5,2) NOT NULL DEFAULT 0.0,
    "TaxAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "DiscountAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "Status" text NOT NULL,
    "DealDate" timestamp with time zone NOT NULL,
    "ClosedDate" timestamp with time zone,
    "PaymentDueDate" timestamp with time zone,
    "ClosedByUserId" text,
    "TeamId" uuid,
    "ReferralName" character varying(200),
    "ReferralEmail" character varying(100),
    "ReferralPhone" character varying(50),
    "ReferralCommission" numeric(18,2),
    "IsReferralCommissionPaid" boolean NOT NULL,
    "Source" character varying(100) NOT NULL,
    "IncentiveRuleId" uuid,
    "Notes" character varying(2000),
    "RecurringFrequencyMonths" integer,
    "UserId" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_Deals" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Deals_IncentiveRules_IncentiveRuleId" FOREIGN KEY ("IncentiveRuleId") REFERENCES "IncentiveManagement"."IncentiveRules" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "Identity"."RoleClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_RoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Identity"."Roles" ("Id") ON DELETE CASCADE
);


CREATE TABLE "IncentiveManagement"."IncentivePlans" (
    "Id" uuid NOT NULL,
    "PlanName" character varying(200) NOT NULL,
    "PlanType" text NOT NULL,
    "PeriodType" text NOT NULL,
    "StartDate" timestamp with time zone,
    "EndDate" timestamp with time zone,
    "IsActive" boolean NOT NULL,
    "PlanDiscriminator" character varying(21) NOT NULL,
    "Location" character varying(200),
    "MetricType" text,
    "TargetValue" numeric(18,2),
    "ConsistencyMonths" integer,
    "AwardType" text,
    "AwardValue" numeric(18,2),
    "GiftDescription" character varying(500),
    "ProjectId" uuid,
    "ProjectBasedIncentivePlan_MetricType" text,
    "ProjectBasedIncentivePlan_TargetValue" numeric(18,2),
    "IncentiveValue" numeric(18,2),
    "CalculationType" text,
    "IncentiveAfterExceedingTarget" boolean,
    "Role" character varying(100),
    "IsTeamBased" boolean,
    "TeamId" uuid,
    "TargetType" text,
    "SalaryPercentage" numeric(5,2),
    "RoleBasedIncentivePlan_MetricType" text,
    "RoleBasedIncentivePlan_TargetValue" numeric(18,2),
    "RoleBasedIncentivePlan_CalculationType" text,
    "RoleBasedIncentivePlan_IncentiveValue" numeric(18,2),
    "IsCumulative" boolean,
    "RoleBasedIncentivePlan_IncentiveAfterExceedingTarget" boolean,
    "IncludeSalaryInTarget" boolean,
    "TargetBasedIncentivePlan_TargetType" integer,
    "Salary" numeric(5,2),
    "TargetBasedIncentivePlan_MetricType" integer,
    "TargetBasedIncentivePlan_TargetValue" numeric(18,2),
    "TargetBasedIncentivePlan_CalculationType" integer,
    "TargetBasedIncentivePlan_IncentiveValue" numeric(18,2),
    "TargetBasedIncentivePlan_IncentiveAfterExceedingTarget" boolean,
    "ProvideAdditionalIncentiveOnExceeding" boolean,
    "TargetBasedIncentivePlan_IncludeSalaryInTarget" boolean,
    "TieredIncentivePlan_MetricType" text,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_IncentivePlans" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_IncentivePlans_Project_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "IncentiveManagement"."Project" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_IncentivePlans_Teams_TeamId" FOREIGN KEY ("TeamId") REFERENCES "IncentiveManagement"."Teams" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "IncentiveManagement"."TeamMembers" (
    "Id" uuid NOT NULL,
    "TeamId" uuid NOT NULL,
    "UserId" character varying(450) NOT NULL,
    "Role" character varying(100) NOT NULL,
    "IsActive" boolean NOT NULL,
    "JoinedDate" timestamp with time zone NOT NULL,
    "LeftDate" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_TeamMembers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TeamMembers_Teams_TeamId" FOREIGN KEY ("TeamId") REFERENCES "IncentiveManagement"."Teams" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "Identity"."UserClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_UserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Identity"."Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Identity"."UserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_UserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Identity"."Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Identity"."UserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Identity"."Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Identity"."Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Identity"."UserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text,
    CONSTRAINT "PK_UserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Identity"."Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "IncentiveManagement"."DealActivities" (
    "Id" uuid NOT NULL,
    "DealId" uuid NOT NULL,
    "Type" text NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Notes" character varying(1000),
    "ActivityDate" timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_DealActivities" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_DealActivities_Deals_DealId" FOREIGN KEY ("DealId") REFERENCES "IncentiveManagement"."Deals" ("Id") ON DELETE CASCADE
);


CREATE TABLE "IncentiveManagement"."IncentiveEarnings" (
    "Id" uuid NOT NULL,
    "IncentiveRuleId" uuid NOT NULL,
    "UserId" text NOT NULL,
    "DealId" uuid NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "EarningDate" timestamp with time zone NOT NULL,
    "Status" text NOT NULL,
    "IsPaid" boolean NOT NULL,
    "PaidDate" timestamp with time zone,
    "Notes" character varying(500),
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_IncentiveEarnings" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_IncentiveEarnings_Deals_DealId" FOREIGN KEY ("DealId") REFERENCES "IncentiveManagement"."Deals" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_IncentiveEarnings_IncentiveRules_IncentiveRuleId" FOREIGN KEY ("IncentiveRuleId") REFERENCES "IncentiveManagement"."IncentiveRules" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "IncentiveManagement"."Payments" (
    "Id" uuid NOT NULL,
    "DealId" uuid NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "PaymentDate" timestamp with time zone NOT NULL,
    "PaymentMethod" character varying(50) NOT NULL,
    "TransactionReference" character varying(100),
    "Notes" character varying(500),
    "ReceivedByUserId" text,
    "IsVerified" boolean NOT NULL DEFAULT FALSE,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_Payments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Payments_Deals_DealId" FOREIGN KEY ("DealId") REFERENCES "IncentiveManagement"."Deals" ("Id") ON DELETE CASCADE
);


CREATE TABLE "IncentiveManagement"."TieredIncentiveTiers" (
    "Id" uuid NOT NULL,
    "TieredIncentivePlanId" uuid NOT NULL,
    "FromValue" numeric(18,2) NOT NULL,
    "ToValue" numeric(18,2) NOT NULL,
    "IncentiveValue" numeric(18,2) NOT NULL,
    "CalculationType" text NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "LastModifiedAt" timestamp with time zone,
    "LastModifiedBy" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "TenantId" text NOT NULL,
    CONSTRAINT "PK_TieredIncentiveTiers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TieredIncentiveTiers_IncentivePlans_TieredIncentivePlanId" FOREIGN KEY ("TieredIncentivePlanId") REFERENCES "IncentiveManagement"."IncentivePlans" ("Id") ON DELETE CASCADE
);


CREATE INDEX "IX_DealActivities_DealId" ON "IncentiveManagement"."DealActivities" ("DealId");


CREATE INDEX "IX_Deals_IncentiveRuleId" ON "IncentiveManagement"."Deals" ("IncentiveRuleId");


CREATE INDEX "IX_IncentiveEarnings_DealId" ON "IncentiveManagement"."IncentiveEarnings" ("DealId");


CREATE INDEX "IX_IncentiveEarnings_IncentiveRuleId" ON "IncentiveManagement"."IncentiveEarnings" ("IncentiveRuleId");


CREATE INDEX "IX_IncentivePlans_ProjectId" ON "IncentiveManagement"."IncentivePlans" ("ProjectId");


CREATE INDEX "IX_IncentivePlans_TeamId" ON "IncentiveManagement"."IncentivePlans" ("TeamId");


CREATE INDEX "IX_Payments_DealId" ON "IncentiveManagement"."Payments" ("DealId");


CREATE INDEX "IX_RoleClaims_RoleId" ON "Identity"."RoleClaims" ("RoleId");


CREATE UNIQUE INDEX "RoleNameIndex" ON "Identity"."Roles" ("NormalizedName");


CREATE INDEX "IX_TeamMembers_TeamId" ON "IncentiveManagement"."TeamMembers" ("TeamId");


CREATE UNIQUE INDEX "IX_Tenants_Identifier" ON "Tenant"."Tenants" ("Identifier");


CREATE INDEX "IX_TieredIncentiveTiers_TieredIncentivePlanId" ON "IncentiveManagement"."TieredIncentiveTiers" ("TieredIncentivePlanId");


CREATE INDEX "IX_UserClaims_UserId" ON "Identity"."UserClaims" ("UserId");


CREATE INDEX "IX_UserLogins_UserId" ON "Identity"."UserLogins" ("UserId");


CREATE INDEX "IX_UserRoles_RoleId" ON "Identity"."UserRoles" ("RoleId");


CREATE INDEX "EmailIndex" ON "Identity"."Users" ("NormalizedEmail");


CREATE UNIQUE INDEX "UserNameIndex" ON "Identity"."Users" ("NormalizedUserName");


