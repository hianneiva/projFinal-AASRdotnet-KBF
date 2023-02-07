namespace KnowledgeBaseForum.AdminWebApp.Models.Enum
{
    /// <summary>
    /// Enumeration of the profiles of the users in the system.
    /// </summary>
    public enum UsuarioPerfilEnum : int
    {
        Normal = 1,
        Administrador = 2
    }

    public enum TopicoTipoAcessoEnum : int
    {
        WIP = 0,
        Publicado = 1,
        Finalizado = 2
    }
}
