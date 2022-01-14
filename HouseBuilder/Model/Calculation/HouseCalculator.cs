using HouseBuilder.Model.Common;

namespace HouseBuilder.Model.Calculation;

internal class HouseCalculator
{
    private const int MIN_WIDTH = 4;

    internal int CalculateActualWidth(int requestedWidth)
    {
        if (requestedWidth % 2 != 0)
            requestedWidth++;
        if (requestedWidth < MIN_WIDTH)
            requestedWidth = MIN_WIDTH;
        return requestedWidth;
    }

    internal List<RoomData> CalculateRooms(int stories, int width)
    {
        List<RoomData> rooms = new();
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
    
    internal string CalculateSpacesForStory(int stories, int width, List<RoomData> rooms, int currentStoryIndex)
    {
        Random random = new();
        string spaces = "";
        bool doorIsPlaced = false;
        bool windowIsPlaced = false; //BUGFIX: Windows too close to each other
        for (int currentRoomIndex = 0; currentRoomIndex < width - 2; currentRoomIndex++)
        {
            if (rooms.Any(x => x.StoryIndex == currentStoryIndex && x.HorizontalIndex == currentRoomIndex + 1))
            {
                spaces += HouseElements.VERTICAL;
                windowIsPlaced = false;
                continue;
            }

            if (currentStoryIndex == stories - 1)
            {
                if (random.NextDouble() < 0.5 &&
                    !doorIsPlaced) //BUGFIX: House had no door. If still no door, adjust number.
                {
                    spaces += HouseElements.DOOR;
                    doorIsPlaced = true;
                }
                else
                    spaces += " ";
            }
            else
            {
                if (random.NextDouble() < 0.25 && !windowIsPlaced)
                {
                    spaces += HouseElements.WINDOW;
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
}