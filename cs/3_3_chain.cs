using System;
using System.Text.Json;

namespace Behavioral.ChainOfResponsibility
{
    public class Request
    {
        public string Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Count { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now;
        public Request(string Login, string Password, string Role = "user")
        {
            this.Login = Login;
            this.Password = Password;
            this.Role = Role;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    public class Response
    {
        public Request request;
        public string Value;
    }

    public class FailedResponse : Response
    {
        public FailedResponse(string Message)
        {
            this.Value = $"Failed {Message}";
            this.request = null;
        }
    }
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        Response Handle(Request request);
    }

    class AbstractHandler : IHandler
    {
        private IHandler _nextHandler = null;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual Response Handle(Request request)
        {
            if (this._nextHandler == null)
                return new FailedResponse("no next handler");
            return this._nextHandler.Handle(request);
        }
    }


    class LogHandler : AbstractHandler
    {
        public override Response Handle(Request request)
        {
            Console.WriteLine($"Log request: {request}");
            return base.Handle(request);
        }
    }

    class AuthorizeHandler : AbstractHandler
    {
        private bool Check(string Login, string Password)
        {
            return Login == "admin" && Password == "admin";
        }
        public override Response Handle(Request request)
        {
            Console.WriteLine("Authorize");
            if (!Check(request.Login, request.Password))
            {
                Console.WriteLine("Wrong login or password");
                return new FailedResponse("Wrong login or password");
            }
            return base.Handle(request);
        }
    }
    class ResponseHandler : AbstractHandler
    {
        public override Response Handle(Request request)
        {
            Console.WriteLine("Response");
            return new Response()
            {
                Value = "42",
                request = request
            };
        }
    }

    class IncHandler : AbstractHandler
    {
        public override Response Handle(Request request)
        {
            Console.WriteLine("Inc Count");
            request.Count++;
            return base.Handle(request);
        }
    }

    class RoleHandler : AbstractHandler
    {
        public override Response Handle(Request request)
        {
            Console.WriteLine("Role check");
            if (request.Role != "admin")
                return new FailedResponse("Admin only access");
            return base.Handle(request);
        }
    }

}