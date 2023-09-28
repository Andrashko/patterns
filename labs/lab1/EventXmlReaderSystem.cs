using System.Text;
using System.Xml;

class EventXmlReaderSystem : EventReaderSystem
{
    public override List<Event> ReadFromFile(string FileName)
    {
        var events = new List<Event>();
        using (XmlTextReader reader = new XmlTextReader(FileName))
        {
            StringBuilder str = new StringBuilder();
            reader.ReadStartElement("events");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var eventReader = EventXmlReaderFabric.Create(reader.Name);
                    events.Add(eventReader.Read(reader));
                }
            }
        }
        return events;
    }
}

class EventXmlReaderFabric
{
    public static EventXmlReader Create(string type)
    {
        if (type == "event")
            return new EventXmlReader();
        if (type == "birthday-event")
            return new BirthdayEventXmlReader();
        if (type == "meeting-event")
            return new MeetingEventXmlReader();
        return null;
    }
}

class EventXmlReader
{
    public virtual Event Read(XmlTextReader reader)
    {
        return new Event()
        {
            date = DateOnly.Parse(reader.GetAttribute("date")),
            description = reader.GetAttribute("description")
        };
    }
}

class BirthdayEventXmlReader : EventXmlReader
{
    public override BirthdayEvent Read(XmlTextReader reader)
    {
        return new BirthdayEvent()
        {
            date = DateOnly.Parse(reader.GetAttribute("date")),
            description = reader.GetAttribute("description"),
            name = reader.GetAttribute("name"),
            phone = reader.GetAttribute("phone"),
        };
    }
}

class MeetingEventXmlReader : EventXmlReader
{
    public override MeetingEvent Read(XmlTextReader reader)
    {
        return new MeetingEvent()
        {
            date = DateOnly.Parse(reader.GetAttribute("date")),
            description = reader.GetAttribute("description"),
            begin = TimeOnly.Parse(reader.GetAttribute("begin")),
            end = TimeOnly.Parse(reader.GetAttribute("end")),
        };
    }
}