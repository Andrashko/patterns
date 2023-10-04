//та контакти іменинника (Ім'я, номер телефону). 
class BirthdayEvent : Event
{
    public string name;
    public string phone;

    public override string ToString()
    {
        return base.ToString() + $" name: {name} phone : {phone}";
    }
}

