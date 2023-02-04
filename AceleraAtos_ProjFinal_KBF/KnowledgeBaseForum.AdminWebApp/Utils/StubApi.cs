using KnowledgeBaseForum.AdminWebApp.Models;

namespace KnowledgeBaseForum.AdminWebApp.Utils
{
    /// <summary>
    /// Class that implements functionality to simulate API calls.
    /// </summary>
    public static class StubApi
    {
        #region Entities for "Topico" objects manipulation"
        private static IList<TagViewModel> stubbedTags = new List<TagViewModel>()
        {
            new TagViewModel() { Id = new Guid("e448ec71-b9c0-43e4-9d00-03bea7b91767"), Descricao = "Tag_1" },
            new TagViewModel() { Id = new Guid("b98d322d-fe32-4a13-9cfa-f2849994f57a"), Descricao = "Tag_2" },
            new TagViewModel() { Id = new Guid("372a30e0-ce68-4e21-a6c6-962befd40fcd"), Descricao = "Tag_3" }
        };

        private static IList<TopicoViewModel> stubbedTopicos = new List<TopicoViewModel>()
        {
            new TopicoViewModel ()
            {
                Id = new Guid("57738f58-5378-46f8-b8b1-7bba80498d90"),
                Conteudo = "Tópico 01",
                Comentarios = new List<ComentarioViewModel>()
                {
                    new ComentarioViewModel() { Id = new Guid(), DataCriacao = DateTime.Now, Conteudo = "Isso é muito interessante." },
                    new ComentarioViewModel() { Id = new Guid(), DataCriacao = DateTime.Now, Conteudo = "Não sabia disso." },
                    new ComentarioViewModel() { Id = new Guid(), DataCriacao = DateTime.Now, Conteudo = "Há mais referências?" }
                },
                DataCriacao = DateTime.Now,
                Status = true,
                TagLinks = new List<TopicoTagLink>()
                {
                    new TopicoTagLink() 
                    {
                        TopicoId = new Guid("57738f58-5378-46f8-b8b1-7bba80498d90"),
                        TagId = new Guid("e448ec71-b9c0-43e4-9d00-03bea7b91767"),
                        Tag = stubbedTags[0]
                    },
                    new TopicoTagLink() 
                    {
                        TopicoId = new Guid("57738f58-5378-46f8-b8b1-7bba80498d90"),
                        TagId = new Guid("372a30e0-ce68-4e21-a6c6-962befd40fcd"),
                        Tag = stubbedTags[2]
                    }
                },
                Titulo = "Tópico 1",
                UsuarioId = "USR_1",
                Usuario = new UsuarioViewModel()
                {
                    Login = "USR_1",
                    Nome = "Usuário 1"
                }
            },
            new TopicoViewModel ()
            {
                Id = new Guid("daa645bb-1fbb-4329-8621-d7f4b4ef57c5"),
                Conteudo = "Tópico 002",
                Comentarios = new List<ComentarioViewModel>()
                {
                    new ComentarioViewModel() { Id = new Guid(), DataCriacao = DateTime.Now, Conteudo = "Tem isso para uma outra linguagem?" },
                },
                DataCriacao = DateTime.Now,
                Status = true,
                TagLinks = new List<TopicoTagLink>()
                {
                    new TopicoTagLink()
                    {
                        TopicoId = new Guid("daa645bb-1fbb-4329-8621-d7f4b4ef57c5"),
                        TagId = new Guid("b98d322d-fe32-4a13-9cfa-f2849994f57a"),
                        Tag = stubbedTags[1]
                    }
                },
                Titulo = "Tópico 2",
                UsuarioId = "USR_2",
                Usuario = new UsuarioViewModel()
                {
                    Login = "USR_2",
                    Nome = "Usuário 2"
                }
            },
            new TopicoViewModel ()
            {
                Id = new Guid("6b9a2852-5199-4c91-8631-4778debc423d"),
                Conteudo = "Tópico 0003",
                DataCriacao = DateTime.Now,
                Status = true,
                Titulo = "Tópico 3",
                UsuarioId = "USR_3",
                Usuario = new UsuarioViewModel()
                {
                    Login = "USR_3",
                    Nome = "Usuário 3"
                }
             }
        };
        #endregion

        #region
        // Topico entity stub operations
        public static IEnumerable<TopicoViewModel> SimulateListTopico() => stubbedTopicos;
        public static TopicoViewModel? SimulateGetTopico(Guid id) => stubbedTopicos.FirstOrDefault(t => t.Id == id);
        public static void SimulateEditTopico(TopicoViewModel toUpd)
        {
            TopicoViewModel? original = stubbedTopicos.FirstOrDefault(t => t.Id == toUpd.Id);

            if (original == null)
            {
                return;
            }

            original.Status = toUpd?.Status ?? original.Status;
            original.Comentarios = toUpd?.Comentarios ?? original.Comentarios;
            original.TipoAcesso = toUpd?.TipoAcesso ?? original.TipoAcesso;
            original.DataModificacao = DateTime.Now;
            original.UsuarioModificacao = "SYSTEM";
        }

        // Tag entity stub operations
        public static IEnumerable<TagViewModel> SimulateListTags() => stubbedTags;
        #endregion
    }
}
