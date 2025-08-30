using CSRedis;
using Lycoris.CSRedisCore.Extensions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class MonitorService : IMonitorService
    {
        /// <summary>
        /// redis实例
        /// </summary>
        private readonly CSRedisClient CSRedisCore;

        /// <summary>
        /// Json序列化配置
        /// </summary>
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cSRedisCore"></param>
        /// <param name="jsonSetting"></param>
        public MonitorService(CSRedisClient cSRedisCore, JsonSerializerSettings jsonSetting)
        {
            CSRedisCore = cSRedisCore;
            JsonSetting = jsonSetting;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<RedisInfoModel> GetInfoAsync()
        {
            var model = new RedisInfoModel();
            var infos = await CSRedisCore.NodesServerManager.InfoAsync();

            var infoString = infos.FirstOrDefault().value;
            if (string.IsNullOrWhiteSpace(infoString))
                return model;

            var sections = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> currentSection = null;
            string currentSectionName = "";

            foreach (var line in infoString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("#"))
                {
                    currentSectionName = line.Substring(1).Trim();
                    currentSection = new Dictionary<string, string>();
                    sections[currentSectionName] = currentSection;
                }
                else if (currentSection != null)
                {
                    var parts = line.Split(new[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        currentSection[parts[0]] = parts[1];
                    }
                }
            }

            // Server
            if (sections.ContainsKey("Server"))
            {
                var server = sections["Server"];
                if (server.TryGetValue("redis_version", out var version))
                    model.Server.RedisVersion = version;
                if (server.TryGetValue("os", out var os))
                    model.Server.Os = os;
                if (server.TryGetValue("uptime_in_seconds", out var uptimeSec))
                    model.Server.UptimeInSeconds = long.Parse(uptimeSec);
                if (server.TryGetValue("uptime_in_days", out var uptimeDays))
                    model.Server.UptimeInDays = long.Parse(uptimeDays);
            }

            // Clients
            if (sections.ContainsKey("Clients"))
            {
                var clients = sections["Clients"];
                if (clients.TryGetValue("connected_clients", out var clientsStr))
                    model.Clients.ConnectedClients = int.Parse(clientsStr);
            }

            // Memory
            if (sections.ContainsKey("Memory"))
            {
                var memory = sections["Memory"];
                if (memory.TryGetValue("used_memory", out var usedMem))
                    model.Memory.UsedMemory = long.Parse(usedMem);
                if (memory.TryGetValue("used_memory_human", out var usedMemHuman))
                    model.Memory.UsedMemoryHuman = usedMemHuman;
                if (memory.TryGetValue("mem_fragmentation_ratio", out var frag))
                    model.Memory.MemFragmentationRatio = double.Parse(frag);
                if (memory.TryGetValue("maxmemory", out var maxMem))
                    model.Memory.MaxMemory = long.Parse(maxMem);
                if (memory.TryGetValue("maxmemory_human", out var maxMemHuman))
                    model.Memory.MaxMemoryHuman = maxMemHuman;
            }

            // Stats
            if (sections.ContainsKey("Stats"))
            {
                var stats = sections["Stats"];
                if (stats.TryGetValue("total_connections_received", out var totalConn))
                    model.Stats.TotalConnectionsReceived = long.Parse(totalConn);
                if (stats.TryGetValue("total_commands_processed", out var totalCmd))
                    model.Stats.TotalCommandsProcessed = long.Parse(totalCmd);
                if (stats.TryGetValue("keyspace_hits", out var hits))
                    model.Stats.KeyspaceHits = long.Parse(hits);
                if (stats.TryGetValue("keyspace_misses", out var misses))
                    model.Stats.KeyspaceMisses = long.Parse(misses);
                if (stats.TryGetValue("rejected_connections", out var rejected))
                    model.Stats.RejectedConnections = long.Parse(rejected);
                if (stats.TryGetValue("expired_keys", out var expired))
                    model.Stats.ExpiredKeys = long.Parse(expired);
                if (stats.TryGetValue("evicted_keys", out var evicted))
                    model.Stats.EvictedKeys = long.Parse(evicted);
                if (stats.TryGetValue("instantaneous_ops_per_sec", out var ops))
                    model.Stats.InstantaneousOpsPerSec = long.Parse(ops);
            }

            // CPU
            if (sections.ContainsKey("CPU"))
            {
                var cpu = sections["CPU"];
                if (cpu.TryGetValue("used_cpu_sys", out var sys))
                    model.Cpu.UsedCpuSys = double.Parse(sys);
                if (cpu.TryGetValue("used_cpu_user", out var user))
                    model.Cpu.UsedCpuUser = double.Parse(user);
                if (cpu.TryGetValue("used_cpu_sys_children", out var sysChild))
                    model.Cpu.UsedCpuSysChildren = double.Parse(sysChild);
                if (cpu.TryGetValue("used_cpu_user_children", out var userChild))
                    model.Cpu.UsedCpuUserChildren = double.Parse(userChild);
            }

            // Keyspace
            if (sections.ContainsKey("Keyspace"))
            {
                var keyspace = sections["Keyspace"];
                if (keyspace.TryGetValue("db0", out var db0))
                {
                    var parts = db0.Split(',');
                    foreach (var part in parts)
                    {
                        var kv = part.Split('=');
                        if (kv.Length == 2)
                        {
                            switch (kv[0])
                            {
                                case "keys": model.Keyspace.Keys = int.Parse(kv[1]); break;
                                case "expires": model.Keyspace.Expires = int.Parse(kv[1]); break;
                                case "avg_ttl": model.Keyspace.AvgTtl = long.Parse(kv[1]); break;
                            }
                        }
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="rawInfo"></param>
        /// <returns></returns>
        private RedisInfoModel ParseRedisInfo(RedisInfoModel info, string rawInfo)
        {
            var lines = rawInfo.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.StartsWith("#")) continue;

                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length != 2) continue;

                var key = parts[0];
                var value = parts[1].Replace("\r", "");

                switch (key)
                {
                    // Server Info
                    case "redis_version":
                        info.Server.RedisVersion = value;
                        break;
                    case "os":
                        info.Server.Os = value;
                        break;
                    case "uptime_in_seconds":
                        info.Server.UptimeInSeconds = long.Parse(value);
                        break;
                    case "uptime_in_days":
                        info.Server.UptimeInDays = long.Parse(value);
                        break;

                    // Clients Info
                    case "connected_clients":
                        info.Clients.ConnectedClients = int.Parse(value);
                        break;

                    // Memory Info
                    case "used_memory":
                        info.Memory.UsedMemory = long.Parse(value);
                        break;
                    case "used_memory_human":
                        info.Memory.UsedMemoryHuman = value;
                        break;
                    case "mem_fragmentation_ratio":
                        info.Memory.MemFragmentationRatio = double.Parse(value);
                        break;

                    // Stats Info
                    case "total_connections_received":
                        info.Stats.TotalConnectionsReceived = long.Parse(value);
                        break;
                    case "total_commands_processed":
                        info.Stats.TotalCommandsProcessed = long.Parse(value);
                        break;
                    case "keyspace_hits":
                        info.Stats.KeyspaceHits = long.Parse(value);
                        break;
                    case "keyspace_misses":
                        info.Stats.KeyspaceMisses = long.Parse(value);
                        break;

                    // Keyspace Info
                    case "db0":
                        var dbInfo = value.Split(new[] { ',', '=' });
                        info.Keyspace.Keys = int.Parse(dbInfo[1]);
                        info.Keyspace.Expires = int.Parse(dbInfo[3]);
                        info.Keyspace.AvgTtl = long.Parse(dbInfo[5]);
                        break;

                    // CPU Info
                    case "used_cpu_sys":
                        info.Cpu.UsedCpuSys = double.Parse(value);
                        break;
                    case "used_cpu_user":
                        info.Cpu.UsedCpuUser = double.Parse(value);
                        break;
                    case "used_cpu_sys_children":
                        info.Cpu.UsedCpuSysChildren = double.Parse(value);
                        break;
                    case "used_cpu_user_children":
                        info.Cpu.UsedCpuUserChildren = double.Parse(value);
                        break;
                }
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private List<(string node, string value)> RedisInfo(InfoSection? section = null)
        {
            var result = CSRedisCore.NodesServerManager.Info(section);
            return result.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private async Task<List<(string node, string value)>> RedisInfoAsync(InfoSection? section = null)
        {
            var result = await CSRedisCore.NodesServerManager.InfoAsync(section);
            return result.ToList();
        }
    }
}
