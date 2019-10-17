using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganzenbord
{
    class Player
    {
        public string name;
        public int location;

        public Player(string nameInput)
        {
            name = nameInput;
            location = 0;
        }
    }
}
