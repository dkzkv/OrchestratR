using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrchestratR.ServerManager.Persistence.Entities
{
    public class Server : IEntityTypeConfiguration<Server>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxWorkersCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public ICollection<OrchestratedJob> OrchestratedJobs { get; set; }
        public bool IsDeleted { get; set; }
        
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.ToTable("Servers")
                .HasKey(o=>o.Id);
            
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Name).IsRequired();

            builder.HasMany(x => x.OrchestratedJobs)
                .WithOne(b => b.Server)
                .HasForeignKey(b => b.ServerId);
        }
    }
}