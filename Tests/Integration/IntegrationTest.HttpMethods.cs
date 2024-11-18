using System.Net;
using System.Text;
using System.Text.Json;

namespace Integration;

public abstract partial class IntegrationTest
{
    protected async Task<ApiResponse<T>> GetAsync<T>(string path)
    {
        var response = await _client.GetAsync(path);
        return await GetApiResponseAsync<T>(response);
    }

    protected async Task<ApiResponse> GetAsync(string path)
    {
        var response = await _client.GetAsync(path);
        return GetApiResponse(response);
    }

    protected async Task<ApiResponse<T>> DeleteAsync<T>(string path)
    {
        var response = await _client.DeleteAsync(path);
        return await GetApiResponseAsync<T>(response);
    }

    protected async Task<ApiResponse> DeleteAsync(string path)
    {
        var response = await _client.DeleteAsync(path);
        return GetApiResponse(response);
    }

    protected async Task<ApiResponse<T>> PostAsync<T>(string path, object payload)
    {
        var response = await _client.PostAsync(path, SerializeRequestBody(payload));
        return await GetApiResponseAsync<T>(response);
    }

    protected async Task<ApiResponse> PostAsync(string path, object payload)
    {
        var response = await _client.PostAsync(path, SerializeRequestBody(payload));
        return GetApiResponse(response);
    }

    protected async Task<ApiResponse<T>> PutAsync<T>(string path, object payload)
    {
        var response = await _client.PutAsync(path, SerializeRequestBody(payload));
        return await GetApiResponseAsync<T>(response);
    }

    protected async Task<ApiResponse> PutAsync(string path, object payload)
    {
        var response = await _client.PutAsync(path, SerializeRequestBody(payload));
        return GetApiResponse(response);
    }

    private static StringContent? SerializeRequestBody(object? payload)
    {
        if (payload is null)
            return null;

        var json = JsonSerializer.Serialize(payload);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected class ApiResponse
    {
        public HttpStatusCode StatusCode { get; init; }
    }

    protected class ApiResponse<T> : ApiResponse
    {
        public T? Content { get; init; }
    }

    private async Task<ApiResponse<T>> GetApiResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (content == string.Empty)
            return new ApiResponse<T> { StatusCode = response.StatusCode };

        return new ApiResponse<T>
        {
            StatusCode = response.StatusCode,
            Content =
                typeof(T) == typeof(string)
                    ? (T)(object)content
                    : JsonSerializer.Deserialize<T>(content, _serializerOptions),
        };
    }

    private static ApiResponse GetApiResponse(HttpResponseMessage response)
    {
        return new ApiResponse { StatusCode = response.StatusCode };
    }
}
