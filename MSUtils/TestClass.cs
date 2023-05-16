using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSUtils
{
    public class TestClass
    {
        /// <summary>
        /// 测试异常
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public static void NullException()
        {
            throw new NullReferenceException("test exception in reference");
        }
    }
}
