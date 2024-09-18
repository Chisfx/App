using App.Maui.Helpers;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
namespace App.Maui.Helpers
{
    /// <summary>
    /// This class provides methods for making HTTP requests.
    /// </summary>
    public class ClientHttp
    {
#if DEBUG
        private static HttpsClientHandlerService handler = new HttpsClientHandlerService();
        private static readonly HttpClient _httpClient = new HttpClient(handler.GetPlatformMessageHandler());
    #else
        private static readonly HttpClient _httpClient = new HttpClient();
    #endif

        /// <summary>
        /// Sends a GET request to the specified API endpoint and returns a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to retrieve.</typeparam>
        /// <param name="rutaapi">The API endpoint.</param>
        /// <returns>A list of objects.</returns>
        public static async Task<List<T>> GetAll<T>(string rutaapi)
        {
            List<T> result = null;
            try
            {
                if (_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(Router.UrlBase);
                result = await _httpClient.GetFromJsonAsync<List<T>>(rutaapi);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Sends a GET request to the specified API endpoint and returns an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to retrieve.</typeparam>
        /// <param name="rutaapi">The API endpoint.</param>
        /// <returns>An object of the specified type.</returns>
        public static async Task<T> Get<T>(string rutaapi)
        {
            T result = default;
            try
            {
                if (_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(Router.UrlBase);
                string cadena = await _httpClient.GetStringAsync(rutaapi);
                result = JsonSerializer.Deserialize<T>(cadena);
            }
            catch (Exception ex)
            {
                result = (T)Activator.CreateInstance(typeof(T));
            }
            return result;
        }

        /// <summary>
        /// Sends a GET request to the specified API endpoint and returns an integer.
        /// </summary>
        /// <param name="rutaapi">The API endpoint.</param>
        /// <returns>An integer.</returns>
        public static async Task<int> GetInt(string rutaapi)
        {
            int result = 0;
            try
            {
                if (_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(Router.UrlBase);
                string cadena = await _httpClient.GetStringAsync(rutaapi);
                result = int.Parse(cadena);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Sends a DELETE request to the specified API endpoint and returns an integer.
        /// </summary>
        /// <param name="rutaapi">The API endpoint.</param>
        /// <returns>An integer.</returns>
        public static async Task<int> Delete(string rutaapi)
        {
            int result = 0;
            try
            {
                if (_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(Router.UrlBase);
                var response = await _httpClient.DeleteAsync(rutaapi);
                if (response.IsSuccessStatusCode)
                {
                    string cadena = await response.Content.ReadAsStringAsync();
                    result = int.Parse(cadena);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Sends a POST request to the specified API endpoint with the specified object and returns an integer.
        /// </summary>
        /// <typeparam name="T">The type of object to send.</typeparam>
        /// <param name="rutaapi">The API endpoint.</param>
        /// <param name="obj">The object to send.</param>
        /// <returns>An integer.</returns>
        public static async Task<int> Post<T>(string rutaapi, T obj)
        {
            int result = 0;
            try
            {
                _httpClient.BaseAddress = new Uri(Router.UrlBase);
                var response = await _httpClient.PostAsJsonAsync<T>(rutaapi, obj);
                if (response.IsSuccessStatusCode)
                {
                    string cadena = await response.Content.ReadAsStringAsync();
                    result = int.Parse(cadena);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Sends a POST request to the specified API endpoint with the specified object and returns a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to send.</typeparam>
        /// <param name="rutaapi">The API endpoint.</param>
        /// <param name="obj">The object to send.</param>
        /// <returns>A list of objects.</returns>
        public static async Task<List<T>> PostList<T>(string rutaapi, T obj)
        {
            List<T> result = new List<T>();
            try
            {
                _httpClient.BaseAddress = new Uri(Router.UrlBase);
                var response = await _httpClient.PostAsJsonAsync<T>(rutaapi, obj);
                if (response.IsSuccessStatusCode)
                {
                    string cadena = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<List<T>>(cadena);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine(ex.Message);
            }
            return result;
        }
    }
}
