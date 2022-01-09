using HouseBuilder.Model;

namespace HouseBuilder 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Drawing house...");
            HouseDrawer.DrawHouse(4,20);
        }
    }
}