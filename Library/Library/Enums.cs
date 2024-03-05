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
    Baby = 1,
    Kids = 2,
    Youth = 4,
    Adult = 8,
    Senior = 16,
}