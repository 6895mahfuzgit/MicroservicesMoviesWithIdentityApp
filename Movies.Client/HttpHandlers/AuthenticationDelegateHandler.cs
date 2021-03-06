using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegateHandler : DelegatingHandler
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly ClientCredentialsTokenRequest _clientCredentialsTokenRequest;

        //public AuthenticationDelegateHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest clientCredentialsTokenRequest)
        //{
        //    _httpClientFactory = httpClientFactory;
        //    _clientCredentialsTokenRequest = clientCredentialsTokenRequest;
        //}


        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationDelegateHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var httpClient = _httpClientFactory.CreateClient("IDPClient");
            //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_clientCredentialsTokenRequest);

            //if (tokenResponse.IsError)
            //{
            //    throw new HttpRequestException("Seever Error while Creating Access Token.");
            //}

            //request.SetBearerToken(tokenResponse.AccessToken);

            var accesstoken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accesstoken))
            {
                request.SetBearerToken(accesstoken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
