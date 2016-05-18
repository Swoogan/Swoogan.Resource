using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swoogan.Resource.Url;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void Empty()
        {
            var lexer = new Lexer();
            lexer.Lex("");

            Assert.AreEqual(0, lexer.Tokens.Count);
        }

        [TestMethod]
        public void Null()
        {
            var lexer = new Lexer();
            lexer.Lex(null);

            Assert.AreEqual(0, lexer.Tokens.Count);
        }


        [TestMethod]
        public void Simple()
        {
            var lexer = new Lexer();
            lexer.Lex("/wak/");

            Assert.AreEqual(1, lexer.Tokens.Count);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[0].Type);
            Assert.AreEqual("/wak/", lexer.Tokens[0].Value);
        }

        [TestMethod]
        public void Simple_Full()
        {
            var lexer = new Lexer();
            lexer.Lex("http://localhost/wak");

            Assert.AreEqual(2, lexer.Tokens.Count);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[0].Type);
            Assert.AreEqual("http", lexer.Tokens[0].Value);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[1].Type);
            Assert.AreEqual("://localhost/wak", lexer.Tokens[1].Value);
        }


        [TestMethod]
        public void Simple_With_Port()
        {
            var lexer = new Lexer();
            lexer.Lex("http://localhost:9000");

            Assert.AreEqual(3, lexer.Tokens.Count);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[0].Type);
            Assert.AreEqual("http", lexer.Tokens[0].Value);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[1].Type);
            Assert.AreEqual("://localhost", lexer.Tokens[1].Value);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[2].Type);
            Assert.AreEqual(":9000", lexer.Tokens[2].Value);
        }

        [TestMethod]
        public void Two_Parameters()
        {
            var lexer = new Lexer();
            lexer.Lex("/wak/:userId/:orderId");
            
            Assert.AreEqual(4, lexer.Tokens.Count);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[0].Type);
            Assert.AreEqual("/wak/", lexer.Tokens[0].Value);

            Assert.AreEqual(TokenType.Parameter, lexer.Tokens[1].Type);
            Assert.AreEqual("userId", lexer.Tokens[1].Value);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[2].Type);
            Assert.AreEqual("/", lexer.Tokens[2].Value);

            Assert.AreEqual(TokenType.Parameter, lexer.Tokens[3].Type);
            Assert.AreEqual("orderId", lexer.Tokens[3].Value);
        }
        
        [TestMethod]
        public void Two_Dotted_Parameters()
        {
            var lexer = new Lexer();
            lexer.Lex("/wak/:userId.foo/:orderId.bar");

            Assert.AreEqual(5, lexer.Tokens.Count);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[0].Type);
            Assert.AreEqual("/wak/", lexer.Tokens[0].Value);

            Assert.AreEqual(TokenType.Parameter, lexer.Tokens[1].Type);
            Assert.AreEqual("userId", lexer.Tokens[1].Value);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[2].Type);
            Assert.AreEqual(".foo/", lexer.Tokens[2].Value);

            Assert.AreEqual(TokenType.Parameter, lexer.Tokens[3].Type);
            Assert.AreEqual("orderId", lexer.Tokens[3].Value);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[4].Type);
            Assert.AreEqual(".bar", lexer.Tokens[4].Value);
        }

        [TestMethod]
        public void Escaped_Backslash()
        {
            var lexer = new Lexer();
            lexer.Lex(@"/wak/\\:userId");

            Assert.AreEqual(1, lexer.Tokens.Count);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[0].Type);
            Assert.AreEqual(@"/wak/\\:userId", lexer.Tokens[0].Value);
        }

        [TestMethod]
        public void Escaped_Colon()
        {
            var lexer = new Lexer();
            lexer.Lex(@"/wak/\:userId");

            Assert.AreEqual(1, lexer.Tokens.Count);

            Assert.AreEqual(TokenType.Literal, lexer.Tokens[0].Type);
            Assert.AreEqual(@"/wak/\:userId", lexer.Tokens[0].Value);
        }
    }
}
