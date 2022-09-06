using System;
using System.Text.Json;

namespace Behavioral.ChainOfResponsibility{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        
        object Handle(object request);
    }

    class AbstractHandler : IHandler
    {
        private IHandler _nextHandler = null;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }
        
        public virtual object Handle(object request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }

    class Request
    {
        public String Login {get;set;}
        public String Password {get;set;}
         public DateTime Created {get;set;} =  DateTime.Now;
        public Request(string Login, string Password){
            this.Login = Login;
            this.Password = Password;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    class LogHendler: AbstractHandler
    {
        public override object Handle(object request)
        {
            Console.WriteLine("Log");
            Console.WriteLine(request);
            return base.Handle(request);
        }  
    }

    class AuthorizeHendler: AbstractHandler
    {
        private bool Check(string Login, string Password){
            return Login == "admin" && Password == "admin";
        }
        public override object Handle(object request)
        {
            Console.WriteLine("Authorize");
            Request req = request as Request;
            if (Check(req.Login, req.Password))
            {
                return base.Handle(request);
            }
            else
            {
                Console.WriteLine("Wrong login or password");
                return null;
            }
        }  
    }
    class ResponceHendler: AbstractHandler
    {   public override object Handle(object request)
        {
            Console.WriteLine("Responce");
            return 42;
        }  
    }
}