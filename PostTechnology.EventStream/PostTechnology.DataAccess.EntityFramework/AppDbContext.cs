using Microsoft.EntityFrameworkCore;
using PostTechnology.DataAccess.EntityFramework.Entities;

namespace PostTechnology.DataAccess.EntityFramework
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TxMessage> SentMessages { get; set; }
        public virtual DbSet<RxMessage> ReceivedMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var sentMessageEntity = modelBuilder.Entity<TxMessage>();
            sentMessageEntity.ToTable("SentMessages");
            sentMessageEntity.HasKey(c => c.Id);
            sentMessageEntity.Property(c => c.Number).IsRequired();
            sentMessageEntity.Property(c => c.Sent).IsRequired();
            sentMessageEntity.Property(c => c.Content).IsRequired();
            sentMessageEntity.Property(c => c.Hash).IsRequired();
            sentMessageEntity.HasIndex(c => c.Number).IsUnique();

            var recvMessageEntity = modelBuilder.Entity<RxMessage>();
            recvMessageEntity.ToTable("ReceivedMessages");
            recvMessageEntity.HasKey(c => c.Id);
            recvMessageEntity.Property(c => c.Number).IsRequired();
            recvMessageEntity.Property(c => c.Sent).IsRequired();
            recvMessageEntity.Property(c => c.Content).IsRequired();
            recvMessageEntity.Property(c => c.Hash).IsRequired();
            recvMessageEntity.Property(c => c.Received).IsRequired();
            recvMessageEntity.HasIndex(c => c.Number).IsUnique();
        }
    }
}
