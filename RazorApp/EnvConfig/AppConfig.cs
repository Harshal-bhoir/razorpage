using System;
namespace RazorApp.EnvConfig;

public class AppConfig: IAppConfig
{
    public readonly string _testvalue = string.Empty;

    public IConfiguration Configuration { get; }
    public AppConfig(IConfiguration configuration)
    {
        Configuration = configuration;
        _testvalue = Configuration["ENV"];
    }

    public string GetTestValue()
    {
        return _testvalue;
    }

}

