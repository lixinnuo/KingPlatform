using System;

namespace King.Domain
{
    public interface IModificationAudited
    {
        string F_Id { get; set; }

        string F_LastModifyUserId { get; set; }

        DateTime? F_LastModifyTime { get; set; }
    }
}
