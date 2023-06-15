using System;
using System.Runtime.ConstrainedExecution;
using RazorApp.Models;

namespace RazorApp.Services;

    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> Get(string sqlCosmosQuery);
        Task<EmployeeModel> AddAsync(EmployeeModel newEmp);
        Task<EmployeeModel> Update(EmployeeModel empToUpdate);
        Task Delete(string EmpId);
    }

