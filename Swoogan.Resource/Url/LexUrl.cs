using System;

namespace Swoogan.Resource.Url
{
    public class LexUrl : IState
    {
        public IState Execute(Lexer lexer)
        {
            var c = lexer.Next();

            if (!c.HasValue) // EOF
                return null;

            if (c.Value != ':') 
                return new LexLiteral();

            c = lexer.Peek();
            if (!c.HasValue || (!Char.IsLetter(c.Value) && c.Value != '_')) 
                return new LexLiteral();

            lexer.Ignore(); // Discard the ':'
            return new LexParameter();
        }
    }
}