namespace mvvm
{
    public class View : Framework
    {
        private readonly ViewModel viewModel;
        public View(ViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = viewModel;
            SetBinding("Count");
        }

        public void Input()
        {
            Console.WriteLine($"-------Likes:{ValueOf("Count")}----------");
            Console.WriteLine("1-inc, 2-dec, 0-exit");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "1")
                {
                    viewModel.Count++;
                }
                if (input == "2")
                {
                    viewModel.Count--;
                }
                if (input == "0")
                {
                    break;
                }
                Console.WriteLine($"-------Likes:{ValueOf("Count")}----------");
                Console.WriteLine("1-inc, 2-dec, 0-exit");
            };
        }
    }
}