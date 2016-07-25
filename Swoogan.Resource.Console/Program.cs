using System;

namespace Swoogan.Resource.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var resource = new StaticResource<BranchResource>("http://localhost:9000/api/v1/branch");

            var results = resource.Query();

            if (results == null)
                throw new Exception();
            //System.Console.WriteLine(result.aol.Name);
            //System.Console.WriteLine(result.fow.Name);
            //System.Console.WriteLine();

            //var result2 = resource.Get<Buynext>();
            //System.Console.WriteLine(result2.Aol.Name);
            //System.Console.WriteLine(result2.Fow.Name);

            System.Console.ReadKey();
        }
    }

    public class BranchResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
