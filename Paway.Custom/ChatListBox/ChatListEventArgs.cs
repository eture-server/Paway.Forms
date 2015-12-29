namespace Paway.Custom
{
    //自定义事件参数类
    public class ChatListEventArgs
    {
        public ChatListEventArgs(ChatListSubItem mouseonsubitem, ChatListSubItem selectsubitem)
        {
            MouseOnSubItem = mouseonsubitem;
            SelectSubItem = selectsubitem;
        }

        public ChatListSubItem MouseOnSubItem { get; private set; }

        public ChatListSubItem SelectSubItem { get; private set; }
    }
}