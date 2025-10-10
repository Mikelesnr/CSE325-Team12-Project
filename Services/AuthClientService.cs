using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using CSE325_Team12_Project.Models.DTOs;
using Blazored.LocalStorage;

public class AuthClientService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public UserDto? CurrentUser { get; private set; }
    public string? Token { get; private set; }

    public AuthClientService(HttpClient http, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
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

        await _localStorage.SetItemAsync("authToken", Token);

        var success = await LoadCurrentUserAsync();
        if (success && CurrentUser != null)
        {
            await _localStorage.SetItemAsync("authUser", CurrentUser);
        }

        return success;
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

        await _localStorage.SetItemAsync("authToken", Token);

        var success = await LoadCurrentUserAsync();
        if (success && CurrentUser != null)
        {
            await _localStorage.SetItemAsync("authUser", CurrentUser);
        }

        return success;
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

    public async Task<bool> RestoreSessionAsync()
    {
        Token = await _localStorage.GetItemAsync<string>("authToken");
        if (string.IsNullOrWhiteSpace(Token)) return false;

        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

        return await LoadCurrentUserAsync();
    }

    public async Task LogoutAsync()
    {
        _http.DefaultRequestHeaders.Authorization = null;
        CurrentUser = null;
        Token = null;

        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("authUser");
    }

    public bool IsAuthenticated => CurrentUser != null;

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
