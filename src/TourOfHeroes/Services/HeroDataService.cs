using System;
using System.Net.Http;
using System.Threading.Tasks;
using TourOfHeroes.Data;
using TourOfHeroes.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace TourOfHeroes.Services
{

    /// <summary>
    /// Service which allows you to interact with a
    /// list of heroes
    /// </summary>
    public class HeroDataService : IHeroDataService
    {

        private IHttpClientFactory Factory {get;}
        
        private HttpClient ApiClient => Factory.CreateClient( GetType().FullName );

        /// <summary>
        /// Root Uri of the API controller
        /// </summary>
        /// <value></value>
        /// <remarks>In a real application, this would be part of a cofigurable item.</remarks>
        private Uri BaseUri {get;} = new Uri( "http://localhost:5000" );

        /// <summary>
        /// Relative path to the API controller.
        /// </summary>
        private readonly string ApiPath = "/api/herodata";

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="clientFactory">Factory which creates <see cref="HttpClient" /> objects for communicating
        /// with the backend WebApi controller.</param>
        public HeroDataService(IHttpClientFactory clientFactory)
        {
            Factory = clientFactory;
        }

        /// <inheritdoc/>
        public async Task<Hero[]> GetHeroesAsync()
        {
            var route = new Uri(BaseUri, ApiPath);

            var response = await ApiClient.GetAsync(route);
            
            var text = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Hero[]>(text);

        }


        /// <inheritdoc/>
        public async Task<Hero> FindHeroAsync(int id)
        {
            var route = new Uri(BaseUri, $"{ApiPath}/find/{id}");

            var response = await ApiClient.GetAsync(route);
            
            var text = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Hero>(text);
        }

        /// <inheritdoc/>
        public async Task<Hero> AddHeroAsync(string heroName)
        {
            var route = new Uri(BaseUri, $"{ApiPath}");
            
            heroName = heroName.Replace("\"", "\\\"");
            var content = new StringContent("\"" + heroName + "\"");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await ApiClient.PostAsync(route, content);
            
            if(!response.IsSuccessStatusCode)
                return null;

            var text = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Hero>(text);

        }

        /// <inheritdoc/>
        public async Task<Hero> SaveHeroAsync(Hero hero)
        {
            var route = new Uri(BaseUri, $"{ApiPath}");
            var content = new StringContent(JsonConvert.SerializeObject(hero) );
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await ApiClient.PutAsync(route, content);
            
            if(!response.IsSuccessStatusCode)
                return null;

            var text = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Hero>(text);
        }

        /// <inheritdoc/>
        public async Task<Hero> DeleteHeroAsync(int id)
        {

            var route = new Uri(BaseUri, $"{ApiPath}/{id}");

            var response = await ApiClient.DeleteAsync(route);
            
            if(!response.IsSuccessStatusCode)
                return null;

            var text = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Hero>(text);
        }
        
    }
}