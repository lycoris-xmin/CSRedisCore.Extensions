# 可补齐方法清单

本文档对比现有接口与 Redis 标准命令，列出每个模块缺失的方法。CSRedisCore 底层已支持这些命令，只需在接口和实现中添加包装即可。

---

## 1. IRedisStringCache — 字符串操作

### 缺失方法

| 方法名 | 对应 Redis 命令 | 说明 |
|--------|----------------|------|
| `StrLen` / `StrLenAsync` | STRLEN | 获取字符串值的长度 |
| `Append` / `AppendAsync` | APPEND | 追加内容到已有字符串末尾，返回新长度 |
| `GetRange` / `GetRangeAsync` | GETRANGE | 获取子字符串 |
| `SetRange` / `SetRangeAsync` | SETRANGE | 覆盖指定偏移位置的子字符串 |
| `Incr` / `IncrAsync` | INCR | 自增 1（当前 Addition 默认 value=1 等价，但命名不够直观） |
| `Decr` / `DecrAsync` | DECR | 自减 1 |
| `IncrByFloat` / `IncrByFloatAsync` | INCRBYFLOAT | 浮点增量 |
| `SetIfExists` / `SetIfExistsAsync` | SET ... XX | 仅当 key 已存在时才设置 |
| `GetSet` sync version (T) | GETSET | 当前缺少泛型同步版本 `GetSet<T>` sync（仅有 async） |

> **备注**：`Incr` / `Decr` 可以在内部委托给 `Addition(key, 1)` / `Subtraction(key, 1)`，作为语义更清晰的快捷方法。

---

## 2. IRedisHashCache — 哈希操作

### 缺失方法

| 方法名 | 对应 Redis 命令 | 说明 |
|--------|----------------|------|
| `Values` / `ValuesAsync` | HVALS | 获取哈希表中所有字段的值（不返回键名） |
| `StrLen` / `StrLenAsync` | HSTRLEN | 获取指定字段的值的字符串长度 |
| `IncrByFloat` / `IncrByFloatAsync` | HINCRBYFLOAT | 浮点增量 |
| `Scan` / `ScanAsync` | HSCAN | 迭代遍历大型哈希表 |

---

## 3. IRedisListCache — 列表操作

当前仅有 5 个操作用法（GetAndRemoveFirst、GetAndRemoveLast、SetFirst、SetLast、GetAllAsync），是**缺口最大的模块**。

### 缺失方法

| 方法名 | 对应 Redis 命令 | 说明 |
|--------|----------------|------|
| `GetByIndex` / `GetByIndexAsync` | LINDEX | 通过索引获取元素 |
| `GetRange` / `GetRangeAsync` | LRANGE | 获取指定区间元素（支持 start/stop） |
| `GetAll` | — | 同步版本 `GetAll`（当前仅有异步） |
| `InsertBefore` / `InsertBeforeAsync` | LINSERT BEFORE | 在指定元素前插入 |
| `InsertAfter` / `InsertAfterAsync` | LINSERT AFTER | 在指定元素后插入 |
| `SetByIndex` / `SetByIndexAsync` | LSET | 通过索引设置元素值 |
| `Length` / `LengthAsync` | LLEN | 获取列表长度 |
| `Remove` / `RemoveAsync` | LREM | 移除指定数量的匹配元素 |
| `Trim` / `TrimAsync` | LTRIM | 保留指定区间元素，丢弃其余 |
| `PopFirstBlocking` / `PopFirstBlockingAsync` | BLPOP | 阻塞式弹出头部元素（支持超时） |
| `PopLastBlocking` / `PopLastBlockingAsync` | BRPOP | 阻塞式弹出尾部元素（支持超时） |
| `PopLastPushFirst` / `PopLastPushFirstAsync` | RPOPLPUSH | 从源列表尾部弹出，推入目标列表头部 |
| `PopLastPushFirstBlocking` ... | BRPOPLPUSH | 阻塞版 RPOPLPUSH |

---

## 4. IRedisSetCache — 集合操作

### 缺失方法

