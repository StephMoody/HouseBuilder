using System.Text;

namespace HouseBuilder.Model;

public static class HouseDrawer
{
    private const string HORIZONTAL = "─";
    private const string VERTICAL = "│";
    private const string LOWER_CORNER_LEFT = "└";
    private const string LOWER_CORNER_RIGHT = "┘";
    private const string TEE_HORIZONTAL_LOWER = "┴";
    private const string TEE_HORIZONTAL_UPPER = "┬";
    private const string TEE_VERTICAL_LEFT = "├";
    private const string TEE_VERTICAL_RIGHT = "┤";
    private const string CROSS = "┼";
    private const string WINDOW = "■";
    private const string DOOR = "▐";
        
    internal static void DrawHouse(int stories, int requestedWidth)
    {
        int actualWidth = CalculateActualWidth(requestedWidth);
        List<RoomData> rooms = CaclulateRooms(stories, requestedWidth);
        
        DrawRoof(actualWidth);
        DrawCeiling(stories, requestedWidth, rooms);
        DrawStories(stories, requestedWidth, rooms);
    }

    private static int CalculateActualWidth(int requestedWidth)
    {
        if (requestedWidth % 2 != 0)
            requestedWidth++;
        if (requestedWidth < 4)
            requestedWidth = 4;
        return requestedWidth;
    }

    private static void DrawStories(int stories, int width, List<RoomData> rooms)
    {
        for (int currentStoryIndex = 0; currentStoryIndex < stories; currentStoryIndex++)
        {
            string spaces = CalculateSpacesForStory(stories, width, rooms, currentStoryIndex);
            DrawStory(stories, width, rooms, spaces, currentStoryIndex);
        }
    }

    private static string CalculateSpacesForStory(int stories, int width, List<RoomData> rooms, int currentStoryIndex)
    {
        Random random = new();
        string spaces = "";
        bool doorIsPlaced = false;
        bool windowIsPlaced = false; //BUGFIX: Windows too close to each other
        for (int currentRoomIndex = 0; currentRoomIndex < width - 2; currentRoomIndex++)
        {
            if (rooms.Any(x => x.StoryIndex == currentStoryIndex && x.HorizontalIndex == currentRoomIndex + 1))
            {
                spaces += VERTICAL;
                windowIsPlaced = false;
                continue;
            }

            if (currentStoryIndex == stories - 1)
            {
                if (random.NextDouble() < 0.5 &&
                    !doorIsPlaced) //BUGFIX: House had no door. If still no door, adjust number.
                {
                    spaces += DOOR;
                    doorIsPlaced = true;
                }
                else
                    spaces += " ";
            }
            else
            {
                if (random.NextDouble() < 0.25 && !windowIsPlaced)
                {
                    spaces += WINDOW;
                    windowIsPlaced = true;
                }
                else
                {
                    spaces += " ";
                    windowIsPlaced = false;
                }
            }
        }

        return spaces;
    }

    private static void DrawStory(int stories, int width, List<RoomData> rooms, string spaces, int currentStoryIndex)
    {
        Console.WriteLine($"{VERTICAL}{spaces}{VERTICAL}");
        if (currentStoryIndex == stories - 1)
        {
            Console.Write(LOWER_CORNER_LEFT);
            for (int xIndex = 0; xIndex < width - 2; xIndex++)
            {
                DrawRoomOfBaseLevel(rooms, currentStoryIndex, xIndex);
            }

            Console.WriteLine(LOWER_CORNER_RIGHT);
        }

        if (currentStoryIndex < stories - 1)
        {
            Console.Write(TEE_VERTICAL_LEFT);
            for (int xIndex = 0; xIndex < width - 2; xIndex++)
            {
                DrawRoomOfSubLevel(rooms, currentStoryIndex, xIndex);
            }

            Console.WriteLine(TEE_VERTICAL_RIGHT);
        }
    }

    private static void DrawRoomOfSubLevel(List<RoomData> rooms, int currentStoryIndex, int xIndex)
    {
        bool hasRoomNextRightOnCurrentStory = rooms.Contains(new RoomData(currentStoryIndex, xIndex + 1));
        bool hasRoomNextRightOnNextStory = rooms.Contains(new RoomData(currentStoryIndex + 1, xIndex + 1));

        if (hasRoomNextRightOnCurrentStory &&
            !hasRoomNextRightOnNextStory)
        {
            Console.Write(TEE_HORIZONTAL_LOWER);
        }

        if (hasRoomNextRightOnCurrentStory &&
            hasRoomNextRightOnNextStory)
        {
            Console.Write(CROSS);
        }

        if (!hasRoomNextRightOnCurrentStory &&
            hasRoomNextRightOnNextStory)
        {
            Console.Write(TEE_HORIZONTAL_UPPER);
        }

        if (!hasRoomNextRightOnCurrentStory &&
            !hasRoomNextRightOnNextStory)
        {
            Console.Write(HORIZONTAL);
        }
    }

