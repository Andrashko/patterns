/*
даних про події (дата події, опис) 
*/

using System.Runtime.InteropServices;
using System.Xml;
class Event
{
    public DateOnly date;
    public string description;


    public override string ToString()
    {
        return $"{date}: {description}";
    }
}
