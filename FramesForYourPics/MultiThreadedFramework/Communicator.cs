using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace FramesForYourPics.MultiThreadedFramework
{
    public class Communicator
    {
        private readonly Dispatcher<Dispatchable> _manager;
        private readonly Thread _runningThread;
        private readonly ConcurrentQueue<Dispatchable> _messagesQueue = new ConcurrentQueue<Dispatchable>(); 
        private readonly AutoResetEvent _notifyEvent = new AutoResetEvent(false);

        public Communicator()
        {
            //Initialize the dispatcher
            _manager = new Dispatcher<Dispatchable>();

            //Start the listening thread
            _runningThread = new Thread(StartRun);
            _runningThread.Start();

            //Register all handlers
            RegisterHandlers(_manager);
        }

        public void Stop()
        {
            _runningThread.Abort();
        }

        public void Notify(Dispatchable msg)
        {
            //Insert the message to the message queue
            _messagesQueue.Enqueue(msg);

            //Signal the waiting object
            _notifyEvent.Set();
        }


        #region Private Helpers

        protected void StartRun()
        {
            //Wait for a message to arrive
            while (_notifyEvent.WaitOne())
            {
                Dispatchable message;
                _messagesQueue.TryDequeue(out message);

                _manager.HandleMessage(message);
            }
        }

        /// <summary>
        /// registers all of the methods decorated with the 'MessageHandler' attribute to the HandlerManager
        /// </summary>
        protected void RegisterHandlers()
        {
            // By default - we supply a Message dispatcher...
            RegisterHandlers(_manager);
        }
        protected void RegisterHandlers<T>(Dispatcher<T> dispatcher) where T : Dispatchable
        {

            // get all of the methods that are decorated with the "MessageHandler" attribute (these are the methods we need to register)
            var methods = from m in GetType().GetMethods()
                          where m.IsDefined(typeof(MessageHandlerAttribute), false)
                          select m;

            // register all of the methods that we got from the query
            foreach (var m in methods)
            {
                /* we want to invoke _manager.AddHandler for each handler - but we don't know the handled message type as 
                   it will only be known during runtime, so we can't call the Generic AddHandler<> function directly - we have to use reflection. */

                // get the method's parameter (the handled message)
                var firstOrDefault = m.GetParameters().FirstOrDefault();
                if (firstOrDefault == null) continue;
                var paramType = firstOrDefault.ParameterType;

                // check if paramType is (or is derived from) baseType - if not, we will not register the function
                if (!paramType.IsSubclassOf(typeof(T)))
                    throw new BadMessageHandlerParameterException();

                // get the dispatcher's generic parameter
                var genParam = dispatcher.GetType().GetGenericArguments().FirstOrDefault();

                // construct the delegate type from the reflected method (unfortunately we have to use reflection here as well... )
                var del = GetType()
                    .GetMethod("GetDelegate")
                    .MakeGenericMethod(new[] { paramType, genParam } )
                    .Invoke(this, new object[] { m });


                // using reflection, we pass the delegate to the AddHandler method, achieving the desired result 
                _manager.GetType()
                    .GetMethod("AddHandler")
                    .MakeGenericMethod(paramType)
                    .Invoke(dispatcher, new[] { del });
            }

        }

        /// <summary>
        /// Helper method that constructs desired delegate given the MethodInfo object and the generic type.
        /// 
        /// The method is needed because (sadly) the reflection toolbox lets us 
        /// construct generic classes or mehtods - but not generic delegates... 
        /// </summary>
        public Dispatcher<T>.GenericMessageHandler<U> GetDelegate<U, T>(MethodInfo method)
            where U : T
            where T : Dispatchable
        {

            var del = method.CreateDelegate(typeof(Dispatcher<T>.GenericMessageHandler<U>), this);
            return del as Dispatcher<T>.GenericMessageHandler<U>;
        }

        #endregion
    }

}
