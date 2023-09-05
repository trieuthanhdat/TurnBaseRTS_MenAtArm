

using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;
    
    public GridPosition(int w, int h)
    {
        this.x = w;
        this.z = h;
    }
    public override string ToString()
    {
        return $"x: {x}; z: {z}";
    }
    public static bool operator == (GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }
    public static bool operator != (GridPosition a, GridPosition b)
    {
        return !(a == b);
    }
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
                      x == position.x       &&
                      z == position.z;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }
    public static GridPosition operator + (GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    public static GridPosition operator - (GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}
