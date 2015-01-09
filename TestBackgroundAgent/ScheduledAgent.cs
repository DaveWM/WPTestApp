using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System;
using Windows.Storage;
using Octokit;

namespace TestBackgroundAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                System.Windows.Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(20));

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("token"))
            {
                var token = ApplicationData.Current.LocalSettings.Values["token"].ToString();
                var client = new GitHubClient(new ProductHeaderValue("TestApp"));
                client.Credentials = new Credentials(token);
            }

            

            NotifyComplete();
        }
    }
}