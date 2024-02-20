using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Genre
{
    Historical_Fiction,
    Science_fiction,
    Fantasy,
    Realistic_Fiction,
    Mystery,
    Biography,
    Autobiography,
    Informational,
    Legend,
    Fable,
    Fairy_Tale,
    Myth,
    Tall_Tale,
    Drama,
    Poetry,
    Religious
}

[Flags]
enum Tags
{
    None = 0,
    Barn = 1,
    Ungdom = 2,
    Vuxen = 4,
}