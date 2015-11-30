using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    /// <summary>
    /// 获取Character的工厂。
    /// </summary>
    public static class CharacterFactory
    {

        public static Character GetCharacter(string name)
        {
            return (Character) typeof(CharacterFactory).GetMethod(name).Invoke(null, null);
        }


        #region 构建角色
        #region 刘慈欣
        public static Character 刘慈欣()
        {
            return new Character("刘慈欣", Camps.Human, Locations.SolarSystem, null);
        }
        #endregion
        #endregion
    }
}
