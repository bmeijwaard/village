using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Village.Data.Domain;

namespace Village.Data.Identity
{
    public static class SecurityModelBuilder
    {
        public static ModelBuilder CreateCustomSecurityModel(this ModelBuilder builder, string schema = "dbo")
        {

            builder.Entity<User>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<Role>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users", schema: schema);
                entity.Property(e => e.Id).HasColumnName("Id");

            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Roles", schema: schema);
                entity.Property(e => e.Id).HasColumnName("RoleId");

            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims", schema: schema);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Id).HasColumnName("UserClaimId");

            });

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins", schema: schema);
                entity.Property(e => e.UserId).HasColumnName("UserId");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaims", schema: schema);
                entity.Property(e => e.Id).HasColumnName("RoleClaimId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("UserRoles", schema: schema);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");

            });

            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens", schema: schema);
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            return builder;
        }
    }
}
