using KnowledgeBaseForum.AdminWebApp.Models.Enum;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeBaseForum.AdminWebApp.Models
{
    public class TopicoViewModel : BaseViewModel
    {
        /// <summary>
        /// Topic title.
        /// </summary>
        [Display(Name = "Título")]
        public string Titulo { get; set; } = null!;

        /// <summary>
        /// Topic content.
        /// </summary>
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; } = null!;

        /// <summary>
        /// Access type (enum).
        /// </summary>
        [Display(Name = "Estado Publicação")]
        public TopicoTipoAcessoEnum TipoAcesso { get; set; }

        /// <summary>
        /// Topic status (active).
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Author (User) Id.
        /// </summary>
        public string UsuarioId { get; set; } = null!;

        /// <summary>
        /// Comments tied to the topic.
        /// </summary>
        public IEnumerable<ComentarioViewModel>? Comentarios { get; set; }

        /// <summary>
        /// Tags tied to the topic.
        /// </summary>
        [Display(Name = "Tags")]
        [JsonProperty("TopicoTag")]
        public IEnumerable<TopicoTagLink>? TagLinks { get; set; }

        /// <summary>
        /// Author user.
        /// </summary>
        [Display(Name = "Autor")]
        public UsuarioViewModel? Usuario { get; set; }

        /// <summary>
        /// Returns the author's name.
        /// </summary>
        public string? AuthorName => Usuario?.Nome;

        /// <summary>
        /// Returns tag list as string.
        /// </summary>
        public string? Tags => string.Join(", ", TagLinks?.Select(tl => tl.Tag?.Descricao) ?? new string[] { });
    }

    public class TopicoTagLink
    {
        /// <summary>
        /// TBTag Id.
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// Topic Id.
        /// </summary>
        public Guid TopicoId { get; set; }

        /// <summary>
        /// Related tag.
        /// </summary>
        public TagViewModel Tag { get; set; } = null!;
    }
}
