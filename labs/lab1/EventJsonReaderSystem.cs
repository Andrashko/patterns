using System.Text.Json;
using System.Text.RegularExpressions;

class EventJsonReaderSystem : EventReaderSystem
{
    public override List<Event> ReadFromFile(string FileName)
    {
        var events = new List<Event>();
        using (StreamReader r = new StreamReader(FileName))
        {
            var json = r.ReadToEnd();
            json = json.Substring(1, json.Length - 2);
            var parts = json.Split("},");
            foreach (var part in parts)
            {
                events.Add(
                    EventJsonFabric.Create(part + "}")
                );
            }
        }
        return events;
    }
}


class EventJsonFabric
{
    public static Event? Create(string data)
    {
        var attributes = data
            .Split(",")
            .Select(
                attr => attr.Split(":")
            )
            .ToDictionary(
                attr => Regex.Match(attr[0], "\"(.+)\"").Groups[1].Value,
                attr => Regex.Match(attr[1], "\"(.+)\"").Groups[1].Value
            );
        EventJsonReader reader = null;
        if (attributes["type"] == "event")
            reader = new EventJsonReader();
        if (attributes["type"] == "birthday-event")
            reader = new BirthdayEventJsonReader();
        if (attributes["type"] == "meeting-event")
            reader = new MeetingEventJsonReader();

        return reader.Read(attributes);
        // if (type == "birthday-event")
        //     return new BirthdayEventXmlReader();
        // if (type == "meeting-event")
        //     return new MeetingEventXmlReader();
        return null;
    }
}

class EventJsonReader
{
    public virtual Event Read(Dictionary<string, string> attributes)
    {
        return new Event()
        {
            description = attributes["description"],
            date = DateOnly.Parse(attributes["date"])
        };
    }
}

class BirthdayEventJsonReader : EventJsonReader
{
    public override BirthdayEvent Read(Dictionary<string, string> attributes)
    {
        return new BirthdayEvent()
        {
            description = attributes["description"],
            date = DateOnly.Parse(attributes["date"]),
            name = attributes["name"],
            phone = attributes["phone"],
        };
    }
}

class MeetingEventJsonReader : EventJsonReader
{
    public override MeetingEvent Read(Dictionary<string, string> attributes)
    {
        return new MeetingEvent()
        {
            description = attributes["description"],
            date = DateOnly.Parse(attributes["date"]),
            begin = TimeOnly.Parse(attributes["begin"].Replace(".", ":")),
            end = TimeOnly.Parse(attributes["end"].Replace(".", ":")),
        };
    }
}