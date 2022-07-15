using EDT.MSA.API.Shared.Utils;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MsgTrackerServiceCollectionExtensions
    {
        public static void AddMsgTracker(this IServiceCollection services)
        {
            services.AddSingleton<IMsgTracker, RedisMsgTracker>();
        }
    }
}
