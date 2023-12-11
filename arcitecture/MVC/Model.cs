
namespace mvc
{
    public class Model
    {
        private int _count = 0;
        public int Count
        {
            get
            {
                Console.WriteLine("SELECT [count] FROM likes;");
                return _count;
            }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Invalid value. >=0 required");
                    return;
                }
                Console.WriteLine($"UPDATE likes SET [count] = {value};");
                _count = value;
            }
        }
    }
}