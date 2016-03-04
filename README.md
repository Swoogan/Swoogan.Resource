#Swoogan.Resource
[![Build status](https://ci.appveyor.com/api/projects/status/q08he4xtsn33d2xx?svg=true)](https://ci.appveyor.com/project/Swoogan/swoogan-resource)

A client REST library for .NET with an API that models Angular's ngResource

### Installing

NuGet package will be available soon.

### Example Use

	class Program
	{
		static void Main(string[] args)
		{
			var resource = new Resource("http://workstation:8090/api");

			dynamic result = resource.Get();
			System.Console.WriteLine(result.cup.Name);
			System.Console.WriteLine(result.saucer.Name);
			System.Console.WriteLine();

			var result2 = resource.Get<Product>();
			System.Console.WriteLine(result2.Cup.Name);
			System.Console.WriteLine(result2.Saucer.Name);

			System.Console.ReadKey();
		}
		
		class Product
		{
			public ProductDetails Cup { get; set; }
			public ProductDetails Saucer { get; set; }
		}

		public class ProductDetails
		{
			public string Name { get; set; }
			public decimal Price { get; set; }
		}
	}

		