using System.Net;
using System.Text.Json;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ServerAPI.Entities;
using TempFuncMachine.Entities;
using TempFuncMachine.Extensions;
using TempFuncMachine.Helpers;

namespace TempFuncMachine
{
    public class FuncTemp
    {
        private readonly ILogger _logger;

        public FuncTemp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FuncTemp>();
        }

        // Nuget:
        // Microsoft.Azure.Functions.Worker.Extensions.Tables - Isolated!
        // Azure.Data.Tables
        // Microsoft.AspNetCore.Mvc

        [Function("Get Machine")]
        public async Task<HttpResponseData> Get(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Machine")] HttpRequestData req,
        [TableInput(TableNames.TableName, TableNames.PartionKey, Connection = "AzureWebJobsStorage")] IEnumerable<MachineTable> tableEntities)
        {
            _logger.LogInformation("Get all machine started!");

            var response = req.CreateResponse();
            var items = tableEntities.Select(Mapper.ToMachine);
            await response.WriteAsJsonAsync(items);
            return response;
        }

        //[Function("GetId Machine")]
        //public async Task<HttpResponseData> GetId(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Machine/{id}")] HttpRequestData req,
        //[TableInput(TableNames.TableName, TableNames.PartionKey, Connection = "AzureWebJobsStorage")] IEnumerable<MachineTable> tableEntities,
        //[FromRoute] string id)
        //{
        //    _logger.LogInformation("Get specific machine");

        //    var response = req.CreateResponse();
        //    var items = tableEntities.Select(Mapper.ToMachine);

        //    foreach (var item in items)
        //    {
        //        if (item.Id == id)
        //        {
        //            await response.WriteAsJsonAsync(item);
        //            return response;
        //        }
        //    }
        //    return response;
        //}

        [Function("GetId Machine")]
        public async Task<HttpResponseData> GetId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Machine/{id}")] HttpRequestData req,
        [FromRoute] string id)
        {
            _logger.LogInformation("Get specific machine");

            var tableClient = GetTableClient();
            var response = req.CreateResponse();

            var found = await tableClient.GetEntityIfExistsAsync<MachineTable>(TableNames.PartionKey, id); // found.
            if (!found.HasValue)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            await response.WriteAsJsonAsync(found); // found används istället för items här.

            return response;
        }

        [Function("Add Machine")]
        public async Task<HttpResponseData> Create(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Machine")] HttpRequestData req)
        // [TableInput("Items", "Todos", Connection = "AzureWebJobsStorage")] TableClient tableClient)
        //[TableInput(TableNames.TableName, TableNames.PartionKey, Connection = "AzureWebJobsStorage")] IEnumerable<MachineTable> tableEntities)
        {
            _logger.LogInformation("Create new machine");

            var tableClient = GetTableClient();
            //var tableClient = tableEntities.Select(Mapper.ToCreateMachine);
            var response = req.CreateResponse();

            //var stream = await new StreamReader(req.Body).ReadToEndAsync();
            var createdMachine = JsonSerializer.Deserialize<Machine>(req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (createdMachine is null || string.IsNullOrWhiteSpace(createdMachine.Location))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            try
            {
                await tableClient.CreateIfNotExistsAsync();
                await tableClient.AddEntityAsync(createdMachine.ToTableEntity());
            }
            catch (RequestFailedException)
            {
                //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                await response.WriteAsJsonAsync(createdMachine);
                response.StatusCode = HttpStatusCode.Created;
            }
            return response;
        }

        [Function("Delete Machine")]
        public async Task<HttpResponseData> Delete(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Machine/{id}")] HttpRequestData req,
        // [TableInput(TableNames.TableName, TableNames.PartionKey, "{id}", Connection = "AzureWebJobsStorage")] IEnumerable<ItemTableEntity> tableEntity,
        [FromRoute] string id)
        {
            _logger.LogInformation("Delete machine");

            var tableClient = GetTableClient();
            var response = req.CreateResponse();

            //if(createdItem is null || string.IsNullOrWhiteSpace(createdItem.Text))
            //{
            //    response.StatusCode = HttpStatusCode.BadRequest;
            //    return response;
            //}

            var isOk = await tableClient.DeleteEntityAsync(TableNames.PartionKey, id);

            if (isOk.Status == StatusCodes.Status404NotFound)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            response.StatusCode = HttpStatusCode.NoContent;
            return response;
        }

        [Function("Edit Machine")]
        public async Task<HttpResponseData> Edit(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Machine/{id}")] HttpRequestData req,
        [FromRoute] string id)
        {
            _logger.LogInformation("Edit machine");

            var tableClient = GetTableClient();
            var response = req.CreateResponse();

            var editMachine = await JsonSerializer.DeserializeAsync<Machine>(req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (editMachine is null || string.IsNullOrWhiteSpace(editMachine.Location) || editMachine.Id != id)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            var found = await tableClient.GetEntityIfExistsAsync<MachineTable>(TableNames.PartionKey, id);
            if (!found.HasValue)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            var reponse = await tableClient.UpdateEntityAsync((MachineTable?)editMachine.ToTableEntity(), Azure.ETag.All);

            //ToDo check response!

            response.StatusCode = HttpStatusCode.NoContent;
            return response;
        }

        //[Function("TempFunc")]
        //public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        //{
        //    _logger.LogInformation("C# HTTP trigger function processed a request.");

        //    var response = req.CreateResponse(HttpStatusCode.OK);
        //    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        //    response.WriteString("Welcome to Azure Functions!");

        //    return response;
        //}

        private TableClient GetTableClient()
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            return new TableClient(connectionString, TableNames.TableName);
        }
    }
}
