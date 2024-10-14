
namespace mvc
{
    public class View
    {
        public void Show(int Count)
        {
            Console.WriteLine("MVC");
            Console.WriteLine($"-------Likes:{Count}----------");
            Console.WriteLine("1-inc, 2-dec, 0-exit");
        }
    }
}