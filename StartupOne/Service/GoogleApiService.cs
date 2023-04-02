using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2.Responses;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc;

namespace StartupOne.Service
{
    public class GoogleApiService
    {
        private const string ClientId = "38470441804-l13d9bvtj9o1oe87u6sunehevocuo48o.apps.googleusercontent.com";
        private const string ClientSecret = "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";
        private static readonly string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        private static readonly string CredentialFilePath = "Books.ListMyLibrary";

        public async Task<string> Auth(string user)
        {
            var credential = await GetCredential(user);
            return credential.Token.AccessToken;
        }


        public async Task<object> GetEvents(string user)
        {
            bool possuiAtuenticacaoGoogle = await new FileDataStore(CredentialFilePath, true).GetAsync<TokenResponse>(user) != null;

            if (possuiAtuenticacaoGoogle)
            {

                var credential = await GetCredential(user);

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar",
                });

                var request = service.Events.List("primary");

                request.TimeMin = DateTime.Now;

                var events = request.Execute()?.Items;

                IList<object> result = events.Select(x => new
                {
                    Summary = x.Summary,
                    Start = x.Start.DateTimeRaw,
                    End = x.End.DateTimeRaw,
                    Recurrence = x.Recurrence
                }).ToList<object>();
                return result;
            }
            return new { statusCode = StatusCodes.Status401Unauthorized, error = "Usuário não autenticado" };

        }
        private static async Task<UserCredential> GetCredential(string user)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            };

            var fileDataStore = new FileDataStore(CredentialFilePath, true);

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                Scopes,
                user,
                CancellationToken.None,
                fileDataStore);

            return credential;
        }
    }
}