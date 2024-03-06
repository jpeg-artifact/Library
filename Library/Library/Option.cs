using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class Option
    {
        public Option (string name, Action onSelect)
        {
            Name = name;
            OnSelect = onSelect;
        }

        public string Name { get; set; }
        public Action OnSelect { get; set; }
    }
}
