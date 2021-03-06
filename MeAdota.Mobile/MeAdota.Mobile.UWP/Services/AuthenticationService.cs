﻿using MeAdota.Mobile.Helpers;
using MeAdota.Mobile.Interfaces;
using MeAdota.Mobile.UWP.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]
namespace MeAdota.Mobile.UWP.Services
{
    public class AuthenticationService : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(provider);

                Settings.FacebookAuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.FacebookUserId = user?.UserId ?? string.Empty;

                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public void RegisterPush() { }

        public async Task Logout()
        {
            await Mobile.App.MobileService.LoginAsync();
        }

        public async Task Logout(MobileServiceClient client)
        {
            await client.LogoutAsync();
        }
    }
}
