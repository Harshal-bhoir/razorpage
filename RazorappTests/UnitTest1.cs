namespace RazorappTests;
using RazorApp.Services;
using RazorApp.Models;
using Moq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

[TestClass]
public class UnitTest1
{
    private readonly EmployeeService _employeeService;
    private readonly Mock<IContainer> container = new Mock<IContainer>();
    private readonly Mock<ILogger> logger = new Mock<ILogger>();

    public UnitTest1()
    {
        _employeeService = new EmployeeService();
    }

    [TestMethod]
    public void TestGetEmployees()
    {
        List<EmployeeModel> empList = new List<EmployeeModel>()
        {
            
        };
        var empService = new Mock<IEmployeeService>();
        empService.Setup(x => x.Get(It.IsAny<string>())).Returns(empList);

    }
}
