using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    [NotMapped]
    public class Position2DInt
    {
        public Position2DInt(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Position2DInt Offset(int x, int y)
        {
            return new Position2DInt(X + x, Y + y);
        }

        public Position2DInt Offset(Direction d, int mag)
        {
            switch (d)
            {
                case Direction.North:
                    return Offset(0, -mag);
                case Direction.East:
                    return Offset(mag, 0);
                case Direction.South:
                    return Offset(0, mag);
                case Direction.West:
                    return Offset(-mag, 0);
                default:
                    return this;
            }
        }

        public static bool operator ==(Position2DInt left, Position2DInt right)
            => CheckEquality(left, right);

        public static bool operator !=(Position2DInt left, Position2DInt right)
            => !CheckEquality(left, right);

        public override bool Equals(object obj) 
            => obj is Position2DInt other ? CheckEquality(this, other) : false;

        public override int GetHashCode() => X ^ Y;


        public static bool CheckEquality(Position2DInt left, Position2DInt right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            else if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            else
                return left.X == right.X && left.Y == right.Y;
        }
    }
}
