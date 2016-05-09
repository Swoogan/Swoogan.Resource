namespace Swoogan.Resource.Url
{
    public interface IState
    {
        IState Execute(Lexer lexer);
    }
}