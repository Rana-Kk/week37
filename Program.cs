public class Product(string? category, string name, double price)
{
    public string Category { get; set; } = category ?? "";
    public string Name { get; set; } = name;
    public double Price { get; set; } = price;
}
public class ProductManager
{
    private readonly List<Product> _products = new List<Product>();
    public void AddProduct()
    {
        Formatter.WriteMessage("Enter Category: ");
        string? category = Console.ReadLine()?.Trim();

        Formatter.WriteMessage("Enter Product Name: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        while (string.IsNullOrWhiteSpace(name))
        {
            Formatter.WriteError("\nProduct name cannot be empty. Please enter a valid name.");
            Formatter.WriteMessage("Enter Product Name: ");
            name = Console.ReadLine()?.Trim() ?? "";
        }

        Formatter.WriteMessage("Enter Price (in kr): ");
        string? priceInput = Console.ReadLine()?.Trim();
        double price = double.TryParse(priceInput, out price) ? price : -1;
        while (price <= 0)
        {
            Formatter.WriteError("Invalid price. Please enter a valid positive number.");
            Formatter.WriteMessage("Enter Price (in kr): ");
            priceInput = Console.ReadLine()?.Trim();
            price = double.TryParse(priceInput, out price) ? price : -1;
        }

        _products.Add(new Product(category, name, price));
        Formatter.WriteInfo($"Product '{name}' added successfully.");
    }
    public void ShowProducts()
    {
        if (_products.Count == 0)
        {
            Formatter.WriteMessage("No products added yet.");
        }
        else
        {
            Formatter.WriteInfo($"\n{new string('=', 14)} PRODUCT LIST {new string('=', 14)}");

            var sortedProducts = _products.OrderBy(p => p.Price).ToList();

            foreach (var product in sortedProducts)
            {
                Formatter.WriteInfo($"{product.Category.PadRight(15)} | {product.Name.PadRight(15)} | {product.Price}kr");
            }

            Formatter.WriteInfo(new string('=', 42));
            Formatter.WriteInfo($"TOTAL PRICE: {CalculateTotal()}kr");
            Formatter.WriteInfo(new string('=', 42) + "\n");
        }
    }

    public void SearchProducts()
    {
        Formatter.WriteMessage("Enter Category or Name: ");
        string? query = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(query))
        {
            Formatter.WriteError("Search query cannot be empty.");
            return;
        }
        {
            var matches = _products.Where(p => p.Name.Equals(query, StringComparison.OrdinalIgnoreCase) ||
                          p.Category.Equals(query, StringComparison.OrdinalIgnoreCase)).ToList();
            Formatter.WriteInfo($"{new string('=', 13)} SEARCH RESULTS {new string('=', 13)}");
            if (!matches.Any())
            {
                Formatter.WriteError("No matching products found.");
                return;
            }
            foreach (var product in matches)
            {
                Formatter.WriteInfo($"{product.Category.PadRight(15)} | {product.Name.PadRight(15)} | {product.Price}kr (MATCH)");
            }
        }
    }
    public double CalculateTotal()
    {
        return _products.Sum(p => p.Price);
    }

}

class Program
{
    static void Main()
    {
        ProductManager manager = new ProductManager();
        Console.WriteLine("*** PRODUCT LIST APPLICATION ***\n");

        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. Show Products");
            Console.WriteLine("3. Search Products");
            Console.WriteLine("4. Quit");
            string? choice = Console.ReadKey().KeyChar.ToString();
            Formatter.WriteMessage(" selected");

            switch (choice)
            {
                case "1":
                    manager.AddProduct();
                    break;
                case "2":
                    manager.ShowProducts();
                    break;
                case "3":
                    manager.SearchProducts();
                    break;
                case "4":
                    Formatter.WriteInfo("Exiting the application...");
                    return;
                default:
                    Formatter.WriteError("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }
}

public class Formatter
{
    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void WriteInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void WriteMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}