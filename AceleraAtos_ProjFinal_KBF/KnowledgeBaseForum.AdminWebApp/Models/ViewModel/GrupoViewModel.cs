using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KnowledgeBaseForum.AdminWebApp.Models.ViewModel
{
    /// <summary>
    /// Model that will collect the "Grupo" data from the API and display in the front end views.
    /// </summary>
    public class GrupoViewModel : BaseViewModel
    {
        /// <summary>
        /// Group name and/or description.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// The entry status, <c>true</c> if active, <c>false</c> otherwise.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Users tied to the group.
        /// </summary>
        [JsonProperty("UsuarioGrupo")]
        public IEnumerable<UsuarioGrupoLink>? UserLinks { get; set; }
    }

    /// <summary>
    /// Implements relational link between Users and Groups.
    /// </summary>
    public class UsuarioGrupoLink
    {
        /// <summary>
        /// Username.
        /// </summary>
        public string UsuarioId { get; set; } = null!;

        /// <summary>
        /// Group Id.
        /// </summary>
        public Guid GrupoId { get; set; }

        /// <summary>
        /// Related User.
        /// </summary>
        public UsuarioViewModel? Usuario { get; set; }
    }
}
