namespace mvc
{
    public class Controller
    {
        private View view;
        private Model model;
        public Controller(Model model, View view)
        {
            this.view = view;
            this.model = model;
        }
        public void Inc()
        {
            model.Count++;
        }
        public void Dec()
        {
            model.Count--;
        }

        public void Start()
        {
            view.Show(model.Count);
            while (true)
            {
                var choice = Console.ReadLine();
                if (choice == "1")
                {
                    Inc();
                }
                if (choice == "2")
                {
                    Dec();
                }
                if (choice == "0")
                {
                    Exit();
                    break;
                }
                view.Show(model.Count);
            }
        }

        public void Exit()
        {

        }
    }
}