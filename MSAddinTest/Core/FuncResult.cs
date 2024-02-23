using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core
{
    public class FuncResult
    {
        /// <summary>
        /// 默认为 false
        /// </summary>
        public FuncResult()
        {

        }

        public FuncResult(bool ok):this()
        {
            Ok= ok;
        }

        public FuncResult(bool ok,string message):this(ok)
        {
            Message = message;
        }

        /// <summary>
        /// 返回正确
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// 返回错误
        /// </summary>
        public bool NotOk => !Ok;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCode StatusCode { get; set; }= StatusCode.Success;

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        // 与 bool 类型的隐式转换
        public static implicit operator bool(FuncResult result)
        {
            return result.Ok;
        }
    }
}
