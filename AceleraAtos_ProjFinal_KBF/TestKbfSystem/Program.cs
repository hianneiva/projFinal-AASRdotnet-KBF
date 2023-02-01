using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace TestKbfSystem
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Database test:");
            await DatabaseTest();
            Console.ReadKey();
        }

        private static async Task DatabaseTest()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseSqlServer("Data Source=localhost;Initial Catalog=DB_KBFORUM;Integrated Security=True");

            using (KbfContext context = new KbfContext(optionsBuilder.Options)) 
            {
                Usuario userTest = new Usuario()
                {
                    Login = "TEST_USR",
                    Email = "test@test.net",
                    Nome = "Usuário de Teste",
                    Perfil = 0,
                    Senha = "123456",
                    Status = true,
                    UsuarioCriacao = "DEFAULT"
                };

                Console.Write("Adding user to DB... ");
                
                try
                {
                    await context.AddAsync(userTest);
                    await context.SaveChangesAsync();
                    Console.WriteLine("DONE!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAILURE! {ex.Message}\n{ex.StackTrace}");
                }

                Console.Write("Reading user from DB... ");

                try
                {
                    Usuario? readUser = context.Usuarios.FirstOrDefault(u => u.Login.Equals(userTest.Login));
                    Console.WriteLine($"DONE!\nUser data:\t{readUser?.Login}, {readUser?.Nome}, {readUser?.Email}, {readUser?.DataCriacao:dd/MM/yyyy}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAILURE! {ex.Message}\n{ex.StackTrace}");
                }

                Console.Write("Updating user from DB... ");

                try
                {

                    Usuario? readUser = context.Usuarios.FirstOrDefault(u => u.Login.Equals(userTest.Login));
                    readUser!.Nome = "Usuário teste atualizado";
                    context.Entry(readUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();
                    Usuario? readUserUpd = context.Usuarios.FirstOrDefault(u => u.Login.Equals(userTest.Login));
                    Console.WriteLine($"DONE!\nUser data:\t{readUserUpd?.Login}, {readUserUpd?.Nome}, {readUserUpd?.Email}, {readUser?.DataCriacao:dd/MM/yyyy}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAILURE! {ex.Message}\n{ex.StackTrace}");
                }

                Console.WriteLine($"Current entries in TBUsuario table: {context.Usuarios.Count()}\n");
                Console.Write("Deleting user from DB... ");

                try
                {
                    Usuario? readUser = context.Usuarios.FirstOrDefault(u => u.Login.Equals(userTest.Login));
                    context.Remove(readUser!);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"DONE!\nCurrent entries in TBUsuario table: {context.Usuarios.Count()}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAILURE! {ex.Message}\n{ex.StackTrace}");
                }
            }
        }
    }
}