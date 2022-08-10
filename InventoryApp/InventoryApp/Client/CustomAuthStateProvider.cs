using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;

namespace InventoryApp.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            string authToken = await _localStorageService.GetItemAsync<string>("authToken");
            //string authToken = await _localStorageService.GetItemAsStringAsync("authToken");
            
            var identity = new ClaimsIdentity();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
                }
                catch (Exception)
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            var employee = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(employee);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyPairs = JsonSerializer
                .Deserialize<Dictionary<string, object>>(jsonBytes);
            
            if (keyPairs == null)
                return new List<Claim>();
            else
                return keyPairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }
    }
}