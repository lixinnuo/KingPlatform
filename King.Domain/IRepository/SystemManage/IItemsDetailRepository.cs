using Basic.Data;
using King.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace King.Domain.IRepository.SystemManage
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetailEntity>
    {
        List<ItemsDetailEntity> GetItemList(string enCode);
    }
}
