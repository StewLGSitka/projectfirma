//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ProjectImageUpdate]
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProjectFirmaModels.Models
{
    public class ProjectImageUpdateConfiguration : EntityTypeConfiguration<ProjectImageUpdate>
    {
        public ProjectImageUpdateConfiguration() : this("dbo"){}

        public ProjectImageUpdateConfiguration(string schema)
        {
            ToTable("ProjectImageUpdate", schema);
            HasKey(x => x.ProjectImageUpdateID);
            Property(x => x.ProjectImageUpdateID).HasColumnName(@"ProjectImageUpdateID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TenantID).HasColumnName(@"TenantID").HasColumnType("int").IsRequired();
            Property(x => x.FileResourceID).HasColumnName(@"FileResourceID").HasColumnType("int").IsOptional();
            Property(x => x.ProjectUpdateBatchID).HasColumnName(@"ProjectUpdateBatchID").HasColumnType("int").IsRequired();
            Property(x => x.ProjectImageTimingID).HasColumnName(@"ProjectImageTimingID").HasColumnType("int").IsRequired();
            Property(x => x.Caption).HasColumnName(@"Caption").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(200);
            Property(x => x.Credit).HasColumnName(@"Credit").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(200);
            Property(x => x.IsKeyPhoto).HasColumnName(@"IsKeyPhoto").HasColumnType("bit").IsRequired();
            Property(x => x.ExcludeFromFactSheet).HasColumnName(@"ExcludeFromFactSheet").HasColumnType("bit").IsRequired();
            Property(x => x.ProjectImageID).HasColumnName(@"ProjectImageID").HasColumnType("int").IsOptional();

            // Foreign keys
            HasOptional(a => a.FileResource).WithMany(b => b.ProjectImageUpdates).HasForeignKey(c => c.FileResourceID).WillCascadeOnDelete(false); // FK_ProjectImageUpdate_FileResource_FileResourceID
            HasRequired(a => a.ProjectUpdateBatch).WithMany(b => b.ProjectImageUpdates).HasForeignKey(c => c.ProjectUpdateBatchID).WillCascadeOnDelete(false); // FK_ProjectImageUpdate_ProjectUpdateBatch_ProjectUpdateBatchID
            HasOptional(a => a.ProjectImage).WithMany(b => b.ProjectImageUpdates).HasForeignKey(c => c.ProjectImageID).WillCascadeOnDelete(false); // FK_ProjectImageUpdate_ProjectImage_ProjectImageID
        }
    }
}