using System;
using MassTransit.Futures.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Entities
{
    public class OrchestratedJob : IEntityTypeConfiguration<OrchestratedJob>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Argument { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public DateTimeOffset? HeartBeatTime { get; set; }
        public JobLifecycleStatus Status { get; set; }
        public Guid? ServerId { get; set; }
        public virtual Server Server { get; set; }

        public void Configure(EntityTypeBuilder<OrchestratedJob> builder)
        {
            builder.ToTable("Jobs")
                .HasKey(o=>o.Id);
            
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Name).IsRequired();

            builder.Property(o => o.Status)
                .HasConversion(x => x.ToString(),
                    x => (JobLifecycleStatus) Enum.Parse(typeof(JobLifecycleStatus), x))
                .IsUnicode(false);
            
            builder.HasOne(e => e.Server)
                .WithMany(c => c.OrchestratedJobs);
        }
    }
}