using System;

namespace EDT.MSA.API.Shared.Models
{
    public class MsgTrackLog
    {
        public MsgTrackLog(string msgId)
        {
            MsgId = msgId;
            CreatedDate = DateTime.Now;
        }

        public string MsgId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
