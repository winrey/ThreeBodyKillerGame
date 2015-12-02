using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    public class Player
    {
        private Director drirector;

        /// <summary>
        /// 该玩家拿到的角色
        /// </summary>
        public Character PlayerCharater { get; private set; }
        /// <summary>
        /// 该角色的遗言。注意！仅可设置一次。
        /// </summary>
        public string LastWords
        {
            get
            {
                return lastWords;
            }
            set
            {
                if (lastWords != null)
                    throw new AlreadyHaveLastWords();
                lastWords = value;
            }
        }
        private string lastWords;
        /// <summary>
        /// 初始化玩家类
        /// </summary>
        /// <param name="charachter">该玩家的角色。</param>
        /// <param name="director">该场游戏的导演类。</param>
        public Player(Character charachter, Director director)
        {
            this.drirector = director;
            this.PlayerCharater = charachter;
        }
        /// <summary>
        /// 某角色发出的命令。
        /// </summary>
        /// <param name="cmd"></param>
        public void Commend(string cmd)
        {
            var com = from b in this.drirector.waitingList
                      where b.PlayerToChat == this.PlayerCharater.CharacterName
                      select b;
            foreach(var b in com)
            {
                b.Commend(cmd);
            }
        }

        public class AlreadyHaveLastWords : Exception
        {
            public AlreadyHaveLastWords()
                :base("该角色已有遗言！")
            {

            }
        }

    }
}
