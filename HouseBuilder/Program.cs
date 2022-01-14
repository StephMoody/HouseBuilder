using HouseBuilder.Model;
using HouseBuilder.Model.Drawing;

namespace HouseBuilder 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Drawing house...");
            new HouseDrawer().DrawHouse(4,20);
            // OriginalHouseBuilder.CertainlyNotMain(null);
        }
    }
}