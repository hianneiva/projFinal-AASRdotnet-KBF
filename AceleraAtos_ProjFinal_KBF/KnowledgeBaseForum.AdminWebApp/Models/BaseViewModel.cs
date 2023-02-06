using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KnowledgeBaseForum.AdminWebApp.Models
{
    public abstract class BaseViewModel
    {
        /// <summary>
        /// The row Id.
        /// </summary>
        public Guid Id { get; set; }

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
        /// Creation user name.
        /// </summary>
        [Display(Name = "Usuário Criador")]
        public string UsuarioCriacao { get; set; } = null!;

        /// <summary>
        /// Update user name.
        /// </summary>
        public string? UsuarioModificacao { get; set; }
    }
}
