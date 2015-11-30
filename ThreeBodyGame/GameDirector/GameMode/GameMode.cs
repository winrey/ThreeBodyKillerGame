using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    public class GameMode
    {
        /// <summary>
        /// 该局游戏的人数。
        /// </summary>
        public int PlayersNum
        {
            get
            {
                return Characters.Count;
            }
        }
        private List<string> Characters;
        /// <summary>
        /// 使用人物生成游戏。注意人物不包含刘慈欣！！！
        /// </summary>
        /// <param name="characters"></param>
        public GameMode(string[] characters)
        {
            Characters = new List<string>(characters);
        }
        /// <summary>
        /// 得到一份场上角色清单的克隆。
        /// </summary>
        /// <returns></returns>
        public List<string> GetCharacter()
        {
            List<string> l = new List<string>(Characters.ToArray());
            return l;
        }
    }
}
