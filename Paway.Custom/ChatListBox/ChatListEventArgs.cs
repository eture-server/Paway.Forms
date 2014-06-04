using System;
using System.Collections.Generic;
using System.Text;

namespace Paway.Custom
{
    //自定义事件参数类
    public class ChatListEventArgs
    {
        private ChatListSubItem mouseOnSubItem;
        public ChatListSubItem MouseOnSubItem {
            get { return mouseOnSubItem; }
        }

        private ChatListSubItem selectSubItem;
        public ChatListSubItem SelectSubItem {
            get { return selectSubItem; }
        }

        public ChatListEventArgs(ChatListSubItem mouseonsubitem, ChatListSubItem selectsubitem) {
            this.mouseOnSubItem = mouseonsubitem;
            this.selectSubItem = selectsubitem;
        }
    }
}
