using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    [NotMapped]
    public class Region2DInt
    {
        public Region2DInt(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
