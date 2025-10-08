using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

public class AuthStateService
{
    private readonly IJSRuntime _js;

    public AuthStateService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
    }

    public async Task<bool> CheckAndRedirectIfNotAuthenticatedAsync(NavigationManager navigation, bool checkedAuth)
    {
        if (!checkedAuth)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                navigation.NavigateTo("/login", true);
            }
            return true;
        }
        return checkedAuth;
    }
}