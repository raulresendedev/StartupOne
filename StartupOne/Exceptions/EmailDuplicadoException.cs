namespace StartupOne.Exceptions
{
    public class EmailDuplicadoException : Exception
    {
        public EmailDuplicadoException() : base("O e-mail fornecido já está em uso.") { }
    }

}
