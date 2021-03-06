using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {


        private readonly IHttpClientFactory _httpClinetFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieApiService(IHttpClientFactory httpClinetFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClinetFactory = httpClinetFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            try
            {
                var httpClient = _httpClinetFactory.CreateClient("MovieAPIClient");
                var request = new HttpRequestMessage(HttpMethod.Post, "/Movies");
                var movieJson = JsonConvert.SerializeObject(movie);
                request.Content = new StringContent(movieJson, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                                        .ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return movie;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteMovie(int id)
        {
            try
            {
                var httpClient = _httpClinetFactory.CreateClient("MovieAPIClient");
                var request = new HttpRequestMessage(HttpMethod.Delete, $"/Movies/{id}");
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                                        .ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Movie> GetMovie(int id)
        {
            try
            {
                var httpClient = _httpClinetFactory.CreateClient("MovieAPIClient");
                var request = new HttpRequestMessage(HttpMethod.Get, $"/Movies/{id}");
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                                        .ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
                //Convert Response in String 
                var content = await response.Content.ReadAsStringAsync();
                var movie = JsonConvert.DeserializeObject<Movie>(content);
                return movie;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            try
            {
                var httpClient = _httpClinetFactory.CreateClient("MovieAPIClient");
                var request = new HttpRequestMessage(HttpMethod.Get, "/Movies");
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

        public async Task<UserInfoViewModel> GetUserInformation()
        {
            try
            {
                var idpClient = _httpClinetFactory.CreateClient("IDPClient");

                var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();
                if (metaDataResponse.IsError)
                {
                    throw new HttpRequestException("Can't Connect to Identity Server.");
                }

                var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

                var userInfoResponse = await idpClient.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = metaDataResponse.UserInfoEndpoint,
                    Token = accessToken
                });

                if (userInfoResponse.IsError)
                {
                    throw new HttpRequestException("failed to get user information.");
                }

                var userDictionary = new Dictionary<string, string>();
                foreach (var claim in userInfoResponse.Claims)
                {
                    userDictionary.Add(claim.Type, claim.Value);
                }


                return new UserInfoViewModel(userDictionary);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            try
            {
                var httpClient = _httpClinetFactory.CreateClient("MovieAPIClient");
                var request = new HttpRequestMessage(HttpMethod.Put, $"Movies/{movie.Id}");
                var movieJson = JsonConvert.SerializeObject(movie);
                request.Content = new StringContent(movieJson, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                                        .ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return movie;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
