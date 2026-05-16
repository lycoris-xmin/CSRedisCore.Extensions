# Lycoris.CSRedisCore.Extensions

[![NuGet](https://img.shields.io/nuget/v/Lycoris.CSRedisCore.Extensions.svg)](https://www.nuget.org/packages/Lycoris.CSRedisCore.Extensions)
[![License](https://img.shields.io/github/license/lycoris-xmin/CSRedisCore.Extensions)](LICENSE)

基于 [CSRedisCore](https://github.com/2881099/csredis) 的二次封装库，为不熟悉 Redis 命令的开发者提供简洁、类型安全的 C# API。内置 JSON 序列化、分布式锁、队列、发布/订阅、Redis 监控等常用场景支持。

## 特性

- **强类型 API** — 支持泛型读写，自动 JSON 序列化/反序列化
- **多数据类型** — String、Hash、List、Set、Sorted Set 全覆盖
- **单实例 / 多实例** — 静态类快速调用，工厂模式管理多 Redis 连接
- **分布式锁** — 带看门狗（Watchdog）自动续期，防止死锁
- **队列封装** — 入队/出队、去重、移除、计数
- **发布/订阅** — 普通订阅 & 模式订阅（通配符），集群兼容
- **有序集合增强** — 按分数范围查询、排名、弹出最高/最低分成员
- **Redis 监控** — 解析 INFO 命令，返回结构化 Server / Memory / CPU / Stats / Keyspace 信息
- **事务 & 管道** — 类型化管道 API（与 RedisCache 一致的调用风格），支持批量操作、Lua 脚本执行
- **缓存穿透保护** — `CacheShell` 空值缓存
- **Key 前缀** — 自动为所有 Key 添加统一前缀，便于多项目共用
- **哨兵模式** — 支持 Redis Sentinel 高可用部署
- **安全序列化** — 内置 `long` → `string`、`decimal` → `string` 转换器，防止前端精度丢失

## 安装

```shell
dotnet add package Lycoris.CSRedisCore.Extensions
```

## 快速开始

### 单实例

```csharp
// 注册（Program.cs 中调用一次）
CSRedisCoreBuilder.AddSingleRedisInstance(opt =>
{
    opt.Host = "127.0.0.1";
    opt.Port = 6379;
    opt.Password = "your-password";
    opt.Prefix = "MyApp";       // Key 前缀，自动变为 "MyApp:"
});

// 使用
await RedisCache.String.SetAsync("user:1", new { Name = "Alice" }, TimeSpan.FromMinutes(10));
var user = await RedisCache.String.GetAsync<User>("user:1");
```

### 多实例

```csharp
// 注册
CSRedisCoreBuilder.AddMultipleRedisInstance("cache", opt =>
{
    opt.Host = "cache-server";
    opt.Port = 6379;
});

CSRedisCoreBuilder.AddMultipleRedisInstance("session", opt =>
{
    opt.Host = "session-server";
    opt.Port = 6380;
    opt.UseDatabase = 1;
});

// 使用
var cache = RedisCacheFactory.GetInstance("cache");
var session = RedisCacheFactory.GetInstance("session");

await cache.String.SetAsync("key", "value");
await session.String.SetAsync("token", "xxx");
```

## API 概览

所有 API 均提供同步与异步版本。以下以静态类 `RedisCache` 为例，多实例通过 `RedisCacheFactory.GetInstance(name)` 获取的 `RedisCacheService` 实例拥有相同接口。

### String 操作 `RedisCache.String`

```csharp
// 写入
await RedisCache.String.SetAsync("key", "value", TimeSpan.FromMinutes(5));
await RedisCache.String.SetAsync("user", userObj);

// 仅当 key 不存在/已存在时才设置
await RedisCache.String.SetIfNotExistsAsync("lock", "1", TimeSpan.FromSeconds(30));
await RedisCache.String.SetIfExistsAsync("key", "newvalue");

// 读取
string val = await RedisCache.String.GetAsync("key");
User user = await RedisCache.String.GetAsync<User>("user");

// 批量操作
await RedisCache.String.MultipleSetAsync("k1", "v1", "k2", "v2");
List<string> values = await RedisCache.String.MultipleGetAsync("k1", "k2");

// 目录批量获取
List<User> allUsers = await RedisCache.String.MultipleGetAsync<User>("user");

// 原子增减
long count = await RedisCache.String.AdditionAsync("counter", 1);
long remain = await RedisCache.String.SubtractionAsync("stock", 5);

// 字符串操作
long len = await RedisCache.String.StringLengthAsync("key");
long newLen = await RedisCache.String.AppendAsync("key", "-suffix");
string sub = await RedisCache.String.GetRangeAsync("key", 0, 10);
await RedisCache.String.SetRangeAsync("key", 5, "replace");
```

### Hash 操作 `RedisCache.Hash`

```csharp
// 设置字段
await RedisCache.Hash.SetAsync("user:1", "name", "Alice");
await RedisCache.Hash.SetAsync("user:1", ("name", "Bob"), ("age", "30"));

// 获取字段
string name = await RedisCache.Hash.GetAsync("user:1", "name");

// 获取全部字段
Dictionary<string, string> all = await RedisCache.Hash.GetAllAsync("user:1");

// 仅当字段不存在才设置
await RedisCache.Hash.SetNxAsync("user:1", "role", "admin");

// 字段增减（整数/浮点）
await RedisCache.Hash.AdditionAsync("user:1", "score", 10);
await RedisCache.Hash.IncrByFloatAsync("user:1", "balance", 1.5m);

// 获取所有值 / 字段值长度
List<string> values = await RedisCache.Hash.ValuesAsync("user:1");
long len = await RedisCache.Hash.FieldStringLengthAsync("user:1", "name");

// 迭代大型 Hash
var scan = await RedisCache.Hash.ScanAsync("user:1", 0, count: 100);
```

### List 操作 `RedisCache.List`

```csharp
// 头部插入 / 尾部插入
await RedisCache.List.SetFirstAsync("queue", "item1");
await RedisCache.List.SetLastAsync("queue", "item2");

// 弹出
string first = await RedisCache.List.GetAndRemoveFirstAsync("queue");
string last = await RedisCache.List.GetAndRemoveLastAsync("queue");

// 获取全部 / 区间元素
List<string> all = await RedisCache.List.GetAllAsync("queue");
List<string> range = await RedisCache.List.GetRangeAsync("queue", 0, 9);

// 按索引操作
string item = await RedisCache.List.GetByIndexAsync("queue", 3);
await RedisCache.List.SetByIndexAsync("queue", 3, "newvalue");

// 插入（在指定元素前/后）
await RedisCache.List.InsertBeforeAsync("queue", "pivot", "newitem");
await RedisCache.List.InsertAfterAsync("queue", "pivot", "newitem");

// 列表信息
long len = await RedisCache.List.LengthAsync("queue");

// 移除与裁剪
long removed = await RedisCache.List.RemoveAsync("queue", 0, "item1"); // 移除所有匹配
await RedisCache.List.TrimAsync("queue", 0, 99); // 保留前100个

// 转移元素
string moved = await RedisCache.List.PopLastPushFirstAsync("source", "target");
```

### Set 操作 `RedisCache.Set`

```csharp
// 添加 / 移除
await RedisCache.Set.SetAsync("tags", "redis", "csharp", "dotnet");
await RedisCache.Set.RemoveAsync("tags", "csharp");

// 查询
long count = await RedisCache.Set.CountAsync("tags");
bool exists = await RedisCache.Set.ExistsAsync("tags", "redis");
List<string> all = await RedisCache.Set.GetAllAsync("tags");

// 随机获取（不移除）
string r = await RedisCache.Set.RandomMemberAsync("tags");
string[] rs = await RedisCache.Set.RandomMembersAsync("tags", 3);

// 随机弹出（移除）
string popped = await RedisCache.Set.GetRandomAsync("tags");

// 移动成员
await RedisCache.Set.MoveAsync("tags", "archive", "redis");

// 集合运算
string[] diff = await RedisCache.Set.DifferenceAsync("a", "b");   // 差集
string[] inter = await RedisCache.Set.IntersectionAsync("a", "b"); // 交集
string[] union = await RedisCache.Set.UnionAsync("a", "b");       // 并集
await RedisCache.Set.IntersectionStoreAsync("dest", "a", "b");   // 交集存储到新集合

// 迭代大型集合
var scan = await RedisCache.Set.ScanAsync("tags", 0, count: 100);
```

### Sorted Set 操作 `RedisCache.Sort`

```csharp
// 添加（单个/批量）
await RedisCache.Sort.AddAsync("leaderboard", "player1", 100);
await RedisCache.Sort.SetMultipleAsync("leaderboard", (200, "p2"), (300, "p3"));

// 按分数范围查询
var top10 = await RedisCache.Sort.GetRevRangeByScoreWithScoresAsync("leaderboard", 1000, 0, count: 10);

// 按排名查询（升序/降序）
var rankRange = await RedisCache.Sort.GetRangeByRankWithScoresAsync("leaderboard", 0, 9);
var revRankRange = await RedisCache.Sort.GetRevRangeByRankWithScoresAsync("leaderboard", 0, 9);

// 排名 & 分数（降序/升序）
long? rank = await RedisCache.Sort.RankAsync("leaderboard", "player1");          // 降序
long? rankAsc = await RedisCache.Sort.RankAscendingAsync("leaderboard", "player1"); // 升序
decimal? score = await RedisCache.Sort.GetScoreAsync("leaderboard", "player1");

// 统计分数区间
long cnt = await RedisCache.Sort.CountByScoreAsync("leaderboard", 100, 500);

// 弹出最高分 / 最低分成员
string[] max = await RedisCache.Sort.MaxAsync("leaderboard", 3);
string[] min = await RedisCache.Sort.MinAsync("leaderboard", 3);

// 按分数区间移除
await RedisCache.Sort.RemoveByScoreAsync("leaderboard", 0, 50);

// 迭代大型有序集合
var scan = await RedisCache.Sort.ScanAsync("leaderboard", 0, count: 100);
```

### 分布式锁 `RedisCache.Utils`

```csharp
// 尝试获取锁（未拿到立即返回 null）
var redisLock = await RedisCache.Utils.LockAsync("resource-key", timeout: 30, autoDelay: true);
if (redisLock.IsLock)
{
    // 执行临界区代码
    redisLock.Unlock();
}

// 带超时等待的锁
var redisLock = await RedisCache.Utils.TryLockAsync(getTimeout: 5, "resource-key", timeout: 30);
```

`autoDelay: true` 开启看门狗线程，自动续期直到显式调用 `Unlock()`，进程异常退出不会导致死锁。

### 队列操作 `RedisCache.Utils`

```csharp
// 入队（默认去重）
await RedisCache.Utils.EnqueueAsync("tasks", taskObj);
await RedisCache.Utils.EnqueueAsync("tasks", taskObj, checkDuplicate: false);

// 出队
var task = await RedisCache.Utils.DequeueAsync<Task>("tasks");

// 队列长度
long len = await RedisCache.Utils.QueueCountAsync("tasks");

// 移除指定元素
await RedisCache.Utils.RemoveValueFromQueueAsync("tasks", targetObj);

// 检查元素是否存在
bool exists = await RedisCache.Utils.CheckValueExitsFromQueueAsync("tasks", targetObj);
```

### 键管理 `RedisCache.Key`

```csharp
bool exists = await RedisCache.Key.ExistsAsync("key");
await RedisCache.Key.ExpireAsync("key", TimeSpan.FromMinutes(30));
await RedisCache.Key.ExpireAtAsync("key", DateTime.Now.AddHours(1)); // Unix 时间戳过期
long ttl = await RedisCache.Key.TTLAsync("key");      // 秒
long pttl = await RedisCache.Key.PTTLAsync("key");     // 毫秒
await RedisCache.Key.PExpireAsync("key", 5000);         // 毫秒过期
await RedisCache.Key.PersistAsync("key");               // 移除过期时间
await RedisCache.Key.RenameAsync("old", "new");
await RedisCache.Key.RemoveAsync("key1", "key2");
string[] keys = await RedisCache.Key.GetKeysAsync("user:*");
var type = await RedisCache.Key.TypeAsync("key");       // KeyType 枚举
var random = await RedisCache.Key.RandomKeyAsync();
var allKeys = await RedisCache.Key.GetAllRedisKeysInfoAsync(); // 全量 key 列表 + 类型 + TTL

// 游标迭代（安全遍历大量 key）
var scan = await RedisCache.Key.ScanAsync(0, "user:*", count: 100);
```

### 发布/订阅 `RedisCache.Message`

```csharp
// 发布
RedisCache.Message.Publish("channel", new { Type = "notify", Content = "hello" });

// 订阅
var sub = RedisCache.Message.Subscribe("channel", msg => Console.WriteLine(msg));
var subT = RedisCache.Message.Subscribe<MyMsg>("channel", obj => Console.WriteLine(obj.Type));

// 模式订阅（通配符，集群安全）
var psub = RedisCache.Message.PSubscribe(e => Console.WriteLine(e.Body), "order:*", "user:*");
```

### 监控 `RedisCache.Monitor`

```csharp
var info = await RedisCache.Monitor.GetInfoAsync();

Console.WriteLine($"Redis 版本: {info.Server.RedisVersion}");
Console.WriteLine($"运行天数: {info.Server.UptimeInDays}");
Console.WriteLine($"内存使用: {info.Memory.UsedMemoryHuman}");
Console.WriteLine($"内存负载: {info.Memory.LoadDescription}");
Console.WriteLine($"缓存命中率: {info.Stats.LoadDescription}");
Console.WriteLine($"QPS: {info.Stats.InstantaneousOpsPerSec}");
Console.WriteLine($"客户端连接: {info.Clients.ConnectedClients}");
Console.WriteLine($"CPU 负载: {info.Cpu.LoadDescription}");
```

### 事务 & Lua 脚本 `RedisCache.Utils`

```csharp
// 类型化管道事务（推荐）— API 风格与 RedisCache 一致
var results = await RedisCache.Utils.PipeExecuteAsync(cache =>
{
    cache.String.SetAsync("k1", "v1");
    cache.String.AdditionAsync("counter", 1);
    cache.Hash.SetAsync("h1", "f1", "v1");
    return Task.CompletedTask;
});
// 注意：回调内不可 await 单个操作，所有命令排队后在回调返回时批量执行

// 原始管道事务（兼容旧版）
var results = await RedisCache.Utils.PipeExecuteAsync(async pipe =>
{
    await pipe.SetAsync("k1", "v1");
    await pipe.IncrByAsync("counter", 1);
});

// Lua 脚本
object result = await RedisCache.Utils.RunLuaScriptAsync("return redis.call('GET', KEYS[1])", "mykey");
```

### 缓存穿透防护 `RedisCacheService.CacheShell`

```csharp
// 多实例模式下
var svc = RedisCacheFactory.GetInstance("cache");
string data = await svc.CacheShell("key", TimeSpan.FromMinutes(5), async () =>
{
    return await FetchFromDbAsync(); // 穿透时回调
});
```

## 配置选项

| 参数 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Host` | string | — | Redis 服务地址 |
| `Port` | int | — | Redis 端口 |
| `UserName` | string | — | Redis 6.0+ ACL 用户名 |
| `Password` | string | — | 连接密码 |
| `UseDatabase` | int | `0` | 数据库编号 |
| `SSL` | bool | `false` | 是否启用 SSL |
| `TestCluster` | bool | `false` | 是否检测集群（阿里云/腾讯云集群需设为 `false`） |
| `Prefix` | string | — | 全局 Key 前缀 |
| `Poolsize` | int | `50` | 连接池大小 |
| `IdleTimeout` | int | `20000` | 连接空闲超时 (ms) |
| `ConnectTimeout` | int | `5000` | 连接超时 (ms) |
| `SyncTimeout` | int | `10000` | 发送/接收超时 (ms) |
| `AutoDispose` | bool | `true` | 跟随进程退出自动释放 |
| `RetryOnFailure` | int | `0` | 失败重试次数 |
| `Preheat` | int | `0` | 连接池预热连接数 |
| `AsyncPipeline` | bool | `false` | 是否自动使用异步 Pipeline |
| `NewtonsoftJsonSerializerSettings` | `JsonSerializerSettings` | — | 自定义 JSON 序列化配置 |
| `UseSentinels(params string[])` | 方法 | — | 启用哨兵模式 |

## 哨兵模式

```csharp
CSRedisCoreBuilder.AddSingleRedisInstance(opt =>
{
    opt.Password = "pwd";
    opt.UseSentinels("127.0.0.1:26379", "127.0.0.1:26380", "127.0.0.1:26381");
});
```

## JSON 序列化

默认配置：
- **CamelCase** 属性命名
- 日期格式 `yyyy-MM-dd HH:mm:ss.ffffff`
- 忽略 `null` 值
- `long` / `decimal` 输出为字符串（防止前端精度丢失）
- 最大嵌套深度 200

可通过 `NewtonsoftJsonSerializerSettings` 覆盖：

```csharp
CSRedisCoreBuilder.AddSingleRedisInstance(opt =>
{
    opt.Host = "127.0.0.1";
    opt.Port = 6379;
    opt.NewtonsoftJsonSerializerSettings = new JsonSerializerSettings
    {
        // 你的自定义配置
    };
});
```

## 直接访问底层 CSRedisClient

```csharp
// 单实例
var rawClient = RedisCache.CSRedisClient;

// 多实例
var rawClient = RedisCacheFactory.GetInstance("name").CSRedisClient;
```

## 许可

[MIT](LICENSE)
