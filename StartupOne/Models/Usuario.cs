using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace StartupOne.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Email { get; set;}
        [Required]
        public string Password { get; set; }
        public ICollection<EventosMarcados>? EventosMarcados { get; set; } = new List<EventosMarcados>();
        public ICollection<EventosPendentes>? EventosPendentes { get; set; } = new List<EventosPendentes>();

        public Usuario(int idUsuario, string login, string email, string password)
        {
            IdUsuario = idUsuario;
            Login = login;
            Email = email;
            Password = CriptografarSenha(password);
        }

        private string CriptografarSenha(string senha)
        {
            if (senha == null)
                return "";

            byte[] bytesSenha = Encoding.UTF8.GetBytes(senha);
            SHA256 sha256 = SHA256.Create();
            byte[] hashSenha = sha256.ComputeHash(bytesSenha);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hashSenha)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
