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


