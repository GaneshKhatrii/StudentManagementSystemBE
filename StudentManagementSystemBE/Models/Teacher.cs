namespace StudentManagementSystemBE.Models
{
    public class Teacher
    {
        // Primary key
        public int TeacherId { get; set; } 
        public string TeacherName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Teacher role by default
        public string Role { get; set; } = "Teacher"; 

        // Navigation property to Students
        public ICollection<Student> Students { get; set; } = new List<Student>(); 
    }
}
