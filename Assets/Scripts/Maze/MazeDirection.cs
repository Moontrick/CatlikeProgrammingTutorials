using UnityEngine;


public enum MazeDirection
{
    North,
    East,
    South,
    West
}


public static class MazeDirections
{
    private static IntVector2[] vectors =
    {
        new IntVector2(0, 1),
        new IntVector2(1, 0),
        new IntVector2(0, -1),
        new IntVector2(-1, 0)
    };

    private static MazeDirection[] opposites =
    {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    private static Quaternion[] rotations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    public const int Count = 4;



    // Return a random direction
    public static MazeDirection RandomValue
    { get { return (MazeDirection)Random.Range(0, Count); } }

    // Converts a direction into an IntVector2
    public static IntVector2 ToIntVector2(this MazeDirection direction)
    { return vectors[(int)direction]; }

    // Get the opposite direction
    public static MazeDirection GetOpposite(this MazeDirection direction)
    { return opposites[(int)direction]; }

    // Rotate this edge to the appropriate direction rotation
    public static Quaternion ToRotation(this MazeDirection direction)
    { return rotations[(int)direction]; }

    // Get the rotation counter clockwise of the passed in direction
    public static MazeDirection GetNextClockwise(this MazeDirection direction)
    { return (MazeDirection)(((int)direction + 1) % Count); }

    // Get the rotation clockwise of the passed in direction
    public static MazeDirection GetNextCounterClockwise(this MazeDirection direction)
    { return (MazeDirection)(((int)direction + Count - 1) % Count); }


}