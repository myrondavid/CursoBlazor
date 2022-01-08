using Client.Static;
using Shared.Models;
using System.Net.Http.Json;

namespace Client.Services
{
    internal sealed class InMemoryDatabaseCache
    {
        private readonly HttpClient _httpClient;
        public InMemoryDatabaseCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private List<Category> _categories = null;
        internal List<Category> Categories
        {
            get { return _categories; }
            set 
            {
                _categories = value;
                NotifyCategoriesDataChanged();
            }
        }

        private bool _gettingCategoriesFromDatabaseAndCache = false;

        internal async Task GetCategoriesFromDatabaseAndCache()
        {
            //só permite uma requisição por vez
            if (_gettingCategoriesFromDatabaseAndCache == false)
            {
                _gettingCategoriesFromDatabaseAndCache = true;
                _categories = await _httpClient.GetFromJsonAsync<List<Category>>(APIEndpoints.s_categories);
                _gettingCategoriesFromDatabaseAndCache = false;
            }
             
        }

        internal event Action OnCategoriesDataChanged;
        private void NotifyCategoriesDataChanged()
        {
            OnCategoriesDataChanged?.Invoke();
        }
    }
}
