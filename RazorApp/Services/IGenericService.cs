using System;
using RazorApp.Models;

namespace RazorApp.Services
{
	public interface IGenericService
	{
        Task<List<T>> Get<T>(string sqlCosmosQuery);
        Task<T> AddAsync<T>(T newData, int id) where T : class, new();
        Task<T> Update<T>(T dataToUpdate, string queryDef, int id);
        Task<T> Delete<T>(string query, string id) where T: class, new();
    }
}

