using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class Cache
    {
        #region 用户缓存
        private static object ULock = new object();
        private static List<UserInfo> _uList;
        /// <summary>
        /// 用户列表
        /// </summary>
        public static List<UserInfo> UserList
        {
            get
            {
                if (_uList == null)
                    _uList = new List<UserInfo>();
                return _uList;
            }
        }
        public static void Add(UserInfo info)
        {
            lock (ULock)
                UserList.Add(info);
        }
        public static void Delete(UserInfo info)
        {
            lock (ULock)
            {
                UserInfo temp = UserList.Find(c => c.Id == info.Id);
                if (temp != null)
                    UserList.Remove(temp);
            }
        }
        public static UserInfo Find(long id)
        {
            return UserList.Find(c => c.Id == id);
        }
        public static UserInfo Find(string name)
        {
            return UserList.Find(c => c.Name == name);
        }
        public static string Name(long id)
        {
            UserInfo info = UserList.Find(c => c.Id == id);
            return info == null ? string.Empty : info.Name;
        }

        #endregion
    }
}
