using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    /// <summary>
    /// 角色所属阵营。
    /// 分别为 人类Human, 三体ThreeBody, 歌者Singer, 特殊Special。
    /// </summary>
    public enum Camps
    {
        Human, ThreeBody, Singer, Special
    }

    /// <summary>
    /// 角色所属地域。
    /// 分别为 太阳系SolarSystem, 三体系ThreeBodySystem, 外太空OuterSpace。
    /// </summary>
    public enum Locations
    {
        SolarSystem, ThreeBodySystem, OuterSpace
    }
}