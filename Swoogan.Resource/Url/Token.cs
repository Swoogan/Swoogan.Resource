using System.Diagnostics;

namespace Swoogan.Resource.Url
{
    [DebuggerDisplay("{Type}: {Value}")]
    public class Token
    {
        public TokenType Type { get; set; } // Type, such as itemNumber.
        public string Value { get; set; } // Value, such as "23.2".
        public int Start { get; set; } // Position the token starts
        public int End { get; set; } // Position the token ends
    }
}