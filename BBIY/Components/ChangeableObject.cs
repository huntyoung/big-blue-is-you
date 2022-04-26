using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    public class ChangeableObject : Component
    {
        public string objectType;
        public ChangeableObject(string objectType)
        {
            this.objectType = objectType;
        }
    }
}
