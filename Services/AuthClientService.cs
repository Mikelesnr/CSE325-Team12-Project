using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using CSE325_Team12_Project.Models.DTOs;

public class AuthClientService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;

    public UserDto? CurrentUser { get; private set; }
    public string? Token { get; private set; }

    public AuthClientService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _http = http; // ✅ Blazor injects this with BaseAddress set
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { email, password });
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result?.Token == null) return false;

        Token = result.Token;
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

        return await LoadCurrentUserAsync();
    }

    public async Task<bool> RegisterAsync(string name, string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", new { name, email, password });
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result?.Token == null) return false;

        Token = result.Token;
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

        return await LoadCurrentUserAsync();
    }

    public async Task<bool> LoadCurrentUserAsync()
    {
        try
        {
            CurrentUser = await _http.GetFromJsonAsync<UserDto>("api/auth/me");
            return CurrentUser != null;
        }
        catch
        {
            CurrentUser = null;
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        _http.DefaultRequestHeaders.Authorization = null;
        CurrentUser = null;
        Token = null;
    }

    public bool IsAuthenticated => CurrentUser != null;

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
