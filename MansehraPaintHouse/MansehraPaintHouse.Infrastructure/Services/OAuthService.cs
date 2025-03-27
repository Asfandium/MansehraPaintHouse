using System.Net.Http.Json;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MansehraPaintHouse.Core.Configuration;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;

namespace MansehraPaintHouse.Infrastructure.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OAuthService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly HttpClient _httpClient;
        private readonly OAuthConfig _oauthConfig;

        public OAuthService(
            IConfiguration configuration,
            ILogger<OAuthService> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClient = httpClient;
            _oauthConfig = _configuration.GetSection("OAuth").Get<OAuthConfig>();
        }

        public async Task<string> GetAuthorizationUrlAsync()
        {
            var state = Guid.NewGuid().ToString("N");
            var scopes = string.Join(" ", _oauthConfig.Scopes);
            
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", _oauthConfig.ClientId },
                { "redirect_uri", _oauthConfig.RedirectUri },
                { "response_type", "code" },
                { "scope", scopes },
                { "state", state },
                { "access_type", "offline" },
                { "prompt", "consent" }
            };

            var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
            return $"{_oauthConfig.AuthorizationEndpoint}?{queryString}";
        }

        public async Task<ApplicationUser> HandleCallbackAsync(string code)
        {
            try
            {
                var tokenRequest = new Dictionary<string, string>
                {
                    { "client_id", _oauthConfig.ClientId },
                    { "client_secret", _oauthConfig.ClientSecret },
                    { "code", code },
                    { "redirect_uri", _oauthConfig.RedirectUri },
                    { "grant_type", "authorization_code" }
                };

                var response = await _httpClient.PostAsync(
                    _oauthConfig.TokenEndpoint,
                    new FormUrlEncodedContent(tokenRequest));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get access token");
                    return null;
                }

                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                var user = await GetUserInfoAsync(tokenResponse.AccessToken);

                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling OAuth callback");
                return null;
            }
        }

        public async Task<ApplicationUser> GetUserInfoAsync(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync(_oauthConfig.UserInfoEndpoint);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get user info");
                    return null;
                }

                var userInfo = await response.Content.ReadFromJsonAsync<UserInfoResponse>();
                
                var user = await _userManager.FindByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userInfo.Email,
                        Email = userInfo.Email,
                        FirstName = userInfo.GivenName,
                        LastName = userInfo.FamilyName,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        _logger.LogError("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                        return null;
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user info");
                return null;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync(_oauthConfig.UserInfoEndpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return false;
            }
        }
    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }

    public class UserInfoResponse
    {
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Picture { get; set; }
    }
} 