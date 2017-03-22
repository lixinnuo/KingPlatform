using Basic.Code;
using King.Domain.Entity.SystemManage;
using King.Domain.IRepository.SystemManage;
using King.Repository.SystemManage;

namespace King.Application.SystemManage
{
    public class UserLogOnApp
    {
        private IUserLogOnRepository server = new UserLogOnRepository();

        public UserLogOnEntity GetForm(string keyValue)
        {
            return server.FindEntity(keyValue);
        }
        public void UpdateForm(UserLogOnEntity userLogOnEntiry)
        {
            server.Update(userLogOnEntiry);
        }
        public void RevisePassword(string userPassword, string keyValue)
        {
            UserLogOnEntity userLogOnEntity = new UserLogOnEntity();
            userLogOnEntity.F_Id = keyValue;
            userLogOnEntity.F_UserSecretkey = Md5.md5(Common.CreateNo(), 16).ToLower();
            userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
            server.Update(userLogOnEntity);
        }
    }
}
