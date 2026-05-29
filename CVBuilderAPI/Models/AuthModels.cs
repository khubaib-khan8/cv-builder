// Models/AuthModels.cs
using System.ComponentModel.DataAnnotations;

namespace CVBuilderAPI.Models
{
    // ── Register ──────────────────────────────────────────────
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";
    }

    // ── Login ─────────────────────────────────────────────────
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }

    // ── Response sent back to frontend ───────────────────────
    public class AuthResponseDto
    {
        public string Token    { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email    { get; set; } = "";
        public int    UserId   { get; set; }
    }
}