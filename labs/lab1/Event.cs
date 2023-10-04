/*
даних про події (дата події, опис) 
*/

class Event
{
    public DateOnly date;
    public string description;

    public override string ToString()
    {
        return $"{date}: {description}";
    }
}
