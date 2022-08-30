using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API_Whisperer;
using GoogleGson;

namespace Mobile_Field_App.Data
{
    public class AuthenticationManager
    {
        #region Fields

        public Authentication authentication = new Authentication();
        public System.Text.Json.JsonElement? userData;

        #endregion Fields

        #region Methods

        public async Task<bool> ConfirmGrantInStorage()
        {
            var req = new Request();

            req.method = "GET";
            req.url = "auth/user/check";

            var res = await req.Execute(authentication);
            if (res.isSuccess)
            {
                userData = res.bodyAsJson.Value;
                return true;
            }
            else
            {
                authentication.headers.Clear();
                return false;
            }
        }

        public async Task<bool> HasGrantInStorage()
        {
            var uid = await SecureStorage.Default.GetAsync("uid");
            var token = await SecureStorage.Default.GetAsync("token");

            if (uid != null && token != null)
            {
                authentication.headers.Clear();
                authentication.headers.Add("uid", uid);
                authentication.headers.Add("token", token);
                return true;
            }

            return false;
        }

        public async Task<bool> LogOut()
        {
            SecureStorage.Default.RemoveAll();
            authentication.headers.Clear();
            return true;
        }

        public async Task<bool> PerformCredentialLogin(string email, string password)
        {
            var req = new Request();

            req.method = "POST";
            req.url = "auth/user/login";
            req.headers.Add("email", email);
            req.headers.Add("password", password);
            req.headers.Add("bodyAuth", "true");

            var res = await req.Execute();
            if (res.isSuccess)
            {
                authentication.headers.Clear();
                authentication.headers.Add("uid", res.bodyAsJson.Value.GetProperty("uid").GetString());
                authentication.headers.Add("token", res.bodyAsJson.Value.GetProperty("token").GetString());
                StoreGrant();
                return true;
            }

            return false;
        }

        public async void StoreGrant()
        {
            foreach (var header in authentication.headers)
            {
                await SecureStorage.SetAsync(header.Key, header.Value);
            }
        }

        #endregion Methods
    }
}