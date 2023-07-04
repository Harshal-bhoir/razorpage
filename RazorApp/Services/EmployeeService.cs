using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using RazorApp.Models;
using RazorApp.Services;

namespace RazorApp.Services;

	public class EmployeeService: IEmployeeService
	{
    private readonly Container _container;
    private readonly ILogger _logger;

    public EmployeeService() { }

    public EmployeeService(CosmosClient cosmosClient,
    string databaseName,
    string containerName, ILogger<EmployeeService> logger)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
        _logger = logger;
    }

    public async Task<List<EmployeeModel>> Get(string sqlCosmosQuery)
    {
        FeedIterator<EmployeeModel> query = _container.GetItemQueryIterator<EmployeeModel>(new QueryDefinition(sqlCosmosQuery));
        _logger.LogWarning("Warning log");
        
        return await GetResult(query);
    }

    public async Task<EmployeeModel> AddAsync(EmployeeModel newEmp)
    {
        ItemResponse<EmployeeModel> item = await _container.CreateItemAsync<EmployeeModel>(newEmp, new PartitionKey(newEmp.EmpId));
        return item;
    }

    public async Task<EmployeeModel> Update(EmployeeModel empToUpdate)
    {
        FeedIterator<EmployeeModel> query = _container.GetItemQueryIterator<EmployeeModel>(new QueryDefinition("select * from c where c.EmpId=" + empToUpdate.EmpId));
        List<EmployeeModel> res = await GetResult(query);
        
        res[0].EmpDepartment = empToUpdate.EmpDepartment;
        res[0].EmpLastName = empToUpdate.EmpLastName;
        res[0].EmpName = empToUpdate.EmpName;
        ItemResponse<EmployeeModel> item = await _container.UpsertItemAsync<EmployeeModel>(res[0], new PartitionKey(empToUpdate.EmpId));
        return item;
    }

    public async Task Delete(string empId)
    {
        FeedIterator<EmployeeModel> query = _container.GetItemQueryIterator<EmployeeModel>(new QueryDefinition("select * from c where c.EmpId=" + empId));
        List<EmployeeModel> res = await GetResult(query);
        string id = res[0].Id;

        await _container.DeleteItemAsync<EmployeeModel>(id, new PartitionKey(res[0].EmpId));
    }

    private async Task<List<EmployeeModel>> GetResult(FeedIterator<EmployeeModel> query)
    {
        List<EmployeeModel> res = new List<EmployeeModel>();
        while (query.HasMoreResults)
        {
            FeedResponse<EmployeeModel> response = await query.ReadNextAsync();
            res.AddRange(response);
        }
        return res;
    }

}

