static void TestMvc()
{
    var controller = new mvc.Controller( new mvc.Model(), new mvc.View());
    controller.Start();
}


static void TestMvp()
{
    var presenter = new mvp.Presenter(new mvp.Model(), new mvp.View());
    presenter.Start();
}


static void TestMvvm(){
    var v = new mvvm.ProgramView ();
    v.Input();
}
// TestMvc();
// TestMvp();
TestMvvm();