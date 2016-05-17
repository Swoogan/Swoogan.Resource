namespace Swoogan.Resource.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var resource = new Resource("http://workstation:8090/api");

            dynamic result = resource.Get();
            System.Console.WriteLine(result.aol.Name);
            System.Console.WriteLine(result.fow.Name);
            System.Console.WriteLine();

            var result2 = resource.Get<Buynext>();
            System.Console.WriteLine(result2.Aol.Name);
            System.Console.WriteLine(result2.Fow.Name);

            System.Console.ReadKey();
        }
    }

    class Buynext
    {
        public Land Aol { get; set; }
        public Land Fow { get; set; }
    }

    public class Land
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
