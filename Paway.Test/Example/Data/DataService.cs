using Paway.Helper;
using Paway.Test.Properties;
using Paway.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class DataService : SQLiteHelper
    {
        private const string dbName = "Temp.db";
        private static DataService intance;
        public static DataService Default
        {
            get
            {
                if (intance == null)
                    intance = new DataService();
                return intance;
            }
        }
        public DataService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string file = Path.Combine(path, dbName);
            base.InitConnect(file);
            if (base.InitCreate(Resources.script))
            {
                UserInfo info = new UserInfo();
                info.Name = "admin";
                info.Pad = EncryptHelper.MD5("admin" + Config.Suffix);
                Insert(info);
            }
        }

        #region Admin.Update
        public void Update(string name, object value)
        {
            DbCommand cmd = null;
            try
            {
                cmd = TransStart();
                string find = "Name = @name";
                List<AdminBaseInfo> list = Find<AdminBaseInfo>(find, new { name }, cmd);
                if (list.Count == 0)
                {
                    AdminBaseInfo info = new AdminBaseInfo() { Name = name, Value = value.ToString(), DateTime = DateTime.Now };
                    Insert(info, cmd);
                }
                else
                {
                    list[0].Value = value.ToString();
                    list[0].DateTime = DateTime.Now;
                    Update(list[0], cmd);
                }

                TransCommit(cmd);
            }
            catch (Exception ex)
            {
                TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd);
            }
        }

        #endregion

        #region Logon
        public UserInfo EncryptLogin(string name, string pad)
        {
            return Login(name, EncryptHelper.MD5("admin" + Config.Suffix));
        }
        public UserInfo Login(string name, string pad)
        {
            var find = string.Format("Name = '{0}' and Pad = '{1}'", name, pad);
            List<UserInfo> list = Find<UserInfo>(find);
            if (list.Count == 0)
            {
                throw new WarningException("用户名或密码错误");
            }
            else
            {
                if (!list[0].Statu) throw new WarningException("用户已停用");
                list[0].DateTime = DateTime.Now;
                Update(list[0], nameof(UserInfo.DateTime));
                return list[0];
            }
        }
        public UserInfo Login(int userId)
        {
            var info = Find<UserInfo>(userId);
            if (info == null)
            {
                throw new WarningException("用户不存在");
            }
            else
            {
                if (!info.Statu) throw new WarningException("用户已停用");
                info.DateTime = DateTime.Now;
                Update(info, nameof(UserInfo.DateTime));
                return info;
            }
        }
        public void UpdatePad(string pad, string newPad, string newPad2)
        {
            if (Config.User == null) throw new WarningException("用户尚未登陆");
            if (newPad != newPad2) throw new WarningException("两次输入密码不一致");
            if (Config.User.Pad != EncryptHelper.MD5(pad + Config.Suffix)) throw new WarningException("用户密码错误");
            Config.User.DateTime = DateTime.Now;
            Config.User.Pad = EncryptHelper.MD5(newPad + Config.Suffix);
            Update(Config.User, nameof(UserInfo.DateTime), nameof(UserInfo.Pad));
        }

        #endregion

        #region 初始化
        public void Load(BackgroundWorker bw, MType type)
        {
            DbCommand cmd = null;
            try
            {
                cmd = TransStart();

                switch (type)
                {
                    //基础数据
                    case MType.Win:
                        Config.Admin = FindAdmin(cmd);
                        Cache.UserList.AddRange(Find<UserInfo>());
                        break;
                }

                TransCommit(cmd);
            }
            catch (Exception ex)
            {
                TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd);
            }
        }
        public AdminInfo FindAdmin(DbCommand cmd = null)
        {
            List<AdminBaseInfo> temp = Find<AdminBaseInfo>(cmd);
            List<IInfo> list = new List<IInfo>();
            list.AddRange(temp);
            return TMethod.Conversion<AdminInfo, IInfo>(list);
        }

        #endregion
    }
}
