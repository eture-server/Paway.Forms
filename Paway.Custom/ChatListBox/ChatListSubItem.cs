using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Paway.Custom
{
    //有待解决
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChatListSubItem : IComparable
    {
        //在线状态
        public enum UserStatus
        {
            QMe,
            Online,
            Away,
            Busy,
            DontDisturb,
            OffLine //貌似对于列表而言 没有隐身状态
        }

        private string displayName;

        private Image headImage;

        private string ipAddress;

        private bool isTwinkle;

        private string nicName;

        private string personalMsg;

        private UserStatus status;

        public ChatListSubItem()
        {
            status = UserStatus.Online;
            displayName = "displayName";
            nicName = "nicName";
            personalMsg = "Personal Message ...";
        }

        public ChatListSubItem(string nicname)
        {
            nicName = nicname;
        }

        public ChatListSubItem(string nicname, UserStatus status)
        {
            nicName = nicname;
            this.status = status;
        }

        public ChatListSubItem(string nicname, string displayname, string personalmsg)
        {
            nicName = nicname;
            displayName = displayname;
            personalMsg = personalmsg;
        }

        public ChatListSubItem(string nicname, string displayname, string personalmsg, UserStatus status)
        {
            nicName = nicname;
            displayName = displayname;
            personalMsg = personalmsg;
            this.status = status;
        }

        public ChatListSubItem(int id, string nicname, string displayname, string personalmsg, UserStatus status,
            Bitmap head)
        {
            ID = id;
            nicName = nicname;
            displayName = displayname;
            personalMsg = personalmsg;
            this.status = status;
            headImage = head;
        }

        /// <summary>
        ///     获取或者设置用户账号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     获取或者设置用户昵称
        /// </summary>
        public string NicName
        {
            get { return nicName; }
            set
            {
                nicName = value;
                RedrawSubItem();
            }
        }

        /// <summary>
        ///     获取或者设置用户备注名称
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;
                RedrawSubItem();
            }
        }

        /// <summary>
        ///     获取或者设置用户签名信息
        /// </summary>
        public string PersonalMsg
        {
            get { return personalMsg; }
            set
            {
                personalMsg = value;
                RedrawSubItem();
            }
        }

        /// <summary>
        ///     获取或者设置用户IP地址
        /// </summary>
        public string IpAddress
        {
            get { return ipAddress; }
            set
            {
                if (value != null && !CheckIpAddress(value))
                    throw new ArgumentException("Cannot format " + value + " to IPAddress");
                ipAddress = value;
            }
        }

        /// <summary>
        ///     获取或者设置用户Upd端口
        /// </summary>
        public int UpdPort { get; set; }

        /// <summary>
        ///     获取或者设置用户Tcp端口
        /// </summary>
        public int TcpPort { get; set; }

        /// <summary>
        ///     获取或者设置用户头像
        /// </summary>
        public Image HeadImage
        {
            get { return headImage; }
            set
            {
                headImage = value;
                RedrawSubItem();
            }
        }

        /// <summary>
        ///     获取或者设置用户当前状态
        /// </summary>
        public UserStatus Status
        {
            get { return status; }
            set
            {
                if (status == value) return;
                status = value;
                if (OwnerListItem != null)
                    OwnerListItem.SubItems.Sort();
            }
        }

        /// <summary>
        ///     获取或者设置是否闪动
        /// </summary>
        public bool IsTwinkle
        {
            get { return isTwinkle; }
            set
            {
                if (isTwinkle == value) return;
                if (OwnerListItem == null) return;
                isTwinkle = value;
                if (isTwinkle)
                    OwnerListItem.TwinkleSubItemNumber++;
                else
                    OwnerListItem.TwinkleSubItemNumber--;
            }
        }

        internal bool IsTwinkleHide { get; set; }

        /// <summary>
        ///     获取列表子项显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds { get; internal set; }

        /// <summary>
        ///     获取头像显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle HeadRect { get; internal set; }

        /// <summary>
        ///     获取当前列表子项所在的列表项
        /// </summary>
        [Browsable(false)]
        public ChatListItem OwnerListItem { get; internal set; }

        //实现排序接口
        int IComparable.CompareTo(object obj)
        {
            if (!(obj is ChatListSubItem))
                throw new NotImplementedException("obj is not ChatListSubItem");
            var subItem = obj as ChatListSubItem;
            var name = displayName.CompareTo(subItem.displayName);
            var statu = status.CompareTo(subItem.status);
            return statu > 0 ? statu : name;
        }

        private void RedrawSubItem()
        {
            if (OwnerListItem != null)
                if (OwnerListItem.OwnerChatListBox != null)
                    OwnerListItem.OwnerChatListBox.Invalidate(Bounds);
        }

        /// <summary>
        ///     获取当前用户的黑白头像
        /// </summary>
        /// <returns>黑白头像</returns>
        public Bitmap GetDarkImage()
        {
            var b = new Bitmap(headImage);
            var bmp = b.Clone(new Rectangle(0, 0, headImage.Width, headImage.Height), PixelFormat.Format24bppRgb);
            b.Dispose();
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite,
                bmp.PixelFormat);
            var byColorInfo = new byte[bmp.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
            for (int x = 0, xLen = bmp.Width; x < xLen; x++)
            {
                for (int y = 0, yLen = bmp.Height; y < yLen; y++)
                {
                    byColorInfo[y * bmpData.Stride + x * 3] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 1] =
                            byColorInfo[y * bmpData.Stride + x * 3 + 2] =
                                GetAvg(
                                    byColorInfo[y * bmpData.Stride + x * 3],
                                    byColorInfo[y * bmpData.Stride + x * 3 + 1],
                                    byColorInfo[y * bmpData.Stride + x * 3 + 2]);
                }
            }
            Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private byte GetAvg(byte b, byte g, byte r)
        {
            return (byte)((r + g + b) / 3);
        }

        private bool CheckIpAddress(string str)
        {
            var strIp = str.Split('.');
            if (strIp.Length != 4)
                return false;
            for (var i = 0; i < 4; i++)
            {
                try
                {
                    if (Convert.ToInt32(str[i]) > 255)
                        return false;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            return true;
        }
    }
}