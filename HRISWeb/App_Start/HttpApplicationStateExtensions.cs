using System.Web;

namespace Unity.Web
{
    public static class HttpApplicationStateExtensions
    {
        private const string GlobalContainerKey = "EntLibContainer";

        public static IUnityContainer GetContainer(this HttpApplicationState appState)
        {
            appState.Lock();
            try
            {
                if (!(appState[GlobalContainerKey] is IUnityContainer myContainer))
                {
                    myContainer = new UnityContainer();
                    appState[GlobalContainerKey] = myContainer;
                }

                return myContainer;
            }

            finally
            {
                appState.UnLock();
            }
        }
    }
}