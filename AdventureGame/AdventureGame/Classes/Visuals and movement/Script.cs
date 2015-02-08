using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureGame
{
    class Script
    {
        string FilePath;

        public bool Active = false;

        public bool Trigger() { return false; }

        public void Update() { }

        public void Save() { }
    }
}
