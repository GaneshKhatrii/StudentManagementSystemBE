using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementSystemBE.Models;

namespace StudentManagementSystemBE.FluentAPIConfigurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        // Here entity = Teacher, because we are configuring the Teacher entity
        public void Configure(EntityTypeBuilder<Teacher> entity)
        {
            entity.HasKey(teacher => teacher.TeacherId);

            entity.Property(teacher => teacher.TeacherName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(teacher => teacher.PasswordHash)
                .IsRequired();

            entity.Property(teacher => teacher.Role)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(teacher => teacher.TeacherName)
                .IsUnique();
        }
    }
}

