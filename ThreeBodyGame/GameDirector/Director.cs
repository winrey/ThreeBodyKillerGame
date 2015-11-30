using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    /// <summary>
    /// 游戏的总管理类。
    /// </summary>
    public class Director
    {
        #region 公有
        /// <summary>
        /// 现在游戏所处阶段。
        /// 该属性变得会执行相应方法。
        /// </summary>
        public Process NowProcess
        {
            get
            {
                return nowProcess;
            }
            private set
            {
                nowProcess = value;
                switch (NowProcess)
                {
                    case Process.Preparing: Preparing(); break;
                    case Process.FirstDay: FirstDay(); break;
                    case Process.FirstNight: FirstNight(); break;
                    case Process.Day: Day(); break;
                    case Process.Night: Night(); break;
                    case Process.Ending: Ending();break;
                }
            }
        }

        public delegate void NoticedEventHandler(Object sender, NoticedEventArgs e);
        /// <summary>
        /// 监视游戏进程请在此注册。
        /// </summary>
        public event NoticedEventHandler Noticed;
        /// <summary>
        /// 游戏的各项设置
        /// </summary>
        public GameMode GameMode { get; private set; }

        public Random Random { get; private set; }
        #endregion

        #region 内部
        internal Behavior waitingNow
        {
            get
            {
                if (waitingList.Count != 0)
                    return waitingList[0];
                else
                    return null;
            }
        }
        /// <summary>
        /// 等待执行的行为
        /// </summary>
        internal List<Behavior> waitingList = new List<Behavior>();

        /// <summary>
        /// 所有玩家的总集，包括刘慈欣。刘慈欣角色为0号，其它游戏角色在其之后
        /// </summary>
        internal List<Player> players;
        #endregion

        #region 私有

        private Process nowProcess;

       

        #region 进程控制队列
        private void Preparing()
        {
            //添加刘慈欣到0号位占位
            players.Add(new Player(CharacterFactory.刘慈欣()));
            #region 将string转换为对应角色
            List<string> ls = GameMode.GetCharacter();
            List<Character> lc= new List<Character>();
            ls.ForEach(i => lc.Add(CharacterFactory.GetCharacter(i)));
            #endregion
            #region 将角色乱序，并分配给玩家
            for(int i= Random.Next(lc.Count) ; lc.Count != 0; i = Random.Next(lc.Count))
            {
                players.Add(new Player(lc[i]));
                lc.RemoveAt(i);
            }
            #endregion
            #region 通知玩家身份以及顺序
            //生成通知内容
            Noticed(this, new NoticedEventArgs(Notice.AllNoticeFactory("")));
            #endregion
        }
        private void FirstDay()
        {

        }
        private void FirstNight()
        {

        }
        private void Day()
        {

        }
        private void Night()
        {

        }
        private void Ending()
        {

        }

        #endregion

        #endregion

        #region 公有枚举类
        /// <summary>
        /// 游戏的进程。
        /// 分别为 准备Preparing, 首日FirstDay, 首夜FirstNight, 日Day,夜 Night, 结束Ending
        /// </summary>
        public enum Process
        {
            Preparing, FirstDay, FirstNight, Day, Night, Ending
        }
        #endregion

        #region 公有类
       public class NoticedEventArgs : EventArgs
        {
            public Notice NoticeInfo { get; private set; }
            public NoticedEventArgs(Notice notice)
            {
                NoticeInfo = notice;
            }
        }
        #endregion

        #region 公开方法
        public Director()
        {
            NowProcess = Process.Preparing;
            players = new List<Player>();
        }

        /// <summary>
        /// 输入键入的指令。
        /// </summary>
        /// <param name="msg">指令内容。</param>
        public void Message(string msg)
        {
            if (msg == "下一阶段")
            {
                NextProcess();
            }
            if (msg == "Next")
            {
                Next();
            }
            if (waitingNow != null){
                waitingNow.Message(msg);
            }
        }

        #region Send
        public void SendToAll(string contant, string type, Object detail)
        {
            this.Noticed.Invoke(this, new Director.NoticedEventArgs(Notice.AllNoticeFactory(contant, type, detail)))
        }
        #endregion

        /// <summary>
        /// 程序每个行为结束后都会调用该方法推进
        /// </summary>
        public void Next()
        {
            //如果队列为空，询问下一阶段
            if (waitingList.Count == 0)
            {

            }
        }
        public Process NextProcess()
        {
            //检测是否有未完成内容
            if (waitingNow != null)
            {
                throw new CannotNextException();
            }
            switch (NowProcess)
            {
                case Process.Preparing: NowProcess = Process.FirstDay; break;
                case Process.FirstDay: NowProcess = Process.FirstNight; break;
                case Process.FirstNight: NowProcess = Process.Day; break;
                case Process.Day: NowProcess = Process.Night; break;
                case Process.Night: NowProcess = Process.Day; break;
                case Process.Ending: throw new CannotNextException();
            }
            return NowProcess;
        }


        #endregion

        #region 内部方法
        /// <summary>
        /// 加塞，使程序先执行该行为。
        /// </summary>
        /// <param name="behavior"></param>
        internal void CutInLine(Behavior behavior)
        {
            //如果改请求已在队列，删除
            if(waitingList.Contains(behavior))
                waitingList.Remove(behavior);
       
            waitingList.Insert(1, behavior);

        }

        internal void
        #endregion

        #region 异常
        public class CannotNextException : Exception
        {
            public CannotNextException()
                :base("无法执行下一阶段。可能是因为存在操作未完成或该局已为Ending状态。")
            {

            }
        }
        #endregion

    }
}
