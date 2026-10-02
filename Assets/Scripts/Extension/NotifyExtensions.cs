using System;

namespace Game
{
    public static class NotifyExtensions
    {
        public static INotify<T> Bind<T>(this INotify<T> notify, Action<T> action)
        {
            notify.OnChanged -= action;
            notify.OnChanged += action;
            return notify;
        }

        public static void UnBind<T>(this INotify<T> notify, Action<T> action)
        {
            notify.OnChanged -= action;
        }
    }
}