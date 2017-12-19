using Basic.Code;
using King.Domain.Entity.HuaweiOrderManage;
using King.Domain.IRepository.HuaweiOrderManage;
using King.Repository.HuaweiOrderManage;
using System.Collections.Generic;
using System.Linq;

namespace King.Application.HuaweiOrderManage
{
    public class HWVendorItemApp
    {
        private IHWVendorItemRepository service = new HWVendorItemRepository();

        public List<HWVendorItemEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(t => t.F_CreateTime).ToList();
        }
        public HWVendorItemEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void SubmitForm(HWVendorItemEntity hwVendorItemEntity, string keyValue)
        {
            if (!keyValue.IsEmpty())
            {
                hwVendorItemEntity.Modify(keyValue);
                service.Update(hwVendorItemEntity);
            }
            else
            {
                hwVendorItemEntity.Create();
                service.Insert(hwVendorItemEntity);
            }
        }
    }
}
