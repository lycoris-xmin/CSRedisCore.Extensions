### 安装方式

```shell
// net cli
dotnet add package Lycoris.CSRedisCore.Extensions
// package magager
Install-Package Lycoris.CSRedisCore.Extensions
```

**由于扩展服务全局静态服务,故注册位置请自行安排**

### 单实例服务

#### 实例注册

```csharp
CSRedisCoreBuilder.AddSingleRedisInstance(opt =>
{
    opt.Host = "your redis host";
    opt.Port = 6379;

    // redis6.0以上设置了用户名密码的情况下需要填写，其他情况不需要填写
    //opt.UserName = "your redis username";

    opt.Password = "your redis password";
    opt.SSL = false;
    // 不设置的情况下默认为 0 库
    //opt.UseDatabase = 1;
    // 是否尝试集群模式，阿里云、腾讯云集群需要设置此选项为 false
    opt.TestCluster = false;
});
```

### 使用
```csharp
// 写入 string类型 缓存
await RedisCache.String.SetAsync("testdemo", Guid.NewGuid().ToString(), TimeSpan.FromSeconds(60));
// 读取缓存
var cache = await RedisCache.String.GetAsync("testdemo");
```


### 多实例服务

#### 实例注册

```csharp
CSRedisCoreBuilder.AddMultipleRedisInstance("redis1", opt =>
{
    opt.Host = "server.lycoris.cloud";
    opt.Port = 8471;
    opt.Password = "Yt@6JhDEoaTZVFAm";
    opt.SSL = false;
    // 是否尝试集群模式，阿里云、腾讯云集群需要设置此选项为 false
    opt.TestCluster = false;
});

CSRedisCoreBuilder.AddMultipleRedisInstance("redis2", opt =>
{
    opt.Host = "server.lycoris.cloud";
    opt.Port = 8471;
    opt.Password = "Yt@6JhDEoaTZVFAm";
    opt.UseDatabase = 1;
    opt.SSL = false;
    // 是否尝试集群模式，阿里云、腾讯云集群需要设置此选项为 false
    opt.TestCluster = false;
});
```

#### 使用方式

```csharp
// 从工厂中获取指定实例
var redis1 = RedisCacheFactory.GetInstance("redis1");

// 写入 string类型 缓存
await redis1.String.SetAsync("testdemo", Guid.NewGuid().ToString(), TimeSpan.FromSeconds(60));
// 读取缓存
var cache = await redis1.String.GetAsync("testdemo");
```

