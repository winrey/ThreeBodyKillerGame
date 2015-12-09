using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ThreeBodyGame.Behaviors;

namespace ThreeBodyGame.Behaviors
{
    public class NoticeOnly : Behavior
    {
        /// <summary>
        /// 判断选择是否合法。将用此项作为依据判断该选择是否完成。
        /// </summary>
        public override bool IsChoiceLegal
        {
            get
            {
                return true;
            }
        }
        public NoticeOnly(Director director, string content)
            :base(director)
        {
            Content = content;
        }
    }
}
