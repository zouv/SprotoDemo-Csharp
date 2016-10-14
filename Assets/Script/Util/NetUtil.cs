/***
* Author: zouv
* Date: 2016-08-01
* Doc: 网络数据处理工具类
***/

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class NetUtil
{
    public static int ReadBytes(byte[] bytes, int offset, int count)
    {
        int v = 0;
        for (int i = offset; i < offset + count; i++)
        {
            v |= bytes[i] << 8 * (count - i - 1);
        }
        return v;
    }
}
