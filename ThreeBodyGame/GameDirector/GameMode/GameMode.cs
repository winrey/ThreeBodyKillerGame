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
                return characters.Count;
            }
        }
        /// <summary>
        /// 该局游戏的出场人物。注意，得到的List为拷贝的List。
        /// </summary>
        public List<string> Characters
        {
            get
            {
                return new List<string>(characters.ToArray()); ;
            }
        }
        /// <summary>
        /// 初夜进行的人物判断逻辑。注意，得到的List为拷贝的List。
        /// </summary>
        public List<string> FirstNight
        {
            get
            {
                return new List<string>(firstNight.ToArray()); ;
            }
        }
        /// <summary>
        /// 普通进行的所需的判断逻辑。注意，得到的List为拷贝的List。
        /// </summary>
        public List<string> Night
        {
            get
            {
                return new List<string>(night.ToArray()); ;
            }
        }

        private List<string> characters;
        private List<string> firstNight;
        private List<string> night;
        /// <summary>
        /// 使用人物生成游戏。注意人物不包含刘慈欣！！！
        /// </summary>
        /// <param name="characters">该场游戏会分配的角色。</param>
        /// <param name="firstNight">该场游戏首夜会安排的流程。</param>
        /// <param name="night">该场游戏普通夜晚会安排的流程。</param>
        public GameMode(string[] characters, string[] firstNight, string[] night)
        {
            this.characters = new List<string>(characters);
            this.firstNight = new List<string>(firstNight);
            this.night = new List<string>(night);
        }
        /// <summary>
        /// 得到一份场上角色清单的克隆。
        /// </summary>
        /// <returns></returns>
        public List<string> GetCharacter()
        {
            List<string> l = new List<string>(characters.ToArray());
            return l;
        }
    }
}
