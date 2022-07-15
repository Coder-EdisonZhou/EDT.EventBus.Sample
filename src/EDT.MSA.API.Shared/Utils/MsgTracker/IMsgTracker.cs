using System.Threading.Tasks;

namespace EDT.MSA.API.Shared.Utils
{
    public interface IMsgTracker
    {
        Task<bool> HasProcessed(string msgId);
        Task MarkAsProcessed(string msgId);
    }
}