    private static void DrawRoomOfBaseLevel(List<RoomData> rooms, int currentStoryIndex, int xIndex)
    {
        bool hasRoomNextRightOnCurrentStory = rooms.Contains(new RoomData(currentStoryIndex, xIndex + 1));
        bool hasRoomNextRightOnNextStory = rooms.Contains(new RoomData(currentStoryIndex + 1, xIndex + 1));

        if (hasRoomNextRightOnCurrentStory &&
            !hasRoomNextRightOnNextStory)
        {
            Console.Write(TEE_HORIZONTAL_LOWER);
        }

        if (hasRoomNextRightOnCurrentStory &&
            hasRoomNextRightOnNextStory)
        {
            Console.Write(CROSS);
        }

        if (!hasRoomNextRightOnCurrentStory &&
            hasRoomNextRightOnNextStory)
        {
            Console.Write(TEE_HORIZONTAL_UPPER);
        }

        if (!hasRoomNextRightOnCurrentStory &&
            !hasRoomNextRightOnNextStory)
        {
            Console.Write(HORIZONTAL);
        }
    }

    private static void DrawCeiling(int stories, int width, List<RoomData> rList)
    {
        if (stories <= 1) 
            return;
        
        Console.Write(TEE_VERTICAL_LEFT);
        int ceilingStoryIndex = -1;
        for (int horizontalIndex = 0; horizontalIndex < width - 2; horizontalIndex++)
        {
            bool storyBelowHasWall = rList.Contains(new RoomData(ceilingStoryIndex + 1, horizontalIndex + 1));
            bool hasRoomNextRightOnCurrentStory = rList.Contains(new RoomData(ceilingStoryIndex, horizontalIndex + 1));
            if (hasRoomNextRightOnCurrentStory && !storyBelowHasWall)
            {
                Console.Write(TEE_HORIZONTAL_LOWER);
            }

            if (hasRoomNextRightOnCurrentStory && storyBelowHasWall)
            {
                Console.Write(CROSS);
            }

            if (!hasRoomNextRightOnCurrentStory && storyBelowHasWall)
            {
                Console.Write(TEE_HORIZONTAL_UPPER);
            }

            if (!hasRoomNextRightOnCurrentStory && !storyBelowHasWall)
            {
                Console.Write(HORIZONTAL);
            }
        }

        Console.WriteLine(TEE_VERTICAL_RIGHT);
    }

    private struct RoomData
    {
        public RoomData(int storyIndex, int horizontalIndex)
        {
            StoryIndex = storyIndex;
            HorizontalIndex = horizontalIndex;
        }

        public int StoryIndex { get; }

        public int HorizontalIndex { get; }
    }

    private static List<RoomData> CaclulateRooms(int stories, int width)
    {
        List<RoomData> rooms = new List<RoomData>();
        for (int storyIndex = stories - 1; storyIndex >= 0; storyIndex--)
        {
            Random random = Random.Shared;
            int targetRoomCount = random.Next(0, (width - 2) / 3 + 1);
            for (int horizontalIndex = 0; horizontalIndex < targetRoomCount; horizontalIndex++)
            {
                int roomHorizontalIndex;
                do
                {
                    roomHorizontalIndex = random.Next(2, width - 2);
                } 
                while (rooms.Where(x => x.StoryIndex == storyIndex).Where(x => x.HorizontalIndex == roomHorizontalIndex)
                           .Concat(rooms.Where(x => x.StoryIndex == storyIndex && Math.Abs(x.HorizontalIndex - roomHorizontalIndex) <= 1)).Count() != 0);
                rooms.Add(new RoomData(storyIndex, roomHorizontalIndex));
            }
        }

        return rooms;
    }

    private static void DrawRoof(int width)
    {
        int spaceCount = width / 2 - 1; //BUGFIX: roof was wrong
        while (spaceCount >= 0)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spaceCount; i++)
                sb.Append(" ");
            sb.Append("/");
            for (int i = 0; i < (width / 2 - spaceCount) * 2 - 2; i++)
                sb.Append(" ");
            sb.Append("\\");
            for (int i = 0; i < spaceCount; i++)
                sb.Append(" ");
            Console.WriteLine(sb.ToString());
            spaceCount--;
        }
    }
}