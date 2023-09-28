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

class EventXmlReader
{
    public Event Read(XmlTextReader reader)
    {
        var date = reader.GetAttribute("date");
        var description = reader.GetAttribute("description");

        return new Event()
        {
            date = DateOnly.Parse(date),
            description = description
        };
    }

    public static EventXmlReader Create(string type)
    {
        if (type == "event")
            return new EventXmlReader();
        if (type == "birthday-event")
            return new BirthdayEventXmlReader();
        return null;
    }
}