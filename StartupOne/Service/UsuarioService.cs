using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using StartupOne.Dto.Usuario;
using StartupOne.Exceptions;
using StartupOne.Models;
using StartupOne.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace StartupOne.Service
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(IConfiguration configuration, UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }


        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GenerateJwtToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim("id", userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

            var token = GenerateJwtToken(usuario.Email, usuario.IdUsuario);

            _usuarioRepository.Adicionar(usuario);

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

            var token = GenerateJwtToken(usuario.Email, usuario.IdUsuario);

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
