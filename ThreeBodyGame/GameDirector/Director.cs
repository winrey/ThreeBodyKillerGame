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
        /// <summary>
        /// 现在游戏所处阶段。
        /// 该属性变得会执行相应方法。
        /// </summary>
        internal Process NowProcess
        {
            get
            {
                return nowProcess;
            }
            private set
            {
                switch (NowProcess)
                {
                    case Process.Preparing: EndPreparing(); break;
                    case Process.FirstDay: EndFirstDay(); break;
                    case Process.FirstNight: EndFirstNight(); break;
                    case Process.Day: EndDay(); break;
                    case Process.Night: EndNight(); break;
                    case Process.Ending: throw new CannotNextException();
                }
                nowProcess = value;
                switch (NowProcess)
                {
                    case Process.Preparing: Preparing(); break;
                    case Process.FirstDay: FirstDay(); break;
                    case Process.FirstNight: FirstNight(); break;
                    case Process.Day: Day(); break;
                    case Process.Night: Night(); break;
                    case Process.Ending: Ending(); break;
                }
            }
        }
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
            
        }

        private void EndPreparing()
        {

        }

        private void FirstDay()
        {
            //添加刘慈欣到0号位占位
            players.Add(new Player(CharacterFactory.刘慈欣(), this));
            #region 将string转换为对应角色
            List<string> ls = GameMode.GetCharacter();
            List<Character> lc = new List<Character>();
            ls.ForEach(i => lc.Add(CharacterFactory.GetCharacter(i)));
            #endregion
            #region 将角色乱序，并分配给玩家
            for (int i = Random.Next(lc.Count); lc.Count != 0; i = Random.Next(lc.Count))
            {
                players.Add(new Player(lc[i], this));
                lc.RemoveAt(i);
            }
            #endregion
            #region 通知玩家身份以及顺序
            //生成通知内容
            throw new NotImplementedException();
            #endregion
        }
        private void EndFirstDay()
        {

        }
        private void FirstNight()
        {

        }

        private void EndFirstNight()
        {

        }
        private void Day()
        {

        }
        private void EndDay()
        {

        }
        private void Night()
        {

        }
        private void EndNight()
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
        /// <return>返回是否为可识别命令</return>
        public bool Commend(string msg)
        {
            if (msg == "下一阶段")
            {
                NextProcess();
                return true;
            }
            //if (msg == "Next")
            //{
            //    Next();
            //    return true;
            //}
            if (waitingNow != null){
                waitingNow.Commend(msg);
                return true;
            }
            if (msg.StartsWith("使用身份"))
            {
                //识别身份，命令用" "隔离
                var msgs = msg.Split(' ');
                var User = from a in players
                           where a.PlayerCharater.CharacterName == msgs[1]
                           select a;
                foreach(var U in User)
                {
                    //删除第一个空格(含)前的所有内容
                    //这里指“使用身份”命令
                    msg = msg.Remove(0, msg.IndexOf(' '));
                    //这里指角色名
                    msg = msg.Remove(0, msg.IndexOf(' '));
                    U.Commend(msg);
                }
                return true;
            }
            return false;
        }

        #region Send
        /// <summary>
        /// 向某人/全员发送信息（通知）。
        /// </summary>
        /// <param name="contant">通知内容</param>
        /// <param name="type">通知种类</param>
        /// <param name="detail">通知对象</param>
        /// <param name="receiver">接受者，如空则为全体消息。</param>
        /// <param name="receiver">接受者的类型，默认为角色。</param>
        public void SendTo(string contant, string type, Object detail, string receiver = null, 
                         Notice.ReceiverTypes receiverType = Notice.ReceiverTypes.Charater)
        {
            if(receiverType == Notice.ReceiverTypes.System)
                this.Noticed.Invoke(this, new Director.NoticedEventArgs
                    (Notice.SystemNoticeFactory(contant, type, detail)));

            else if (receiver == null || receiverType == Notice.ReceiverTypes.All)
                this.Noticed.Invoke(this, new Director.NoticedEventArgs
                    (Notice.AllNoticeFactory(contant, type, detail)));

            else if(receiverType == Notice.ReceiverTypes.Charater)
                this.Noticed.Invoke(this, new Director.NoticedEventArgs
                    (Notice.CharacterNoticeFactory(receiver, contant, type, detail)));

            else if (receiverType == Notice.ReceiverTypes.Camp)
                this.Noticed.Invoke(this, new Director.NoticedEventArgs
                    (Notice.CampNoticeFactory(receiver, contant, type, detail)));

            else if (receiverType == Notice.ReceiverTypes.Group)
                this.Noticed.Invoke(this, new Director.NoticedEventArgs
                    (Notice.GroupNoticeFactory(receiver, contant, type, detail)));

            else if (receiverType == Notice.ReceiverTypes.Location)
                this.Noticed.Invoke(this, new Director.NoticedEventArgs
                    (Notice.LocationNoticeFactory(receiver, contant, type, detail)));

            else if (receiverType == Notice.ReceiverTypes.Player)
                this.Noticed.Invoke(this, new Director.NoticedEventArgs
                    (Notice.PlayerNoticeFactory(int.Parse(receiver), contant, type, detail)));

        }
        #endregion

        ///// <summary>
        ///// 强制结束某该阶段。
        ///// </summary>
        //public void Next()
        //{
        //    throw new NotImplementedException();
        //}
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

        internal void ClearAllBehavior()
        {
            waitingList = new List<Behavior>();
        }
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
