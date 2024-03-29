﻿using Bentley.DgnPlatformNET.Elements;
using Bentley.GeometryNET;
using Bentley.MstnPlatformNET;
using MSAddinTest.MSTestInterface;
using MSUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAddinPlugin
{
    /// <summary>
    /// 测试静态方法
    /// 1. 类继承接口 IMSTest_StaticMethod
    /// 2. 静态方法添加特性 MSTest
    /// 3. 静态方法有且仅有一个IMSTestArg参数
    /// </summary>
    public class TestStaticMethodExecutor : IMSTest_StaticMethod
    {
        [MSTest("static",Description ="通过 mstest test static 来调用")]
        public static object Execute(string arg)
        {
            MessageBox.Show("IStaticMethodPlugin 被调用了4!");
            return true;
        }


        [MSTest("element")]
        public static object NewElement(string arg)
        {
            // 绘制一个元素
            CurvePrimitive line = CurvePrimitive.CreateLineString(new List<DPoint3d>()
            {
                new DPoint3d(0,0,0),
                new DPoint3d(100000,0,0),
            });
            var instance = Session.Instance;
            if (instance == null) return false;

            var dgnm = instance.GetActiveDgnModel();
            Element et = DraftingElementSchema.ToElement(dgnm, line, null);
            if (et != null) et.AddToModel();

            MessageBox.Show("绘制元素成功!");
            return true;
        }

        [MSTest("exception")]
        public static object TestReference(string arg)
        {
            TestClass.NullException();
            return false;
        }
    }
}
