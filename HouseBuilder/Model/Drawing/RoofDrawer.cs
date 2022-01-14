using System.Text;

namespace HouseBuilder.Model.Drawing;

public class RoofDrawer
{
    private const string SPACE = " ";
    private const string LEFT_ROOF = "/";
    private const string RIGHT_ROOF = "\\";

    public void DrawRoof(int width)
    {
        int roofHeight = width / 2 - 1; //BUGFIX: roof was wrong
        while (roofHeight >= 0)
        {
            DrawRoofLevel(width, roofHeight);
            roofHeight--;
        }
    }

    private void DrawRoofLevel(int width, int roofHeight)
    {
        StringBuilder sb = new StringBuilder();
        //left space before roof starts 
        DrawSpace(roofHeight, width ,sb);
        
        //left roof-part
        sb.Append(LEFT_ROOF);
        
        //space inside roof
        DrawSpace(roofHeight, width,sb, true);
        
        //right roof-part
        sb.Append(RIGHT_ROOF);
        
        //right space behind roof 
        DrawSpace(roofHeight, width,sb);
        Console.WriteLine(sb.ToString());
    }

    private static void DrawSpace(int roofHeight, int width, StringBuilder sb, bool isInside = false)
    {
        if (isInside)
        {
            for (int xIndex = 0; xIndex < (width / 2 - roofHeight) * 2 - 2; xIndex++)
                sb.Append(SPACE);
        }
        else
        {
            for (int xIndex = 0; xIndex < roofHeight; xIndex++)
                sb.Append(SPACE);
        }
    }
}