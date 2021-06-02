using System;
using System.Collections.Generic;
using System.Text;

namespace TempProject
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TestGalleryAttribute : Attribute
    {
        public TestGalleryAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string LastModified { get; set; }
    }
}
