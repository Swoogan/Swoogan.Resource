using System;

namespace Swoogan.Resource.Url
{
    public class LexParameter : IState
    {
        public IState Execute(Lexer lexer)
        {
            var c = lexer.Next();
            if (!c.HasValue)
                throw new LexerExcepiton("':' not followed by an identifier (at end of url)");

            while (true)
            {
                c = lexer.Next();
                if (!c.HasValue)
                    break;

                if (!Char.IsLetterOrDigit(c.Value) && c.Value != '_')
                {
                    lexer.Backup();
                    break;
                }
            }

            lexer.Emit(TokenType.Parameter);

            return new LexUrl();
        }
    }
}