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

    internal List<RoomData> GetAllRoomData(int stories, int width)
    {
        List<RoomData> rooms = new();
        for (int storyIndex = stories - 1; storyIndex >= 0; storyIndex--)
        {
            GetRoomDataPerStory(width, rooms, storyIndex);
        }

        return rooms;
    }

    private void GetRoomDataPerStory(int width, List<RoomData> rooms, int storyIndex)
    {
        Random random = Random.Shared;
        int targetRoomCount = random.Next(0, (width - 2) / 3 + 1);
        for (int horizontalIndex = 0; horizontalIndex < targetRoomCount; horizontalIndex++)
        {
            GetSingleRoomData(width, rooms, storyIndex, random);
        }
    }

    private void GetSingleRoomData(int width, List<RoomData> rooms, int storyIndex, Random random)
    {
        int roomHorizontalIndex;
        do
        {
            roomHorizontalIndex = random.Next(2, width - 2);
        } 
        while (rooms.Any(x => x.StoryIndex == storyIndex && ( x.HorizontalIndex == roomHorizontalIndex || Math.Abs(x.HorizontalIndex - roomHorizontalIndex) <= 1)));

        rooms.Add(new RoomData(storyIndex, roomHorizontalIndex));
    }
}