using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class Menu
    {
        public Menu(string name, List<Option> options)
        {
            Name = name;
            Options = options;
        }

        public string Name { get; set; }
        public List<Option> Options { get; set; }
    }
}
