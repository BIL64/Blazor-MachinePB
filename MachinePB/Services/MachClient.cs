﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ServerAPI.Entities;

namespace MachinePB.Services
{
    public class MachClient : IMachClient
    {
        private readonly HttpClient httpClient;

        public MachClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://localhost:7078"); // Lokal databas.
            //this.httpClient.BaseAddress = new Uri("https://machinepbserverapi.azurewebsites.net"); // Azure SQL databas.
            //this.httpClient.BaseAddress = new Uri("http://localhost:7172"); // Function App databas.
            //this.httpClient.BaseAddress = new Uri("https://machinetempf.azurewebsites.net"); // Function App databas.
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<Machine>?> GetAsync()
        {
            var response = await httpClient.GetFromJsonAsync<IEnumerable<Machine>>("api/Machine");
            return response;
        }

        public async Task<Machine?> GetAsync(string id)
        {
            return await httpClient.GetFromJsonAsync<Machine>($"api/Machine/{id}");
        }

        public async Task<Machine?> PostAsync(Machine machine)
        {
            var response = await httpClient.PostAsJsonAsync("api/Machine", machine);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Machine>() : null;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            return (await httpClient.DeleteAsync($"api/Machine/{id}")).IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string id, Machine machine)
        {
            return (await httpClient.PutAsJsonAsync($"api/Machine/{id}", machine)).IsSuccessStatusCode;
        }
    }
}
