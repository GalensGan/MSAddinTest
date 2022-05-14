using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSUtils
{
    public class TestClass
    {
        public static void NullException()
        {
            throw new NullReferenceException("test exception in reference");
        }
    }
}
