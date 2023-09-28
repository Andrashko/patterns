using System.Text;
using System.Xml;

abstract class EventReaderSystem
{
    public abstract List<Event> ReadFromFile(string FileName);


    public static EventReaderSystem Create(string fileName)
    {
        var parts = fileName.Split(".");
        if (parts[1] == "xml")
            return new EventXmlReaderSystem();
        if (parts[1] == "json")
            return new EventJsonReaderSystem();
        return null;
    }
}

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
                    var eventReader = EventXmlReader.Create(reader.Name);
                    events.Add(eventReader.Read(reader));
                }
            }
        }
        return events;
    }
}

class EventJsonReaderSystem : EventReaderSystem
{
    public override List<Event> ReadFromFile(string FileName)
    {
        return null;
    }
}