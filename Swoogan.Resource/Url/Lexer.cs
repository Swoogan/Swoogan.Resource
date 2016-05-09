using System.Collections.Generic;

namespace Swoogan.Resource.Url
{
    public class Lexer
    {
        public List<Token> Tokens { get; private set; }
        private int _start;
        private int _pos;

        private string _input;

        public void Emit(TokenType type)
        {
            Tokens.Add(new Token
            {
                Type = type,
                Value = Lexem(),
                Start = _start,
                End = _pos,
            });
            _start = _pos;
        }

        private string Lexem()
        {
            return _input.Substring(_start, _pos - _start);
        }

        /// <summary>
        /// Discard any consumed tokens before the current position
        /// </summary>
        public void Ignore()
        {
            _start = _pos;
        }

        /// <summary>
        /// Look at the next token without consuming it.
        /// </summary>
        public char? Peek()
        {
            var c = Next();
            Backup();
            return c;
        }

        /// <summary>
        /// Returns the previous rune in the input
        /// with out moving to it
        /// </summary>
        public char? Previous()
        {
            if (_pos <= 1)
                return null;

            return _input[_pos - 2];
        }
        
        /// <summary>
        /// Returns the next rune in the input.
        /// </summary>
        public char? Next()
        {
            if (_pos >= _input.Length)
                return null;

            return _input[_pos++];
        }

        /// <summary>
        /// backup steps back one rune.
        /// Can be called only once per call of next.
        /// </summary>
        public void Backup()
        {
            _pos -= 1;
        }

        // run lexes the input by executing state functions until
        // the state is null.
        public void Lex(string url)
        {
            _input = url;
            Tokens = new List<Token>();

            IState state = new LexUrl();

            while (state != null)
                state = state.Execute(this);
        }
    }
}