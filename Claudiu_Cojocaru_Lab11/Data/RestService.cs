using Newtonsoft.Json;
using System.Text;
using Claudiu_Cojocaru_Lab11.Models;

namespace Claudiu_Cojocaru_Lab11.Data
{
    public class RestService : IRestService
    {
        HttpClient client;

        #if ANDROID
            string RestUrl = "https://10.0.2.2:5001/api/shoplists/{0}";
        #else
            string RestUrl = "https://192.168.0.162:5001/api/shoplists/{0}";
        #endif
        
        public List<ShopList> Items { get; private set; }

        public RestService()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => { return true; };
            client = new HttpClient(httpClientHandler);
        }

        public async Task<List<ShopList>> RefreshDataAsync()
        {
            Items = new List<ShopList>();
            Uri uri = new Uri(string.Format(RestUrl, string.Empty));
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<ShopList>>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
            
            return Items;
        }

        public async Task SaveShopListAsync(ShopList item, bool isNewItem = true)
        {
            try
            {
                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;

                if (isNewItem)
                {
                    Uri uri = new Uri(string.Format(RestUrl, string.Empty));
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    Uri uri = new Uri(string.Format(RestUrl, item.ID));
                    response = await client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(@"\tShopList successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task DeleteShopListAsync(int id)
        {
            Uri uri = new Uri(string.Format(RestUrl, id));
            
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(@"\tShopList successfully deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
    }
}
