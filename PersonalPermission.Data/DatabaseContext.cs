using Microsoft.EntityFrameworkCore;
using PersonalPermission.Core;
using PersonalPermission.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Data
{
    public class DatabaseContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        public DatabaseContext(DbContextOptions options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UsedAdministrivePermission> UsedAdministrivePermissions { get; set; }
        public DbSet<UsedYearlyPermission> UsedYearlyPermissions { get; set; }
        public DbSet<PermissionState> PermissionStates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsedAdministrivePermission>()
    .HasOne(x => x.User)
    .WithMany(x => x.UsedAdministrivePermissions)
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsedYearlyPermission>()
.HasOne(x => x.User)
.WithMany(x => x.UsedYearlyPermissions)
.HasForeignKey(x => x.UserId)
.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PermissionState>()
.HasOne(x => x.User)
.WithMany(x => x.PermissionStates)
.HasForeignKey(x => x.UserId)
.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // çalışan dll içinden configuration class ları bul
            base.OnModelCreating(modelBuilder);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = OnBeforeSaveChanges();

            var result = await base.SaveChangesAsync(cancellationToken);

            if (auditEntries.Any())
            {
                await AuditLogs.AddRangeAsync(auditEntries);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        private List<AuditLog> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditLog>();
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var audit = new AuditLog
                {
                    TableName = entry.Metadata.GetTableName(),
                    ChangedAt = DateTime.UtcNow,
                    UserName = _currentUserService.Username, // burada o anki kullanıcıyı inject edebilirsin
                    Action = entry.State.ToString()
                };

                // Primary key
                var keyValues = new Dictionary<string, object>();
                foreach (var property in entry.Properties.Where(p => p.Metadata.IsPrimaryKey()))
                {
                    keyValues[property.Metadata.Name] = property.CurrentValue;
                }
                audit.KeyValues = System.Text.Json.JsonSerializer.Serialize(keyValues);

                // Added
                if (entry.State == EntityState.Added)
                {
                    var newValues = new Dictionary<string, object>();
                    foreach (var property in entry.Properties)
                    {
                        newValues[property.Metadata.Name] = property.CurrentValue;
                    }
                    audit.NewValues = System.Text.Json.JsonSerializer.Serialize(newValues);
                }
                // Deleted
                else if (entry.State == EntityState.Deleted)
                {
                    var oldValues = new Dictionary<string, object>();
                    foreach (var property in entry.Properties)
                    {
                        oldValues[property.Metadata.Name] = property.OriginalValue;
                    }
                    audit.OldValues = System.Text.Json.JsonSerializer.Serialize(oldValues);
                }
                // Modified
                else if (entry.State == EntityState.Modified)
                {
                    var oldValues = new Dictionary<string, object>();
                    var newValues = new Dictionary<string, object>();
                    foreach (var property in entry.Properties)
                    {
                        if (property.IsModified)
                        {
                            oldValues[property.Metadata.Name] = property.OriginalValue;
                            newValues[property.Metadata.Name] = property.CurrentValue;
                        }
                    }
                    audit.OldValues = System.Text.Json.JsonSerializer.Serialize(oldValues);
                    audit.NewValues = System.Text.Json.JsonSerializer.Serialize(newValues);
                }

                auditEntries.Add(audit);
            }

            return auditEntries;
        }

    }
}
