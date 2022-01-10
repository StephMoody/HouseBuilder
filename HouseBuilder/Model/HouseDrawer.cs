using System.Text;

namespace HouseBuilder.Model;

public static class HouseDrawer
{
    internal static void DrawHouse(int stories, int requestedWidth)
    {
        int actualWidth = HouseCalculator.CalculateActualWidth(requestedWidth);
        List<RoomData> rooms = HouseCalculator.CalculateRooms(stories, requestedWidth);
        
        DrawRoof(actualWidth);
        DrawCeiling(stories, requestedWidth, rooms);
        DrawStories(stories, requestedWidth, rooms);
    }

    private static void DrawStories(int stories, int width, List<RoomData> rooms)
    {
        for (int currentStoryIndex = 0; currentStoryIndex < stories; currentStoryIndex++)
        {
            string spaces = HouseCalculator.CalculateSpacesForStory(stories, width, rooms, currentStoryIndex);
            DrawStory(stories, width, rooms, spaces, currentStoryIndex);
        }
    }
    
    private static void DrawStory(int stories, int width, List<RoomData> rooms, string spaces, int currentStoryIndex)
    {
        Console.WriteLine($"{HouseElements.VERTICAL}{spaces}{HouseElements.VERTICAL}");
        bool baseFloorReached = currentStoryIndex == stories - 1;
        if (baseFloorReached)
        {
            Console.Write(HouseElements.LOWER_CORNER_LEFT);
            for (int xIndex = 0; xIndex < width - 2; xIndex++)
            {
                DrawRoomFloor(rooms, currentStoryIndex, xIndex);
            }

            Console.WriteLine(HouseElements.LOWER_CORNER_RIGHT);
        }

        if (currentStoryIndex >= stories - 1)
            return;
        
        Console.Write(HouseElements.TEE_VERTICAL_LEFT);
        for (int xIndex = 0; xIndex < width - 2; xIndex++)
        {
            DrawRoomFloor(rooms, currentStoryIndex, xIndex);
        }

        Console.WriteLine(HouseElements.TEE_VERTICAL_RIGHT);
    }

    private static void DrawRoomFloor(List<RoomData> rooms, int currentStoryIndex, int xIndex)
    {
        bool hasRoomNextRightOnCurrentStory = rooms.Contains(new RoomData(currentStoryIndex, xIndex + 1));
        bool hasRoomNextRightOnNextStory = rooms.Contains(new RoomData(currentStoryIndex + 1, xIndex + 1));

        if (hasRoomNextRightOnCurrentStory &&
            !hasRoomNextRightOnNextStory)
        {
            Console.Write(HouseElements.TEE_HORIZONTAL_LOWER);
        }

        if (hasRoomNextRightOnCurrentStory &&
            hasRoomNextRightOnNextStory)
        {
            Console.Write(HouseElements.CROSS);
        }

        if (!hasRoomNextRightOnCurrentStory &&
            hasRoomNextRightOnNextStory)
        {
            Console.Write(HouseElements.TEE_HORIZONTAL_UPPER);
        }

        if (!hasRoomNextRightOnCurrentStory &&
            !hasRoomNextRightOnNextStory)
        {
            Console.Write(HouseElements.HORIZONTAL);
        }
    }
    
    private static void DrawCeiling(int stories, int width, List<RoomData> rList)
    {
        if (stories <= 1) 
            return;
        
        Console.Write(HouseElements.TEE_VERTICAL_LEFT);
        int ceilingStoryIndex = -1;
        for (int horizontalIndex = 0; horizontalIndex < width - 2; horizontalIndex++)
        {
            bool storyBelowHasWall = rList.Contains(new RoomData(ceilingStoryIndex + 1, horizontalIndex + 1));
            bool hasRoomNextRightOnCurrentStory = rList.Contains(new RoomData(ceilingStoryIndex, horizontalIndex + 1));
            if (hasRoomNextRightOnCurrentStory && !storyBelowHasWall)
            {
                Console.Write(HouseElements.TEE_HORIZONTAL_LOWER);
            }

            if (hasRoomNextRightOnCurrentStory && storyBelowHasWall)
            {
                Console.Write(HouseElements.CROSS);
            }

            if (!hasRoomNextRightOnCurrentStory && storyBelowHasWall)
            {
                Console.Write(HouseElements.TEE_HORIZONTAL_UPPER);
            }

            if (!hasRoomNextRightOnCurrentStory && !storyBelowHasWall)
            {
                Console.Write(HouseElements.HORIZONTAL);
            }
        }

        Console.WriteLine(HouseElements.TEE_VERTICAL_RIGHT);
    }

    private static void DrawRoof(int width)
    {
        int roofHeight = width / 2 - 1; //BUGFIX: roof was wrong
        while (roofHeight >= 0)
        {
            DrawRoofLevel(width, roofHeight);
            roofHeight--;
        }
    }

    private static void DrawRoofLevel(int width, int roofHeight)
    {
        StringBuilder sb = new StringBuilder();
        //left space before roof starts 
        for (int xIndex = 0; xIndex < roofHeight; xIndex++)
            sb.Append(" ");
        
        //left roof-part
        sb.Append("/");
        
        //space inside roof
        for (int xIndex = 0; xIndex < (width / 2 - roofHeight) * 2 - 2; xIndex++)
            sb.Append(" ");
        
        //right roof-part
        sb.Append("\\");
        
        //right space behind roof 
        for (int xIndex = 0; xIndex < roofHeight; xIndex++)
            sb.Append(" ");
        Console.WriteLine(sb.ToString());
    }
}