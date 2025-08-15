using UnityEngine;
namespace NoughtsAndCrosses
{
    public static class Directions
    {
        public static Vector2Int GetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Vector2Int(-1, 0);
                case Direction.NorthEast:
                    return new Vector2Int(-1, 1);
                case Direction.East:
                    return new Vector2Int(0, 1);
                case Direction.SouthEast:
                    return new Vector2Int(1, 1);
                case Direction.South:
                    return new Vector2Int(1, 0);
                case Direction.SouthWest:
                    return new Vector2Int(1, -1);
                case Direction.West:
                    return new Vector2Int(0, -1);
                case Direction.NorthWest:
                    return new Vector2Int(-1, -1);
            }

            Debug.Log("Return Direction 0,0");
            return new Vector2Int();
        }

    }

    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }
}