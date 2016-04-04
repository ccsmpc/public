using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AgendaProject.Models;

namespace AgendaProject.DAL
{
    public class AgendaContext : DbContext
    {
        public AgendaContext() : base("AgendaContext")
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<AgendaItem> AgendaItems { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<SectionHeading> SectionHeadings { get; set; }
        public DbSet<StaffDecision> StaffDecisions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteItem> VoteItems { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<MeetingTime> MeetingTimes { get; set; }
        public DbSet<DropDown> DropDowns { get; set; }

        public DbSet<BoardMember> BoardMembers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<AgendaItem>()
                .HasOptional<WorkItem>(s => s.WorkItem)
                .WithMany(s => s.AgendaItems)
                .HasForeignKey(s => s.WorkItemId);


        }
    }
}
