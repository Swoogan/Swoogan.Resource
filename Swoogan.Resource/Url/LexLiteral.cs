namespace Swoogan.Resource.Url
{
    public class LexLiteral : IState
    {
        public IState Execute(Lexer lexer)
        {
            while (true)
            {
                var c = lexer.Next();

                if (!c.HasValue) // EOF
                    break;

                if (c.Value != ':') continue;

                // \: escapes the :
                var p = lexer.Previous();
                if (p.HasValue && p.Value == '\\') continue;
                
                lexer.Backup();
                break;
            }

            lexer.Emit(TokenType.Literal);

            return new LexUrl();
        }
    }
}