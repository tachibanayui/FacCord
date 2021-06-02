using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    [NotMapped]
    public class Rect2DInt
    {
        public Rect2DInt(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect2DInt(Position2DInt pos, int width, int height) : this(pos.X, pos.Y, width, height) { }
        public Rect2DInt(Position2DInt pos, Region2DInt reg) : this(pos.X, pos.Y, reg.Width, reg.Height) { }

        private Region2DInt _Reg;
        private Position2DInt _Pos;
        private Position2DInt _EndPos;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Region2DInt Region
        {
            get
            {
                _Reg = _Reg ?? new Region2DInt(Width, Height);
                return _Reg;
            }
        }
        public Position2DInt Position
        {
            get
            {
                _Pos = _Pos ?? new Position2DInt(X, Y);
                return _Pos;
            }
        }
        public Position2DInt EndPosition
        {
            get
            {
                _EndPos = _EndPos ?? Position.Offset(Region.Width - 1, Region.Height - 1);
                return _EndPos;
            }
        }

        public bool Contain(Position2DInt pos) => Contain(pos.X, pos.Y);
        private bool Contain(int x, int y)
            => x >= X && y >= Y && x <= EndPosition.X && y <= EndPosition.Y;
    }
}
