using System;
using System.Collections.Generic;

namespace FramesForYourPics.MultiThreadedFramework
{
    // 
    /// <summary>
    /// the base class for dispatching Dispatchable objects (e.g. Messages).
    /// 
    /// Note:
    /// The class is single threaded but can be easily extended to provide multithreaded hendler invocation.
    /// 
    /// </summary>
    /// <typeparam name="T">The base type for the dispachble objects handled by the Dispatcher (T must extend the abstract Dispatchable class)</typeparam>
    public class Dispatcher<T> where T : Dispatchable
    {
        public delegate void GenericMessageHandler<in TU>(TU msg) where TU : T;

        private delegate void MessageHandler(object msg);
        private readonly IDictionary<Type, MessageHandler> _handlers;

        public Dispatcher()
        {
            _handlers = new Dictionary<Type, MessageHandler>();
        }

        public void AddHandler<TU>(GenericMessageHandler<TU> handler) where TU : T
        {
            // we allow a single handler per message type, TODO: decide if it should be extended or configured in the base class
            if (_handlers.ContainsKey(typeof(TU)))
                throw new MsgTypeAlreadyRegisteredException();

            // decorate the handler - only needed when multithreading (we can only pass an 'object' type to a ParametrizedThreadStart delegate).
            MessageHandler decoratedHandler = m => handler.Invoke(m as TU);

            // add the handler to the dictionary
            _handlers.Add(typeof(TU), decoratedHandler);
        }

        public virtual void HandleMessage(T msg)
        {
            // get the message type
            var t = msg.GetType();

            // check that the message has a registered handler
            if (!_handlers.ContainsKey(t))
                throw new MsgTypeNotRecognizedException();

            // invoke the handler (TODO: single threaded for now)
            _handlers[t].Invoke(msg);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class MessageHandlerAttribute : Attribute
    {


    }

}

