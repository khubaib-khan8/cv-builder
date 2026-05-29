// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace CVBuilderAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One user can have many CVs
        public List<CV> CVs { get; set; } = new();
    }
}