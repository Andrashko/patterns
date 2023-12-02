
namespace mvp
{
    public class View
    {
        public Presenter presenter;
        public void Show(int Likes)
        {
            Console.WriteLine($"-------Likes:{Likes}----------");
            Console.WriteLine("1-inc, 2-dec, 0 -exit");
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                presenter.Inc();
            }
            if (choice == "2")
            {
                presenter.Dec();
            }
            if (choice == "0")
            {
                presenter.Exit();
            }
        }
    }
}