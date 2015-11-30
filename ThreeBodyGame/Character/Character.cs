using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    /// <summary>
    /// 所有身份请继承该类
    /// </summary>
    public sealed class Character
    {
        /*
        来自程序猿往日的吐槽：设计模式面向对象什么的去！死！吧！！！！！！
        这个身份类只起到存数据的作用和角色特定的辅助代码。
        实现全权交给导演类。
        */

        #region 公有特性
        /// <summary>
        /// 角色姓名。
        /// </summary>
        public string CharacterName { get; private set; }

        /// <summary>
        /// 该角色所属阵营。
        /// </summary>
        public Camps Camp { get; private set; }


        #endregion


        #region 公有属性
        /// <summary>
        /// 该角色所在地域。
        /// </summary>
        public Locations Location { get; set; }

        /// <summary>
        /// 角色是否存活。
        /// </summary>
        public bool IsAlive { get; set; }

        //public Dictionary<string, string> CustomState {get;set }
        #endregion

        #region 私有
        /// <summary>
        /// 自定义属性和角色捆绑。某角色一定会有某属性，游戏内可变，但项目数不可在游戏内增加。由工厂类分配项目和初值。
        /// </summary>
        private Dictionary<string, string> CustomAttribute { get; set; }
        #endregion

        #region 公有方法
        /// <summary>
        /// 获取某个角色属性的值。如果初始化没有设定该属性会出现异常。
        /// </summary>
        /// <param name="name">欲获取值的属性名。</param>
        /// <returns>该属性的值。</returns>
        public string GetAttribute(string name) 
        {
            return CustomAttribute[name];
        }
        /// <summary>
        /// 更改某个角色属性的值。如果初始化没有设定该属性，或想赋值为null时，会出现异常。
        /// </summary>
        /// <param name="name">欲获取值的属性名。</param>
        /// <param name="Contant">欲更改的值。</param>
        public void SetAttribute(string name, string Contant)
        {
            if (Contant == null)
                throw new NullReferenceException();
            if (CustomAttribute.ContainsKey(name) == false)
                throw new KeyNotFoundException();
            CustomAttribute[name] = Contant;
        }
        #endregion

        #region 公开枚举类
        #endregion

        /// <summary>
        /// 初始化类。
        /// </summary>
        /// <param name="name">角色姓名。一经输入不变动。</param>
        /// <param name="camp">角色阵营。一经输入不变动。</param>
        /// <param name="location">角色出生的地理位置。</param>
        /// <param name="attribute">角色的自定义属性。</param>
        public Character(string name, Camps camp, Locations location, params string[] attribute) 
        {
            CharacterName = name;
            Camp = camp;
            Location = location;
            IsAlive = false;
            CustomAttribute = new Dictionary<string, string>();
            if(attribute!=null)
            {
                foreach (string att in attribute)
                {
                    CustomAttribute.Add(att, "");
                }
            }
        }



    }
}
