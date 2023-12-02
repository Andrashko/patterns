
namespace mvc
{
    public class View
    {
        public void Show(Model model)
        {
            Console.WriteLine($"-------Likes:{model.Count}----------");
            Console.WriteLine("1-inc, 2-dec, 0 -exit");
        }
    }
}