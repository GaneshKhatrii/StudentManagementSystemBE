using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementSystemBE.Models;

namespace StudentManagementSystemBE.FluentAPIConfigurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> entity) {
            entity.HasKey(entity => entity.StudentId);

            entity.Property(entity => entity.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(entity => entity.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(entity => entity.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(entity => entity.MobileNumber)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(entity => entity.IsActive)
                .IsRequired();

            // HasDefaultValue is used to set a default value for the CreatedDate property when a new Student entity is added to the database.
            // GETUTCDATE() is a SQL Server function which is used to get the current UTC date and time. 
            entity.Property(entity => entity.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(entity => entity.Email)
                .IsUnique();
            // HasOne is used to specify a one-to-many relationship between the Student and Teacher entities.
            // OnDelete(DeleteBehavior.Cascade) is used to specify that when a Teacher entity is deleted, all related Student entities should also be deleted from the database.
            entity.HasOne(entity => entity.Teacher)
                .WithMany(student => student.Students)
                .HasForeignKey(entity => entity.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
