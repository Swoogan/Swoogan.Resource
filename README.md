#Swoogan.Resource
[![Build status](https://ci.appveyor.com/api/projects/status/q08he4xtsn33d2xx?svg=true)](https://ci.appveyor.com/project/Swoogan/swoogan-resource)

A client REST library for .NET with an API that models Angular's ngResource

## Installing

NuGet package will be available soon.

## Example Use

# Full Program w/ GET

	class Program
	{
		static void Main(string[] args)
		{
			var resource = new Resource("http://workstation:8090/api");

			dynamic products = resource.Get();
			System.Console.WriteLine(products.cup.Name);
			System.Console.WriteLine(products.saucer.Name);
			System.Console.WriteLine();

			var products2 = resource.Get<Product>();
			System.Console.WriteLine(products2.Cup.Name);
			System.Console.WriteLine(products2.Saucer.Name);

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
	
## POST

	var resource = new Resource("http://workstation:8090/products");
	
	object product = new { Name = "Cup", Price = 3.99m };
	resouce.Create(product); 	// PATCH

## PATCH
	var resource = new Resource("http://workstation:8090/products");

	dynamic product = resource.Get(1);
	product.Price = 3.99m;
	resouce.Update(product); 	// PATCH
	
	
		