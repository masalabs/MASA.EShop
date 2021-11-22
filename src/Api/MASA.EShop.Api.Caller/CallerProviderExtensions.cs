namespace MASA.EShop.Api.Caller;

public static class CallerProviderExtensions
{
    static async Task<HttpResponseMessage> InvokeBase(this ICallerProvider caller, HttpMethod httpMethod, string urlOrMethod, HttpContent? httpContent, CancellationToken cancellationToken = default)
    {
        return await caller.SendAsync(httpMethod, urlOrMethod, httpContent, cancellationToken);
    }

    static async Task<string> InvokeProxy(this ICallerProvider caller, HttpMethod httpMethod, string urlOrMethod, HttpContent? httpContent, CancellationToken cancellationToken = default)
    {
        var taskResponse = InvokeBase(caller, httpMethod, urlOrMethod, httpContent, cancellationToken);
        return await GetStringAsyncCore(taskResponse, cancellationToken);
    }

    static async Task<TValue?> InvokeProxy<TValue>(this ICallerProvider caller, HttpMethod httpMethod, string urlOrMethod, JsonSerializerOptions? options, HttpContent? httpContent, CancellationToken cancellationToken = default)
    {
        var taskResponse = InvokeBase(caller, httpMethod, urlOrMethod, httpContent, cancellationToken);
        return await GetFromJsonAsyncCore<TValue>(taskResponse, options, cancellationToken);
    }
    static async Task<string> GetStringAsyncCore(Task<HttpResponseMessage> taskResponse, CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
        {
            return response.Content.ReadAsStringAsync().Result;
        }
    }

    static async Task<T?> GetFromJsonAsyncCore<T>(Task<HttpResponseMessage> taskResponse, JsonSerializerOptions? options, CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
        {
            response.EnsureSuccessStatusCode();
            // Nullable forgiving reason:
            // GetAsync will usually return Content as not-null.
            // If Content happens to be null, the extension will throw.
            return await response.Content.ReadFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);
        }
    }

    public static async Task<TValue?> GetFromJsonAsync<TValue>(this ICallerProvider caller, string requestUri, CancellationToken cancellationToken = default)
    {
        return await GetFromJsonAsync<TValue>(caller, requestUri, null, cancellationToken);
    }

    public static async Task<TValue?> GetFromJsonAsync<TValue>(this ICallerProvider caller, string requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
    {
        return await InvokeProxy<TValue>(caller, HttpMethod.Get, requestUri, options, null, cancellationToken);
    }

    public static async Task<string> GetStringAsync(this ICallerProvider caller, string requestUri, CancellationToken cancellationToken)
    {
        return await InvokeProxy(caller, HttpMethod.Get, requestUri, null, cancellationToken);
    }

    public static async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this ICallerProvider caller, string requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        JsonContent content = JsonContent.Create(value, mediaType: null, options);
        return await InvokeBase(caller, HttpMethod.Post, requestUri, content, cancellationToken);
    }

    public static async Task<HttpResponseMessage> PutAsync(this ICallerProvider caller, string requestUri, HttpContent? content)
    {
        return await InvokeBase(caller, HttpMethod.Put, requestUri, content);
    }
}

