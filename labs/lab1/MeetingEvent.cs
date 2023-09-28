//  Зустріч, яка має час початку, час завершення

class MeetingEvent : Event
{
    public TimeOnly begin;
    public TimeOnly end;

    public override string ToString()
    {
        return base.ToString() + $" [{begin} - {end}]";
    }
}