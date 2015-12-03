using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    public class ProcessNotice
    {
        /// <summary>e { get; private set; }
        /// <summary>
        /// 现在的状态（转换后）。
        /// </summary>
        public Director.Process NowProcess { get; private set; }
        public ProcessNotice(Director.Process nowProcess)
        {
            NowProcess = nowProcess;
        }
    }
}
