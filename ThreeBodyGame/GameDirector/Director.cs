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

        /// <summary>
        /// 游戏自开始后所有对外发送的消息。注意，得到的是原List的拷贝。
        /// </summary>
        public List<Notice> History {
            get
            {
                return new List<Notice>(history.ToArray());
            }
            private set
            {
                history = value;
            }
        }

        /// <summary>
        /// 现在已经进行的回合数。
        /// </summary>
        public int Round { get; private set; }
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
        /// 一般于夜晚填写，白天统一发送的调用堆栈。
        /// 用于发送夜晚各项活动的判定的结果。
        /// </summary>
        internal List<Notice> sendList = new List<Notice>();
        
            /// <summary>
        /// 所有玩家的总集，包括刘慈欣。刘慈欣角色为0号，其它游戏角色在其之后
        /// </summary>
        internal List<Player> players;


        #endregion

        #region 私有

        private Process nowProcess;

        private List<Notice> history;

       

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
            history = new List<Notice>();
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
        /// 制作消息内容的工厂。
        /// </summary>
        /// <param name="contant">通知内容</param>
        /// <param name="type">通知种类</param>
        /// <param name="detail">通知对象</param>
        /// <param name="receiver">接受者，如空则为全体消息。</param>
        /// <param name="receiver">接受者的类型，默认为角色。</param>
        public Notice NoticeMaker(string contant, string type, Object detail, string receiver = null,
                         Notice.ReceiverTypes receiverType = Notice.ReceiverTypes.Charater)
        {
            Notice n;
            if (receiverType == Notice.ReceiverTypes.System)
                n = Notice.SystemNoticeFactory(contant, type, detail);

            else if (receiver == null || receiverType == Notice.ReceiverTypes.All)
                n = Notice.AllNoticeFactory(contant, type, detail);

            else if (receiverType == Notice.ReceiverTypes.Charater)
                n = Notice.CharacterNoticeFactory(receiver, contant, type, detail);

            else if (receiverType == Notice.ReceiverTypes.Camp)
                n = Notice.CampNoticeFactory(receiver, contant, type, detail);

            else if (receiverType == Notice.ReceiverTypes.Group)
                n = Notice.GroupNoticeFactory(receiver, contant, type, detail);

            else if (receiverType == Notice.ReceiverTypes.Location)
                n = Notice.LocationNoticeFactory(receiver, contant, type, detail);

            else if (receiverType == Notice.ReceiverTypes.Player)
                n = Notice.PlayerNoticeFactory(int.Parse(receiver), contant, type, detail);
            else throw new NotImplementedException();
            return n;
        }

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
            Notice n = NoticeMaker(contant, type, detail, receiver, receiverType);
            SendTo(n);

        }
        /// <summary>
        /// 以通知方式，向某人/全员发送信息（通知）。
        /// </summary>
        /// <param name="notice"></param>
        public void SendTo(Notice notice)
        {
            //将该条通知加入历史记录
            history.Add(notice);
            this.Noticed.Invoke(this, new Director.NoticedEventArgs(notice));
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

        /// <summary>
        /// 把该信息加入发送队列，等待统一发送。
        /// </summary>
        /// <param name="contant">通知内容</param>
        /// <param name="type">通知种类</param>
        /// <param name="detail">通知对象</param>
        /// <param name="receiver">接受者，如空则为全体消息。</param>
        /// <param name="receiver">接受者的类型，默认为角色。</param>
        internal void SendLater(string contant, string type, Object detail, string receiver = null,
                         Notice.ReceiverTypes receiverType = Notice.ReceiverTypes.Charater)
        {
            Notice n = NoticeMaker(contant, type, detail, receiver, receiverType);
            sendList.Add(n);
        }
        /// <summary>
        /// 将待发送队列中的内容全部发送，并将其清空。
        /// </summary>
        internal void PushSendList()
        {
            foreach(var n in sendList)
            {
                SendTo(n);
            }
            sendList = new List<Notice>();
        }
        #endregion

        #region 私有方法
        #region 进程控制队列
        private void Preparing()
        {
            SendTo("游戏开始准备。", "阶段变更通知", new ProcessNotice(Process.Preparing));
        }

        private void EndPreparing()
        {

        }

        private void FirstDay()
        {
            //设为第一轮
            Round = 1;
            //通知阶段开始
            SendTo("游戏进入首日，开始进行身份分配。", "阶段变更通知", new ProcessNotice(Process.FirstDay));
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
            //通知阶段开始
            SendTo("首夜将至，浓郁夜色中暗流涌动。", "阶段变更通知", new ProcessNotice(Process.FirstNight));
            //将规定的询问单注入游戏
            //将询问单转化为具体类
            throw new NotImplementedException();
        }

        private void EndFirstNight()
        {

        }
        private void Day()
        {
            //变更回合数
            Round++;
            //通知阶段开始
            SendTo("朝晖再起，第" + Round + "个清晨来临了。", "阶段变更通知", new ProcessNotice(Process.Day));
            //通知前夜积压的消息
            PushSendList();
        }
        private void EndDay()
        {
        }
        private void Night()
        {
            //通知阶段开始
            SendTo("夜色漫漫，时间仿佛已经停滞，你在忐忑不安中入眠。", "阶段变更通知", new ProcessNotice(Process.Night));

        }
        private void EndNight()
        {

        }
        private void Ending()
        {
            //通知游戏结束
            SendTo("本局游戏结束！", "阶段变更通知", new ProcessNotice(Process.Ending));
        }

        #endregion
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
