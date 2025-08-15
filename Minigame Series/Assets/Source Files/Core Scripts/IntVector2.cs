using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use this struct for grid positioning/editing/checking.
[System.Serializable]
public struct IntVector2
{
    public int x;
    public int y;

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x + b.x, a.y + b.y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x - b.x, a.y - b.y);
    }

    public static IntVector2 operator *(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x * b.x, a.y * b.y);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    // Override !=
    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return !(a == b);
    }

    // Override Equals(object)
    public override bool Equals(object obj)
    {
        if (!(obj is IntVector2)) return false;
        IntVector2 other = (IntVector2)obj;
        return this == other; // reuse your operator
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return base.GetHashCode();
        throw new System.NotImplementedException();
    }

}
