using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClinetFactory;

        public MovieApiService(IConfiguration configuration, IHttpClientFactory httpClinetFactory)
        {
            _configuration = configuration;
            _httpClinetFactory = httpClinetFactory;
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
                var httpClient = _httpClinetFactory.CreateClient("MovieAPIClient");
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/Movies");
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                                        .ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                //Convert Response in String 
                var content = await response.Content.ReadAsStringAsync();
                var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);

                //Manual API Call
                //var apiClientCredentials = new ClientCredentialsTokenRequest
                //{
                //    Address = $"{_configuration.GetValue<string>("IdentityServer:ServerURL")}/connect/token",
                //    ClientId = "movieClient",
                //    ClientSecret = _configuration.GetValue<string>("IdentityServer:ClientSecret"),
                //    Scope = _configuration.GetValue<string>("IdentityServer:ScopeOption3")
                //};

                //var client = new HttpClient();

                ////Check Identity Server Status
                //var identityServerEcho = await client.GetDiscoveryDocumentAsync(_configuration.GetValue<string>("IdentityServer:ServerURL"));
                //if (identityServerEcho.IsError)
                //{
                //    throw new Exception("Identity Server Is not running.");
                //}


                ////Get Token From Identity Server
                //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
                //if (tokenResponse.IsError)
                //{
                //    throw new Exception("Failed to Get token From Identity Server.");
                //}


                //var apiClient = new HttpClient();
                //apiClient.SetBearerToken(tokenResponse.AccessToken);

                //var responseFromApi = await apiClient.GetAsync("https://localhost:5001/api/Movies");
                //responseFromApi.EnsureSuccessStatusCode();

                ////Convert Response in String 
                //var content = await responseFromApi.Content.ReadAsStringAsync();
                //var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);




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
