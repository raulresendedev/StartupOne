
using StartupOne.Dto.Usuario;
using StartupOne.Models;
using StartupOne.Repository;
using System.Text.RegularExpressions;

namespace StartupOne.Service
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;

        public UsuarioService(UsuarioRepository usuarioRepository, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

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

        public void ValidarLoginUsuario(LoginUsuarioDto usuarioDto)
        {
            string emailPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";


            if (string.IsNullOrWhiteSpace(usuarioDto.Email))
                throw new Exception("Preencha o Email");

            if (string.IsNullOrWhiteSpace(usuarioDto.Password))
                throw new Exception("Preencha a senha");

            if (!Regex.IsMatch(usuarioDto.Email, emailPattern))
                throw new Exception("Email inválido");

        }

        public UsuarioLogadoDto CadastrarUsuario(CadastrarUsuarioDto usuarioDto)
        {
            ValidarUsuario(usuarioDto);
            Usuario usuario = new Usuario(0, usuarioDto.Nome, usuarioDto.Email, usuarioDto.Password);

            _usuarioRepository.Adicionar(usuario);

            var token = _tokenService.GenerateJwtToken(usuario.Email, usuario.IdUsuario);

            return new UsuarioLogadoDto
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Token = token
            };
        }

        internal UsuarioLogadoDto LogarUsuario(LoginUsuarioDto usuarioDto)
        {
            ValidarLoginUsuario(usuarioDto);

            Usuario usuario = _usuarioRepository.ObterPorEmail(usuarioDto.Email);

            if (usuario == null) throw new Exception("Email não encontrado");

            if (!usuario.ValidarSenha(usuarioDto.Password)) throw new Exception("Senha incorreta");

            var token = _tokenService.GenerateJwtToken(usuario.Email, usuario.IdUsuario);

            return new UsuarioLogadoDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                Nome = usuario.Nome,
                Token = token
            };
        }
    }
}
