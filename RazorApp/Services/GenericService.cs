using System;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using RazorApp.Models;
using RazorApp.Services;

namespace RazorApp.Services
{
	public class GenericService: IGenericService
	{
        private readonly Container _container;
        private readonly ILogger _logger;

        public GenericService()
		{
		}

		public GenericService(CosmosClient cosmosClient,
		string databaseName, string containerName, ILogger logger)
		{
            _container = cosmosClient.GetContainer(databaseName, containerName);
            _logger = logger;
        }

        public async Task<List<T>> Get<T>(string sqlCosmosQuery)
        {
            FeedIterator<T> query = _container.GetItemQueryIterator<T>(new QueryDefinition(sqlCosmosQuery));
            
            return await GetResult(query);
        }

        public async Task<T> AddAsync<T>(T newData, int id) where T : class, new()
        {
            ItemResponse<T> item = await _container.CreateItemAsync<T>(newData, new PartitionKey(id));
            return item;
        }

        public async Task<T> Update<T>(T dataToUpdate, string queryDef, int id)
        {
            FeedIterator<T> query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryDef + id));
            List<T> res = await GetResult(query);
            res.Add(dataToUpdate);

            Type type = typeof(T);
            string cosmosGuid = "";
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name == "Id")
                {
                    cosmosGuid = type.GetProperty(property.Name).GetValue(res[0], null).ToString();
                    dataToUpdate.GetType().GetProperty("Id").SetValue("id", cosmosGuid);
                    break;
                }
            }
       
            ItemResponse<T> item = await _container.UpsertItemAsync<T>(dataToUpdate, new PartitionKey(id));
            return item;
        }

        public async Task<T> Delete<T>(string queryDef, string id) where T: class, new()
        {
            FeedIterator<T> query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryDef + id));
            List<T> res = await GetResult(query);

            Type type = typeof(T);
            string idDel = "";
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name == "Id")
                {
                   idDel = type.GetProperty(property.Name).GetValue(res[0], null).ToString();
                   break;
              
                }
            }
            int partKey = int.Parse(id);
            await _container.DeleteItemAsync<T>(idDel, new PartitionKey(partKey));
            return null;
        }

        private async Task<List<T>> GetResult<T>(FeedIterator<T> query)
        {
            List<T> res = new List<T>();
            while (query.HasMoreResults)
            {
                FeedResponse<T> response = await query.ReadNextAsync();
                res.AddRange(response);
            }
            return res;
        }
    }
}

