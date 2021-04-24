using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Movies.Client.Models;
using Movies.Client.SeedMockData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {

        private readonly IConfiguration _configuration;

        public MovieApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            try
            {
                return movie;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovie(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            try
            {

                var apiClientCredentials = new ClientCredentialsTokenRequest
                {
                    Address = $"{_configuration.GetValue<string>("IdentityServer:ServerURL")}/connect/token",
                    ClientId = "movieClient",
                    ClientSecret = _configuration.GetValue<string>("IdentityServer:ClientSecret"),
                    Scope = _configuration.GetValue<string>("IdentityServer:ScopeOption3")
                };

                var client = new HttpClient();

                //Check Identity Server Status
                var identityServerEcho = await client.GetDiscoveryDocumentAsync(_configuration.GetValue<string>("IdentityServer:ServerURL"));
                if (identityServerEcho.IsError)
                {
                    throw new Exception("Identity Server Is not running.");
                }


                //Get Token From Identity Server
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
                if (tokenResponse.IsError)
                {
                    throw new Exception("Failed to Get token From Identity Server.");
                }


                var apiClient = new HttpClient();
                apiClient.SetBearerToken(tokenResponse.AccessToken);

                var responseFromApi = await apiClient.GetAsync("https://localhost:5001/api/Movies");
                responseFromApi.EnsureSuccessStatusCode();

                //Convert Response in String 
                var content = await responseFromApi.Content.ReadAsStringAsync();

                //De
                var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);

                return movieList;
                //var movies = SeedData.GetMockMovies();
                //return await Task.FromResult(movies);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
