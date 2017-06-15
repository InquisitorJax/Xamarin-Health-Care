﻿using Plugin.Share;
using Plugin.Share.Abstractions;
using System;

namespace Core.AppServices
{
    public interface IDevice
    {
        void OpenUri(Uri uri);

        void Share(string shareMessage, string title = null, string url = null);
    }

    public class Device : IDevice
    {
        public void OpenUri(Uri uri)
        {
            Xamarin.Forms.Device.OpenUri(uri);
        }

        public void Share(string shareMessage, string title = null, string url = null)
        {
            ShareMessage message = new ShareMessage { Text = shareMessage, Title = title, Url = url };
            ShareOptions options = new ShareOptions { ChooserTitle = "share the care" };
            CrossShare.Current.Share(message, options);
        }
    }
}