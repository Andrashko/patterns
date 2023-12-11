static void TestMvc()
{
    Console.WriteLine("Running MVC...");
    var controller = new mvc.Controller( new mvc.Model(), new mvc.View());
    controller.Start();
}


static void TestMvp()
{
    Console.WriteLine("Running MVP...");
    var presenter = new mvp.Presenter(new mvp.Model(), new mvp.View());
    presenter.Start();
}


static void TestMvvm(){
    Console.WriteLine("Running MVVM...");
    var v = new mvvm.View (new mvvm.ViewModel( new mvvm.Model()));
    v.Input();
}

TestMvc();
TestMvp();
TestMvvm();