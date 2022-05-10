﻿using Bentley.DgnPlatformNET.Elements;
using Bentley.GeometryNET;
using Bentley.MstnPlatformNET;
using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAddinPlugin
{
    public class TestStaticMethodExecutor : IStaticMethodPlugin
    {
        [Plugin(Name = "static")]
        public static object Execute(PluginArg arg)
        {
            MessageBox.Show("IStaticMethodPlugin 被调用了!");
            return true;
        }


        [Plugin(Name = "newElement")]
        public static object NewElement(PluginArg arg)
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
    }
}
