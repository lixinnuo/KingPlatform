using Basic.Data;
using King.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace King.Domain.IRepository.SystemManage
{
    public interface IModuleButtonRepository : IRepositoryBase<ModuleButtonEntity>
    {
        void SubmitCloneButton(List<ModuleButtonEntity> entitys);
    }
}
