using App.Maui.Helpers;
using System.Net.Http.Json;
using System.Text.Json;
namespace App.Maui.Abstractions
{
    public class ClientHttp
    {
        private static readonly HttpClient _httpClient = new HttpClient();
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
            }
            return result;
        }

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
            }
            return result;
        }

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
            }
            return result;
        }

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
                
            }
            return result;
        }
    }
}