| 方法名 | 对应 Redis 命令 | 说明 |
|--------|----------------|------|
| `Remove` / `RemoveAsync` | SREM | 移除集合中一个或多个成员 |
| `RandomMember` / `RandomMemberAsync` | SRANDMEMBER | 随机获取成员（**不移除**，当前 GetRandom 是 SPOP 会移除） |
| `RandomMembers` / `RandomMembersAsync` | SRANDMEMBER count | 随机获取多个成员（不移除） |
| `Move` / `MoveAsync` | SMOVE | 将成员从源集合移动到目标集合 |
| `Diff` / `DiffAsync` | SDIFF | 多个集合的差集 |
| `DiffStore` / `DiffStoreAsync` | SDIFFSTORE | 差集并存储到新集合 |
| `Inter` / `InterAsync` | SINTER | 多个集合的交集 |
| `InterStore` / `InterStoreAsync` | SINTERSTORE | 交集并存储到新集合 |
| `Union` / `UnionAsync` | SUNION | 多个集合的并集 |
| `UnionStore` / `UnionStoreAsync` | SUNIONSTORE | 并集并存储到新集合 |
| `Scan` / `ScanAsync` | SSCAN | 迭代遍历大型集合 |
| `GetAll` | — | 同步版本（当前仅有异步） |

---

## 5. IRedisSortCache — 有序集合操作

### 缺失方法

| 方法名 | 对应 Redis 命令 | 说明 |
|--------|----------------|------|
| `Count` by score | ZCOUNT | 统计分数区间内的成员数量（当前 `Count` 是 ZCARD 总成员数） |
| `RangeByRank` / `RangeByRankAsync` | ZRANGE | 按排名（索引）返回成员（升序） |
| `RevRangeByRank` / `RevRangeByRankAsync` | ZREVRANGE | 按排名返回成员（降序） |
| `RangeByScore` (member only) | ZRANGEBYSCORE | 按分数返回成员**不带**分数值（当前只有 WithScores 版本） |
| `RevRangeByScore` (member only) | ZREVRANGEBYSCORE | 按分数降序返回成员**不带**分数值 |
| `Rank` (ascending) | ZRANK | 升序排名（当前 `Rank` 实现的是 ZREVRANK 降序） |
| `RemoveByScore` / `RemoveByScoreAsync` | ZREMRANGEBYSCORE | 按分数区间移除成员 |
| `RemoveByLex` / `RemoveByLexAsync` | ZREMRANGEBYLEX | 按字典序区间移除成员 |
| `LexCount` / `LexCountAsync` | ZLEXCOUNT | 字典序区间成员数量 |
| `RangeByLex` / `RangeByLexAsync` | ZRANGEBYLEX | 按字典序返回成员 |
| `Scan` / `ScanAsync` | ZSCAN | 迭代遍历大型有序集合 |
| `GetAll` | — | 同步版本 |
| `AddMultiple` / `AddMultipleAsync` | ZADD multiple | 批量添加多个成员（当前 Add 只支持单个） |

---

## 6. IRedisKeyCache — 键管理

### 缺失方法

| 方法名 | 对应 Redis 命令 | 说明 |
|--------|----------------|------|
| `PTTL` / `PTTLAsync` | PTTL | 以毫秒返回剩余生存时间（当前 TTL 返回秒） |
| `ExpireAt` / `ExpireAtAsync` | EXPIREAT | 以 Unix 时间戳设置过期时间（秒） |
| `PExpire` / `PExpireAsync` | PEXPIRE | 以毫秒设置过期时间 |
| `PExpireAt` / `PExpireAtAsync` | PEXPIREAT | 以毫秒级 Unix 时间戳设置过期时间 |
| `Type` / `TypeAsync` | TYPE | 获取键的数据类型 |
| `Unlink` / `UnlinkAsync` | UNLINK | 非阻塞删除（仅标记删除，异步回收内存） |
| `Scan` / `ScanAsync` | SCAN | 游标迭代所有键（内部已用，未公开） |
| `Dump` / `DumpAsync` | DUMP | 序列化键的值 |
| `Restore` / `RestoreAsync` | RESTORE | 反序列化恢复键 |
| `RandomKey` / `RandomKeyAsync` | RANDOMKEY | 随机返回一个 key |
| `Move` / `MoveAsync` | MOVE | 将 key 移动到另一个数据库 |
| `Touch` / `TouchAsync` | TOUCH | 修改最后访问时间，不改变值 |
| `ObjectEncoding` / `ObjectEncodingAsync` | OBJECT ENCODING | 获取值内部编码方式 |
| `ObjectIdleTime` / `ObjectIdleTimeAsync` | OBJECT IDLETIME | 获取空闲时间 |
| `ObjectRefCount` / `ObjectRefCountAsync` | OBJECT REFCOUNT | 获取引用计数 |

---

## 7. IRedisEventMessage — 发布/订阅

### 缺失方法

| 方法名 | 对应 Redis 命令 | 说明 |
|--------|----------------|------|
| `Unsubscribe` / `UnsubscribeAsync` | UNSUBSCRIBE | 退订指定频道 |
| `PUnsubscribe` / `PUnsubscribeAsync` | PUNSUBSCRIBE | 退订模式订阅 |

