﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _Client;

        protected readonly string _ServiceAddress;

        protected BaseClient(IConfiguration config, string ServiceAddress)
        {
            _ServiceAddress = ServiceAddress;

            _Client = new HttpClient
            {
                BaseAddress = new Uri(config["ClientAddress"])
            };

            var headers = _Client.DefaultRequestHeaders.Accept;

            headers.Clear();
            headers.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected T Get<T>(string url) where T : new() => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default) where T : new()
        {
            var response = await _Client.GetAsync(url, Cancel);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<T>(Cancel);
            return new T();
        }

        protected HttpResponseMessage Post<T>(string url, T Item) => PostAsync(url, Item).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T Item, CancellationToken Cancel = default)
        {
            var response = await _Client.PostAsJsonAsync(url, Item, Cancel);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T Item) => PutAsync(url, Item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T Item, CancellationToken Cancel = default)
        {
            var response = await _Client.PutAsJsonAsync(url, Item, Cancel);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default) =>
            await _Client.DeleteAsync(url, Cancel);
    }
}
