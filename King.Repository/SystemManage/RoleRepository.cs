using Basic.Code;
using King.Domain.Context;
using King.Domain.Entity.SystemManage;
using King.Domain.IRepository.SystemManage;
using System.Collections.Generic;

namespace King.Repository.SystemManage
{
    public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<RoleEntity>(t => t.F_Id == keyValue);
                db.Delete<RoleAuthorizeEntity>(t => t.F_ObjectId == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(RoleEntity roleEntity, List<RoleAuthorizeEntity> roleAuthorizeEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!keyValue.IsEmpty())
                {
                    db.Update(roleEntity);
                }
                else
                {
                    roleEntity.F_Category = 1;
                    db.Insert(roleEntity);
                }
                db.Delete<RoleAuthorizeEntity>(t => t.F_ObjectId == roleEntity.F_Id);
                db.Insert(roleAuthorizeEntity);
                db.Commit();
            }
        }
    }
}