> **备注**：CSRedisCore 的 `SubscribeObject` / `PSubscribeObject` 自带 `Unsubscribe()` 方法，接口层面可包装为独立方法方便调用。

---

## 8. 全新模块建议

以下 Redis 数据类型在当前项目中完全没有封装，属于**新模块**级别的工作量：

### 8.1 GEO — 地理位置

| 建议接口 | 方法 |
|----------|------|
| `IRedisGeoCache` | `GeoAdd`, `GeoPos`, `GeoDist`, `GeoHash`, `GeoRadius`, `GeoRadiusByMember`, `GeoSearch`, `GeoSearchStore` |

### 8.2 HyperLogLog — 基数统计

| 建议接口 | 方法 |
|----------|------|
| `IRedisHyperLogLogCache` | `PfAdd`, `PfCount`, `PfMerge` |

### 8.3 Bitmap / BitField — 位图操作

| 建议接口 | 方法 |
|----------|------|
| `IRedisBitCache` | `SetBit`, `GetBit`, `BitCount`, `BitPos`, `BitOp`, `BitField` |

### 8.4 Stream — 消息队列（Redis 5.0+）

| 建议接口 | 方法 |
|----------|------|
| `IRedisStreamCache` | `XAdd`, `XRead`, `XReadGroup`, `XRange`, `XRevRange`, `XLen`, `XDel`, `XTrim`, `XGroup`, `XAck`, `XPending`, `XClaim`, `XInfo` |

---

## 9. 结构性问题（非方法缺失）

### 9.1 同步/异步不一致

| 位置 | 问题 |
|------|------|
| `IRedisListCache` | `GetAllAsync` 有异步版本但无同步版本 |
| `IRedisSetCache` | `GetAllAsync` 有异步版本但无同步版本 |
| `IRedisSortCache` | `GetAllAsync` 有异步版本但无同步版本 |
| `IRedisStringCache` | `SetIfNotExists` 仅有 `Async` 版本，无同步版本 |
| `IRedisStringCache` | `GetSet<T>` 的 sync 版本签名有歧义 |

### 9.2 接口命名不一致

| 接口 | 方法名 | 问题 |
|------|--------|------|
| `IRedisSetCache` | `Set(string key, params string[] value)` | 与 String 的 `Set` 语义完全不同，但方法名相同。建议改为 `Add` |
| `IRedisSortCache` | `Max` / `Min` | 实际行为是 `ZPopMax` / `ZPopMin`，会**移除**成员，方法名未体现 Pop 语义，容易误用 |
| `IRedisHashCache` | `FieIds` / `FieIdsCount` | 拼写偏误，应为 `Fields` / `FieldCount`（但不影响使用） |

### 9.3 缺少的能力

| 问题 | 说明 |
|------|------|
| 无连接健康检查 | 缺少 `Ping` / `PingAsync` 方法暴露给用户检查连接状态 |
| 无批量 Key 过期 | 无法一次性为多个 key 设置过期时间 |
| 无原子批量删除 | `Unlink`（非阻塞）未在接口中暴露 |
| Key 前缀问题 | 多实例模式 `RedisCacheService` 内部使用 `RedisStore.PrefixCacheKey`（静态），未能正确隔离各实例的前缀 |

---

## 10. 优先级建议

### P0 — 日常高频使用

- `IRedisListCache`：`Length`、`GetRange`、`Remove`、`GetByIndex`、`Trim`
- `IRedisSetCache`：`Remove`、`RandomMember`（不移除）、`Diff`/`Inter`/`Union`
- `IRedisSortCache`：`Count`（按分数）、`RangeByRank`、`RemoveByScore`
- `IRedisKeyCache`：`Type`、`Unlink`、`Scan`（公开）

### P1 — 补齐完整性

- `IRedisStringCache`：`StrLen`、`Append`、`Incr`/`Decr`
- `IRedisHashCache`：`Values`、`StrLen`、`Scan`
- `IRedisKeyCache`：`PTTL`、`PExpire`
- `IRedisEventMessage`：`Unsubscribe` / `PUnsubscribe`
- List 阻塞弹出（`BLPop`/`BRPop`）

### P2 — 高级场景

- `IRedisListCache`：`LInsert`、`LSET`、`RPOPLPUSH`
- `IRedisSetCache`：`SMove`、集合运算存储（`*Store`）
- `IRedisSortCache`：`AddMultiple`、`RangeByLex`
- `IRedisKeyCache`：`Dump`/`Restore`、`Move`、`Touch`

### P3 — 新模块

- `GEO` — 地理位置场景
- `HyperLogLog` — UV 统计
- `Bitmap` — 签到/权限位
- `Stream` — 消息队列
