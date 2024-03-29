using System.Collections.Generic;
using System;
using System.IO;

namespace Behavioral.ChainOfResponsibility
{
    class PipelineManager
    {
        private List<AbstractHandler> _handlers;
        public int maxHandlersCount;

        public PipelineManager(int maxHandlersCount = 4)
        {
            this.maxHandlersCount = maxHandlersCount;
            this._handlers = new List<AbstractHandler>(maxHandlersCount);
            for (int i = 0; i < maxHandlersCount; i++)
            {
                _handlers.Add(null);
            }
        }

        public bool SetHandler(int index, AbstractHandler handler)
        {
            if (index >= maxHandlersCount || index < 0)
                return false;
            _handlers[index] = handler;
            return true;
        }

        public bool RemoveHandler(int index)
        {
            if (index >= maxHandlersCount || index < 0 || _handlers[index] == null)
                return false;
            _handlers[index] = null;
            return true;
        }
        public Response Handle(Request request)
        {
            Response response = null;
            for (int i = 0; i < maxHandlersCount; i++)
            {
                if (_handlers[i] != null)
                {
                    response = _handlers[i].Handle(request);
                    if (response.request == null)
                        break;
                }
            }
            return response;
        }
    }

    class PipelineLogHandler : AbstractHandler
    {
        public override Response Handle(Request request)
        {
            Console.WriteLine("Log");
            Console.WriteLine(request);
            return new Response() { request = request };
        }
    }

    class PipelineAuthorizeHandler : AbstractHandler
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
            return new Response() { request = request };
        }
    }
    class PipelineResponseHandler : AbstractHandler
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
    class PipelineIncHandler : AbstractHandler
    {
        public override Response Handle(Request request)
        {
            Console.WriteLine("Inc Count");
            request.Count++;
            return new Response() { request = request };
        }
    }
}