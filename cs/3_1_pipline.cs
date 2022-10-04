using System.Collections.Generic;
using System;

namespace Behavioral.ChainOfResponsibility
{
    class PiplineManager
    {
        private List<AbstractHandler> _handlers;
        public int maxHandlersCount;

        public PiplineManager(int maxHandlersCount = 4)
        {
            this.maxHandlersCount = maxHandlersCount;
            this._handlers = new List<AbstractHandler>(maxHandlersCount);
            for (int i = 0; i < maxHandlersCount; i++)
            {
                this._handlers.Add(null);
            }
        }

        public bool SetHandler(int index, AbstractHandler handler)
        {
            if (index >= this.maxHandlersCount || index < 0)
                return false;
            this._handlers[index] = handler;
            return true;
        }

        public bool RemoveHandler(int index)
        {
            if (index >= this.maxHandlersCount || index < 0 || this._handlers[index] == null)
                return false;
            this._handlers[index] = null;
            return true;
        }
        public object Handle(Request request)
        {
            for (int i = 0; i < this.maxHandlersCount; i++)
            {
                if (this._handlers[i] != null)
                {
                    request = this._handlers[i].Handle(request) as Request;
                    if (request == null)
                        break;
                }
            }
            return request;
        }
    }

    class PiplineLogHendler : AbstractHandler
    {
        public override object Handle(Request request)
        {
            Console.WriteLine("Log");
            Console.WriteLine(request);
            return request;
        }
    }

    class PiplineAuthorizeHendler : AbstractHandler
    {
        private bool Check(string Login, string Password)
        {
            return Login == "admin" && Password == "admin";
        }
        public override object Handle(Request request)
        {
            Console.WriteLine("Authorize");
            if (Check(request.Login, request.Password))
            {
                return request;
            }
            else
            {
                Console.WriteLine("Wrong login or password");
                return null;
            }
        }
    }
    class PiplineResponceHendler : AbstractHandler
    {
        public override object Handle(Request request)
        {
            Console.WriteLine("Responce");
            return 42;
        }
    }
    class PiplineIncHendler : AbstractHandler
    {
        public override object Handle(Request request)
        {
            Console.WriteLine("Inc Count");
            request.Count++;
            return request;
        }
    }
}