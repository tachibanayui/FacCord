using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord.Models
{
    public class ValueEventArgs<T> : EventArgs
    {
        public T Value { get; protected set; }

        public ValueEventArgs(T value)
        {
            Value = value;
        }
    }
}
