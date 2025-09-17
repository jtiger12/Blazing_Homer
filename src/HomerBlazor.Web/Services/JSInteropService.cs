using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HomerBlazor.Web.Services;

public interface IJSInteropService
{
    Task<T> InvokeAsync<T>(string identifier, params object?[]? args);
    Task InvokeVoidAsync(string identifier, params object?[]? args);
    Task<T> GetLocalStorageItemAsync<T>(string key, T defaultValue = default!);
    Task SetLocalStorageItemAsync(string key, object? value);
    Task<bool> IsJavaScriptAvailableAsync();
}

public class JSInteropService : IJSInteropService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<JSInteropService> _logger;
    private bool? _isJavaScriptAvailable;

    public JSInteropService(IJSRuntime jsRuntime, ILogger<JSInteropService> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public async Task<bool> IsJavaScriptAvailableAsync()
    {
        if (_isJavaScriptAvailable.HasValue)
            return _isJavaScriptAvailable.Value;

        try
        {
            await _jsRuntime.InvokeAsync<string>("eval", "''");
            _isJavaScriptAvailable = true;
            return true;
        }
        catch (InvalidOperationException)
        {
            _isJavaScriptAvailable = false;
            return false;
        }
    }

    public async Task<T> InvokeAsync<T>(string identifier, params object?[]? args)
    {
        try
        {
            if (!await IsJavaScriptAvailableAsync())
            {
                _logger.LogWarning("JavaScript interop not available for {Identifier}", identifier);
                return default!;
            }

            return await _jsRuntime.InvokeAsync<T>(identifier, args);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "JavaScript interop failed for {Identifier}", identifier);
            return default!;
        }
    }

    public async Task InvokeVoidAsync(string identifier, params object?[]? args)
    {
        try
        {
            if (!await IsJavaScriptAvailableAsync())
            {
                _logger.LogWarning("JavaScript interop not available for {Identifier}", identifier);
                return;
            }

            await _jsRuntime.InvokeVoidAsync(identifier, args);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "JavaScript interop failed for {Identifier}", identifier);
        }
    }

    public async Task<T> GetLocalStorageItemAsync<T>(string key, T defaultValue = default!)
    {
        try
        {
            var result = await InvokeAsync<string>("localStorage.getItem", key);
            
            if (string.IsNullOrEmpty(result))
                return defaultValue;

            if (typeof(T) == typeof(string))
                return (T)(object)result;
            
            if (typeof(T) == typeof(bool))
                return (T)(object)(result == "true");
                
            if (typeof(T) == typeof(int) && int.TryParse(result, out var intValue))
                return (T)(object)intValue;

            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public async Task SetLocalStorageItemAsync(string key, object? value)
    {
        await InvokeVoidAsync("localStorage.setItem", key, value?.ToString() ?? "");
    }
}