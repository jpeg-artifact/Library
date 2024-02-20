using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Genre
{
    Fiction,
    Nonfiction,
    Folklore,
    Drama,
    Poetry
}

[Flags]
enum Tags
{
    None = 0,
    Barn = 1,
    Ungdom = 2,
    Vuxen = 4,
}