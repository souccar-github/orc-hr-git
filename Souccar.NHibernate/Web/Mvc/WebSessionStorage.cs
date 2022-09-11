namespace Souccar.NHibernate.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    using global::NHibernate;
    using System.Threading;

    public class WebSessionStorage : ISessionStorage
    {
        static ThreadLocal<SimpleSessionStorage> threadLocalSessionStorage;
        private const string HttpContextSessionStorageKey = "HttpContextSessionStorageKey";

        public WebSessionStorage(HttpApplication app)
        {
            app.EndRequest += Application_EndRequest;
        }

        public IEnumerable<ISession> GetAllSessions()
        {
            var storage = GetSimpleSessionStorage();
            return storage.GetAllSessions();
        }

        public ISession GetSessionForKey(string factoryKey)
        {
            var storage = GetSimpleSessionStorage();
            return storage.GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            var storage = GetSimpleSessionStorage();
            storage.SetSessionForKey(factoryKey, session);
        }

        private static void Application_EndRequest(object sender, EventArgs e)
        {
            NHibernateSession.CloseAllSessions();

            var context = HttpContext.Current;
            context.Items.Remove(HttpContextSessionStorageKey);
        }

        private static SimpleSessionStorage GetSimpleSessionStorage()
        {
            HttpContext context = HttpContext.Current;
            SimpleSessionStorage storage;
            if (context != null)
            {
                storage = context.Items[HttpContextSessionStorageKey] as SimpleSessionStorage;
                if (storage == null)
                {
                    storage = new SimpleSessionStorage();
                    context.Items[HttpContextSessionStorageKey] = storage;
                }
            }
            else
            {
                if (threadLocalSessionStorage == null)
                    threadLocalSessionStorage = new ThreadLocal<SimpleSessionStorage>(() => new SimpleSessionStorage());
                storage = threadLocalSessionStorage.Value;
            }
            return storage;
        }
    }
}