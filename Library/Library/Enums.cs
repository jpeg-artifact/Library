using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Genre
{
    Bildningsroman,
    Brevroman,
    Dagboksroman,
    Pikareskroman,
    Äventyrsroman,
    Kriminalroman,
    Thriller,
    Historiskroman,
    Fabel,
    Krönika,
    Legend,
    Miniatyr,
    Myt,
    Novell,
    Novellett,
    Parabel,
    Saga,
    Folksaga,
    Konstsaga,
    Skiss,
    Alexandrin,
    Canzon,
    Distikon,
    Elegi,
    Haiku,
    Sapfiskstrof,
    Sonett,
    Lustspel,
    Satyrspel,
    Musikal,
    Operett
}

[Flags]
enum Tags
{
    None = 0,
    Barn = 1,
    Ungdom = 2,
    Vuxen = 4,
}