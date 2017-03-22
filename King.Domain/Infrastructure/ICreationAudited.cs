using System;

namespace King.Domain
{
    public interface ICreationAudited
    {
        string F_Id { get; set; }

        string F_CreateUserId { get; set; }

        DateTime? F_CreateTime { get; set; }
    }
}
