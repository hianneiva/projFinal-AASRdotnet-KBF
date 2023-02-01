using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;

namespace KnowledgeBaseForum.DataAccessLayer.Model
{
    public class Grupo : BaseEntity
    {
        /// <summary>
        /// Group description or name.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Group status (active).
        /// </summary>
        public bool Status { get; set; }

        // Relational properties.

        /// <summary>
        /// user-group relation.
        /// </summary>
        public IEnumerable<UsuarioGrupo>? UsuarioGrupo { get; set; }
    }
}
