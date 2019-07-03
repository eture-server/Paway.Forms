using System;
using System.Collections.Generic;

namespace Paway.Helper
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    [Serializable]
    public class PawayException : Exception
    {
        /// <summary>
        /// 初始化新实例。
        /// </summary>
        public PawayException() : base() { }
        /// <summary>
        /// 使用指定的错误消息初始化新实例。
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public PawayException(string message) : base(message) { }
        /// <summary>
        /// 使用指定错误消息和对作为此异常原因的内部异常的引用来初始化新实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误消息</param>
        /// <param name="innerException">导致当前异常的异常</param>
        public PawayException(string message, Exception innerException) : base(message, innerException) { }
    }
}