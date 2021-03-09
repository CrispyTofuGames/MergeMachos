using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    AndroidNotification _notification;
    AndroidNotificationChannel _channel;
    [SerializeField]
    Color _notificationColor;

    void Start()
    {
        CreateNotificationData();
        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n .Title: " + data.Notification.Title;
            msg += "\n .Body: " + data.Notification.Text;
            msg += "\n .Channel: " + data.Channel;
            AndroidNotificationCenter.CancelAllNotifications();
        };
        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        LoadNotifications();
    }
     
    public void CreateNotificationData()
    {
        _channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
            EnableVibration = true,
            EnableLights = true,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(_channel);
    }

    public void SendNotification(string notificationMessage, DateTime notificationTime)
    {
        _notification = new AndroidNotification();
        _notification.Title = "Tavern of Sins";
        _notification.Text = notificationMessage;
        _notification.SmallIcon = "icon_0";
        _notification.LargeIcon = "icon_1";
        _notification.FireTime = notificationTime;
        _notification.Color = _notificationColor;
        _notification.Style = NotificationStyle.BigTextStyle;
        AndroidNotificationCenter.SendNotification(_notification, _channel.Id);
    }

    public void LoadNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        DateTime targetDatetime;
        int morningHour = 11, eveningHour = 18;
        DateTime notificationHour = DateTime.Now.AddHours(4);

        if (notificationHour.Hour < morningHour)
        {
            //Si aún no son las 7h (11 - 4) de la mañana, programa una notificación para las 11.
            targetDatetime = new DateTime(notificationHour.Year, notificationHour.Month, notificationHour.Day, morningHour, 0, 0);
        }
        else 
        {
            if (notificationHour.Hour < eveningHour)
            {
                //Si aún no son las 14h (18 - 4) de la tarde, programa una notificación para las 18.
                targetDatetime = new DateTime(notificationHour.Year, notificationHour.Month, notificationHour.Day, eveningHour, 0, 0);
            }
            else
            {
                //Si la hora prevista sobrepasa las 18h de la tarde, programa una notificación para las 11 del dia siguiente.
                targetDatetime = new DateTime(notificationHour.Year, notificationHour.Month, notificationHour.Day, morningHour, 0, 0).AddDays(1);
            }
        }
        SendNotification(LocalizationController.GetValueByKey("NOTIFICATION_" + UnityEngine.Random.Range(0, 2)), targetDatetime); 
        DateTime now = DateTime.Now;
        DateTime dailyNotification = new DateTime(now.Year, now.Month, now.Day, eveningHour, 0, 0).AddDays(1);
        SendNotification(LocalizationController.GetValueByKey("NOTIFICATION_2"), dailyNotification);
    }
}
