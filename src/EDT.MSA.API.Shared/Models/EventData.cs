using EDT.MSA.API.Shared.Utils;
using System;

namespace EDT.MSA.API.Shared.Models
{
    public class EventData<T> where T : class
    {
        public string Id { get; set; }

        public T MessageBody { get; set; }

        public DateTime CreatedDate { get; set; }

        public EventData(T messageBody)
        {
            MessageBody = messageBody;
            CreatedDate = DateTime.Now;
            Id = SnowflakeGenerator.Instance().GetId().ToString();
        }
    }
}
