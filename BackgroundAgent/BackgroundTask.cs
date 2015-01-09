using Windows.ApplicationModel.Background;
using Windows.Storage;
using Octokit;

namespace BackgroundAgent
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            var token = ApplicationData.Current.LocalSettings.Values["token"].ToString();
            var client = new GitHubClient(new ProductHeaderValue("TestApp"));
            client.Credentials = new Credentials(token);

            var notifications = client.Notification.GetAllForCurrent(new NotificationsRequest
            {
                All = true
            });
            deferral.Complete();
        }
    }
}
