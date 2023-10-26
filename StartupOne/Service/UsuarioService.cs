using StartupOne.Dto.Usuario;
using StartupOne.Models;
using StartupOne.Repository;
using System.Text.RegularExpressions;

namespace StartupOne.Service
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository = new();

        public void ValidarUsuario(CadastrarUsuarioDto usuarioDto)
        {
            string emailPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";

            if (usuarioDto.Password != usuarioDto.Passwordconfirm)
                throw new Exception("Senhas não conferem.");

            if (!Regex.IsMatch(usuarioDto.Email, emailPattern))
                throw new Exception("Email inválido");

            if (string.IsNullOrWhiteSpace(usuarioDto.Password) || string.IsNullOrWhiteSpace(usuarioDto.Passwordconfirm))
                throw new Exception("Preencha a senha");

            if (string.IsNullOrWhiteSpace(usuarioDto.Email))
                throw new Exception("Preencha o Email");

            if (string.IsNullOrWhiteSpace(usuarioDto.Nome))
                throw new Exception("Preencha o Nome");

        }

        public void CadastrarUsuario(CadastrarUsuarioDto usuarioDto) {

            ValidarUsuario(usuarioDto);
            //_usuarioRepository.Adicionar();
        }
    }
}
