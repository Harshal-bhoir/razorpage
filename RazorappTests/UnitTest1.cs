namespace RazorappTests;

using Autofac.Extras.Moq;
using RazorApp.Services;
using RazorApp.Models;

[TestClass]
public class UnitTest1
{
    private readonly EmployeeService _employeeService;

    public UnitTest1()
    {
        _employeeService = new EmployeeService();
    }

    [TestMethod]
    public void TestGetEmployees()
    {
        using(var mock = AutoMock.GetLoose())
        {
            mock.Mock<IEmployeeService>()
                .Setup(x => x.Get<EmployeeModel>("select * from c"));
        }
    }
}
