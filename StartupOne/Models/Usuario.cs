using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace StartupOne.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set;}
        [Required]
        public string Password { get; set; }
        public ICollection<EventoMarcado>? EventosMarcados { get; set; } = new List<EventoMarcado>();
        public ICollection<Interesses>? EventosPendentes { get; set; } = new List<Interesses>();

        public Usuario(int idUsuario, string nome, string email, string password)
        {
            IdUsuario = idUsuario;
            Nome = nome;
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

        public bool ValidarSenha(string senha)
        {
            string senhaCriptografada = CriptografarSenha(CriptografarSenha(senha));

            if(this.Password.Equals(senhaCriptografada)) return true;

            return false;
        }
    }
}
