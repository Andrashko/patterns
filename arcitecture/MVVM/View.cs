namespace mvvm
{
    public class ProgramView : Framework
    {
        private readonly PersonViewModel viewModel = new PersonViewModel();
        public ProgramView()
        {
            DataContext = viewModel;
            SetBinding("Count");
        }

        public void Input()
        {
            Console.WriteLine($"-------Likes:{viewModel.Count}----------");
            Console.WriteLine("1-inc, 2-dec, 0 -exit");
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
                Console.WriteLine($"-------Likes:{viewModel.Count}----------");
                Console.WriteLine("1-inc, 2-dec, 0 -exit");
            };
        }
    }
}