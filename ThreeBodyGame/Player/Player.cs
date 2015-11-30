using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    public class Player
    {

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
        public Player(Character charachter)
        {

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
