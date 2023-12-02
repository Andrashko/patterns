namespace mvp
{
    public class Presenter
    {
        private Model model;
        private View view;
        public Presenter(Model model, View view)
        {
            this.model = model;
            this.view = view;
            view.presenter = this;
        }

         public void Inc()
        {
            model.Count++;
            view.Show(model.Count);
        }
        public void Dec()
        {
            model.Count--;
            view.Show(model.Count);
        }

        public void Start()
        {
            view.Show(model.Count);
        }

        public void Exit()
        {

        }
    }
}