using Lycoris.CSRedisCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


CSRedisCoreBuilder.AddMultipleRedisInstance("ii", opt =>
{
    opt.Host = "your redis host";
    opt.Port = 6379;

    // redis6.0�����������û���������������Ҫ��д�������������Ҫ��д
    //opt.UserName = "your redis username";

    opt.Password = "your redis password";
    opt.SSL = false;
    // �Ƿ��Լ�Ⱥģʽ�������ơ���Ѷ�Ƽ�Ⱥ��Ҫ���ô�ѡ��Ϊ false
    opt.TestCluster = false;
});

CSRedisCoreBuilder.AddMultipleRedisInstance("i2", opt =>
{
    opt.Host = "your redis host";
    opt.Port = 6379;

    // redis6.0�����������û���������������Ҫ��д�������������Ҫ��д
    //opt.UserName = "your redis username";

    opt.Password = "your redis password";
    opt.UseDatabase = 1;
    opt.SSL = false;
    // �Ƿ��Լ�Ⱥģʽ�������ơ���Ѷ�Ƽ�Ⱥ��Ҫ���ô�ѡ��Ϊ false
    opt.TestCluster = false;
});

app.MapGet("/weatherforecast", async () =>
{
    var ii = RedisCacheFactory.GetInstance("ii");

    await ii.String.SetAsync("testdemo", Guid.NewGuid().ToString());
    var cache = await ii.String.GetAsync("testdemo");
    Console.WriteLine(cache);


    var i2 = RedisCacheFactory.GetInstance("i2");
    await i2.String.SetAsync("testdemo", Guid.NewGuid().ToString());
    var cache2 = await i2.String.GetAsync("testdemo");
    Console.WriteLine(cache2);

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