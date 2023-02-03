using KnowledgeBaseForum.AdminWebApp.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeBaseForum.AdminWebApp.Models
{
    /// <summary>
    /// Model that will collect the "Usuario" data from the API and display in the front end views.
    /// </summary>
    public class UsuarioViewModel
    {
        /// <summary>
        /// User actual name.
        /// </summary>
        [Required]
        public string Nome { get; set; } = null!;

        /// <summary>
        /// User e-mail.
        /// </summary>
        [Display(Name = "E-mail")]
        [Required]
        public string Email { get; set; } = null!;

        /// <summary>
        /// User name used as login.
        /// </summary>
        public string Login { get; set; } = null!;

        /// <summary>
        /// User password.
        /// </summary>
        public string Senha { get; set; } = null!;

        /// <summary>
        /// User status (active).
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Current user profile.
        /// </summary>
        [Required]
        public UsuarioPerfilEnum Perfil { get; set; }

        /// <summary>
        /// Creation date-time.
        /// </summary>
        [Display(Name = "Data de Criação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Update date-time.
        /// </summary>
        public DateTime? DataModificacao { get; set; }

        /// <summary>
        /// Creation user.
        /// </summary>
        public string UsuarioCriacao { get; set; } = null!;

        /// <summary>
        /// Update user.
        /// </summary>
        public string? UsuarioModificacao { get; set; }
    }
}
