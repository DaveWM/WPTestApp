﻿using Windows.ApplicationModel.Background;
using Windows.Storage;
using Octokit;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System;

namespace BackgroundAgent
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var token = ApplicationData.Current.LocalSettings.Values["token"] as string;
            if (token != null)
            {
                var client = new GitHubClient(new ProductHeaderValue("TestApp"));
                client.Credentials = new Credentials(token);

                var notifications = await client.Notification.GetAllForCurrent(new NotificationsRequest
                {
                    All = true
                });

                var badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeGlyph);
                var badge = badgeXml.SelectSingleNode("/badge") as XmlElement;
                badge.SetAttribute("value", notifications.Count.ToString());
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(badgeXml));
                var template = ToastTemplateType.ToastImageAndText01;
                var xml = ToastNotificationManager.GetTemplateContent(template);
                var toastTextElements = xml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(xml.CreateTextNode("\{notifications.Count} GitHub Notifications"));

                XmlElement audioNode = xml.CreateElement("audio");
                audioNode.SetAttribute("silent", "true");
                xml.SelectSingleNode("/toast").AppendChild(audioNode);

                var toast = new ToastNotification(xml);
                toast.SuppressPopup = false;
                toast.Tag = "ghNotification";
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            deferral.Complete();
        }
    }
}
