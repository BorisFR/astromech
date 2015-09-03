using AstroBuilders.Win;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toasts.Forms.Plugin.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastNotificatorImplementation))]

namespace AstroBuilders.Win
{
    public class ToastNotificatorImplementation : IToastNotificator
    {
        public static void Init()
        { }

        void IToastNotificator.HideAll()
        {
            
        }

        Task<bool> IToastNotificator.Notify(ToastNotificationType type, string title, string description, TimeSpan duration, object context)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            taskCompletionSource.TrySetResult(true);
            return taskCompletionSource.Task;
        }
    }
}