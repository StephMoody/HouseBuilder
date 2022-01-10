namespace HouseBuilder.Model;


internal struct RoomData
{
    public RoomData(int storyIndex, int horizontalIndex)
    {
        StoryIndex = storyIndex;
        HorizontalIndex = horizontalIndex;
    }

    public int StoryIndex { get; }

    public int HorizontalIndex { get; }
}

