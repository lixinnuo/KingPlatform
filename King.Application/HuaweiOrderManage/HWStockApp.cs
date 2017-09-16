using Basic.Code;
using King.Domain.Entity.HuaweiOrderManage;
using King.Domain.IRepository.HuaweiOrderManage;
using King.Repository.HuaweiOrderManage;
using System.Collections.Generic;
using System.Linq;

namespace King.Application.HuaweiOrderManage
{
    public class HWStockApp
    {
        private IHWStockRepository service = new HWStockRepository();

        public List<HWStockEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(t => t.F_CreateTime).ToList();
        }
        public HWStockEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void SubmitForm(HWStockEntity hwStockEntity, string keyValue)
        {
            if (!keyValue.IsEmpty())
            {
                hwStockEntity.Modify(keyValue);
                service.Update(hwStockEntity);
            }
            else
            {
                hwStockEntity.Create();
                service.Insert(hwStockEntity);
            }
        }
    }
}
