using HouseBuilder.Model.Calculation;
using HouseBuilder.Model.Common;

namespace HouseBuilder.Model.Drawing;

public class HouseDrawer
{
    private readonly HouseCalculator _houseCalculator  = new HouseCalculator();
    private readonly RoofDrawer _roofDrawer = new RoofDrawer();

    internal void DrawHouse(int stories, int requestedWidth)
    {
        int actualWidth = _houseCalculator.CalculateActualWidth(requestedWidth);
        List<RoomData> rooms = _houseCalculator.CalculateRooms(stories, requestedWidth);

        _roofDrawer.DrawRoof(actualWidth);
        DrawCeiling(stories, requestedWidth, rooms);
        DrawStories(stories, requestedWidth, rooms);
    }

    private void DrawStories(int stories, int width, List<RoomData> rooms)
    {
        for (int currentStoryIndex = 0; currentStoryIndex < stories; currentStoryIndex++)
        {
            string spaces = _houseCalculator.CalculateSpacesForStory(stories, width, rooms, currentStoryIndex);
            DrawStory(stories, width, rooms, spaces, currentStoryIndex);
        }
    }
    
    private void DrawStory(int stories, int width, List<RoomData> rooms, string spaces, int currentStoryIndex)
    {
        Console.WriteLine($"{HouseElements.VERTICAL}{spaces}{HouseElements.VERTICAL}");
        bool baseFloorReached = currentStoryIndex == stories - 1;
        if (baseFloorReached)
        {
            Console.Write(HouseElements.LOWER_CORNER_LEFT);
            for (int xIndex = 0; xIndex < width - 2; xIndex++)
            {
                DrawRooms(rooms, currentStoryIndex, xIndex);
            }

            Console.WriteLine(HouseElements.LOWER_CORNER_RIGHT);
        }

        if (currentStoryIndex >= stories - 1)
            return;
        
        Console.Write(HouseElements.TEE_VERTICAL_LEFT);
        for (int xIndex = 0; xIndex < width - 2; xIndex++)
        {
            DrawRooms(rooms, currentStoryIndex, xIndex);
        }

        Console.WriteLine(HouseElements.TEE_VERTICAL_RIGHT);
    }

    private void DrawRooms(List<RoomData> rooms, int currentStoryIndex, int xIndex)
    {
        bool hasRoomNextRightOnNextStory = rooms.Contains(new RoomData(currentStoryIndex + 1, xIndex + 1));
        bool hasRoomNextRightOnCurrentStory = rooms.Contains(new RoomData(currentStoryIndex, xIndex + 1));

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
    
    private void DrawCeiling(int stories, int width, List<RoomData> rooms)
    {
        if (stories <= 1) 
            return;
        
        Console.Write(HouseElements.TEE_VERTICAL_LEFT);
        int ceilingStoryIndex = -1;
        for (int horizontalIndex = 0; horizontalIndex < width - 2; horizontalIndex++)
        {
            bool storyBelowHasWall = rooms.Contains(new RoomData(ceilingStoryIndex + 1, horizontalIndex + 1));
            bool hasRoomNextRightOnCurrentStory = rooms.Contains(new RoomData(ceilingStoryIndex, horizontalIndex + 1));
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
}