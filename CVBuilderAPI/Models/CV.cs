// Models/CV.cs
using System.ComponentModel.DataAnnotations;

namespace CVBuilderAPI.Models
{
    public class CV
    {
        public int Id { get; set; }

        [Required] public string FullName { get; set; } = "";
        [Required][EmailAddress] public string Email { get; set; } = "";
        [Required] public string Phone    { get; set; } = "";
        public string? Address      { get; set; }
        public string? LinkedIn     { get; set; }
        public string? GitHub       { get; set; }
        public string? Summary      { get; set; }
        public string? TemplateName { get; set; } = "professional"; // ← NEW

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to User
        public int   UserId { get; set; }
        public User? User   { get; set; }

        public List<Experience> Experiences { get; set; } = new();
        public List<Education>  Educations  { get; set; } = new();
        public List<Skill>      Skills      { get; set; } = new();
    }

    public class Experience
    {
        public int    Id          { get; set; }
        public string JobTitle    { get; set; } = "";
        public string Company     { get; set; } = "";
        public string? StartDate  { get; set; }
        public string? EndDate    { get; set; }
        public string? Description { get; set; }
        public int    CVId { get; set; }
        public CV?    CV   { get; set; }
    }

    public class Education
    {
        public int    Id          { get; set; }
        public string Degree      { get; set; } = "";
        public string Institution { get; set; } = "";
        public string? StartYear  { get; set; }
        public string? EndYear    { get; set; }
        public int    CVId { get; set; }
        public CV?    CV   { get; set; }
    }

    public class Skill
    {
        public int    Id   { get; set; }
        public string Name { get; set; } = "";
        public int    CVId { get; set; }
        public CV?    CV   { get; set; }
    }
}