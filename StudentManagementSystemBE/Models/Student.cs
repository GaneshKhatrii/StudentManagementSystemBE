namespace StudentManagementSystemBE.Models
{
    public class Student
    {
        // Primary key
        public int StudentId { get; set; }  
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        // Active by default
        public bool IsActive { get; set; } = true;

        // Set to current UTC time by default
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Foreign key to Teacher
        public int TeacherId { get; set; }

        // Navigation property to Teacher
        public Teacher? Teacher { get; set; } 
    }
}

