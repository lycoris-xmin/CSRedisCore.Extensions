using Lycoris.CSRedisCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

CSRedisCoreBuilder.AddSingleRedisInstance(opt =>
{
    opt.Host = "127.0.0.1";
    opt.Port = 6379;

    // redis6.0以上设置了用户名密码的情况下需要填写，其他情况不需要填写
    //opt.UserName = "your redis username";

    opt.Password = "";
    opt.SSL = false;
    // 不设置的情况下默认为 0 库
    //opt.UseDatabase = 1;
    // 是否尝试集群模式，阿里云、腾讯云集群需要设置此选项为 false
    opt.TestCluster = false;
    opt.Prefix = "Lycoris:Test";
});

app.MapGet("/weatherforecast", async () =>
{
    for (int i = 0; i < 5; i++)
    {
        await RedisCache.Utils.EnqueueAsync("test_demo", i.ToString());
    }

    for (int i = 0; i < 5; i++)
    {
        await RedisCache.Utils.EnqueueAsync("test_demo", i.ToString(), false);
    }

    await RedisCache.Utils.RemoveValueFromQueueAsync("test_demo", 4.ToString());

    var result5 = await RedisCache.Utils.CheckValueExitsFromQueueAsync("test_demo", 5.ToString());

    var result3 = await RedisCache.Utils.CheckValueExitsFromQueueAsync("test_demo", 3.ToString());

    var cache = await RedisCache.String.GetAsync("testdemo");
    Console.WriteLine(cache);

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal class TestDemo
{
    public string A { get; set; } = "";
}