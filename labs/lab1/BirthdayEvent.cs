//та контакти іменинника (Ім'я, номер телефону). 
using System.Xml;
class BirthdayEvent : Event
{
    public string name;
    public string phone;
}

class BirthdayEventXmlReader : EventXmlReader
{
    public BirthdayEvent Read(XmlTextReader reader)
    {
        var date = reader.GetAttribute("date");
        var description = reader.GetAttribute("description");
        var name = reader.GetAttribute("name");
        var phone = reader.GetAttribute("phone");

        return new BirthdayEvent()
        {
            date = DateOnly.Parse(date),
            description = description
        };
    }
}