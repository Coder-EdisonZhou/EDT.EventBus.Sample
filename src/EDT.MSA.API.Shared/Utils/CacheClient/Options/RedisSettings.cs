namespace EDT.MSA.API.Shared.Utils
{
    public class RedisSettings : IRedisSettings
    {
        /// <summary>
        /// 连接主机（含端口）
        /// </summary>
        public string Hosts { get; set; } = string.Empty;

        /// <summary>
        /// 连接密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 默认使用的数据库编号（0~15）
        /// </summary>
        public string DefaultDatabase { get; set; } = "0";

        /// <summary>
        /// 连接池大小（默认50）
        /// </summary>
        public int PoolSize { get; set; } = 50;

        /// <summary>
        /// 是否预热连接，默认为True
        /// </summary>
        public bool Preheat { get; set; } = true;

        /// <summary>
        /// 是否开启加密传输，默认为False
        /// </summary>
        public bool SSL { get; set; } = false;

        /// <summary>
        /// 异步方法写入缓冲区大小 (字节)，默认为1M
        /// </summary>
        public int WriteBuffer { get; set; } = 10240;

        /// <summary>
        /// 统一设置缓存过期时间，默认为60s
        /// </summary>
        public int ExpireSeconds { get; set; } = 60;

        /// <summary>
        /// 执行命令出错，尝试重试的次数
        /// </summary>
        public int TryIt { get; set; } = 0;

        /// <summary>
        /// 连接名称，可以使用 Client List 命令查看
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// key前辍，所有方法都会附带此前辍，Set(prefix + "key", 111);
        /// </summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// 哨兵集群Master节点名字（默认为mymaster）
        /// </summary>
        public string MasterName { get; set; } = "mymaster";

        /// <summary>
        /// 哨兵集群Hosts（含端口）
        /// </summary>
        public string[] SentinelHosts { get; set; } = null;

        public string GetConnectionString()
        {
            // "127.0.0.1:6379,password=123,defaultDatabase=13,poolsize=50,preheat=true,ssl=false,writeBuffer=10240,tryit=0,name=clientName,prefix=key前辍"
            var isCluster = SentinelHosts != null;
            var connStr = !isCluster ?
                $"{Hosts},password={Password},defaultDatabase={DefaultDatabase},poolsize={PoolSize},preheat={Preheat},ssl={SSL},writeBuffer={WriteBuffer},tryit={TryIt}"
                : $"{MasterName},password={Password},defaultDatabase={DefaultDatabase},poolsize={PoolSize},preheat={Preheat},ssl={SSL},writeBuffer={WriteBuffer},tryit={TryIt}";

            if (!string.IsNullOrWhiteSpace(ClientName))
            {
                connStr += $",name={ClientName}";
            }

            if (!string.IsNullOrWhiteSpace(Prefix))
            {
                connStr += $",prefix={Prefix}";
            }

            return connStr;
        }
    }

    public interface IRedisSettings
    {
        string Hosts { get; set; }

        string Password { get; set; }

        string DefaultDatabase { get; set; }

        int PoolSize { get; set; }

        bool Preheat { get; set; }

        bool SSL { get; set; }

        int WriteBuffer { get; set; }

        int ExpireSeconds { get; set; }

        int TryIt { get; set; }

        string MasterName { get; set; }

        string[] SentinelHosts { get; set; }

        string GetConnectionString();
    }
}
