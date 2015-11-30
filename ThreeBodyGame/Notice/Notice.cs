using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    public class Notice
    {
        #region 只读公有
        /// <summary>
        /// 通知对象。
        /// 全体消息为全体，阵营消息为某阵营，系统消息为刘慈欣等等。
        /// </summary>
        public string Receiver { get; private set; }
        /// <summary>
        /// 被通知者类型。
        /// </summary>
        public ReceiverTypes ReceiverType { get; private set; }
        /// <summary>
        /// 通知的简略内容。
        /// </summary>
        public string Contant { get; private set; }
        /// <summary>
        /// 该通知的类型。
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// 通知的相关信息。
        /// </summary>
        public object Detail { get; private set; }
        #endregion
        #region 公有枚举类
        /// <summary>
        /// 通知类型。
        /// 分为：
        /// 全体消息All,
        /// 系统消息System(发给"刘慈欣"), 
        /// 身份消息Charater(发给持有某角色的玩家,如:"希恩斯"), 
        /// 玩家消息Player(直接按玩家编号发给玩家,如:"5"), 
        /// 阵营消息Camp(发给某一阵营,如:"ThreeBodySystem"), 
        /// 群组消息Group(发给某一群组,如:"同归于尽组"),
        /// 地域消息Location(发给处在某地点的玩家,如:"ThreeBody")。
        /// </summary>
        public enum ReceiverTypes
        {
            All, System, Charater, Player, Camp, Group, Location
        }
        #endregion
        #region 私有
        private Notice()
        {

        }
        #endregion
        #region 公有
        #region 通知生成工厂
        /// <summary>
        /// 生成全体消息。
        /// </summary>
        /// <param name="contant">消息内容。</param>
        /// <returns>生成的对象。</returns>
        public static Notice AllNoticeFactory(string contant, string type, Object detail)
        {
            var n = new Notice();
            n.ReceiverType = ReceiverTypes.All;
            n.Receiver = "全体";
            n.Contant = contant;
            return n;
        }
        /// <summary>
        /// 生成系统消息。
        /// </summary>
        /// <param name="contant">面向用户消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="detail">消息细节内容。</param>
        /// <returns>生成的对象。</returns>
        public static Notice SystemNoticeFactory(string contant, string type, Object detail)
        {
            var n = new Notice();
            n.ReceiverType = ReceiverTypes.System;
            n.Receiver = "刘慈欣";
            n.Contant = contant;
            n.Type = type;
            n.Detail = detail;
            return n;
        }
        /// <summary>
        /// 生成发送给某一角色的消息。
        /// </summary>
        /// <param name="receiver">接受消息的角色，如:"希恩斯"。</param>
        /// <param name="contant">面向用户消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="detail">消息细节内容。</param>
        /// <returns>生成的对象。</returns>
        public static Notice CharacterNoticeFactory(string receiver, string contant, string type, Object detail)
        {
            var n = new Notice();
            n.ReceiverType = ReceiverTypes.Charater;
            n.Receiver = receiver;
            n.Contant = contant;
            n.Type = type;
            n.Detail = detail;
            return n;
        }
        /// <summary>
        /// 生成发送给某一玩家的消息。
        /// </summary>
        /// <param name="receiver">接受消息的玩家号码。</param>
        /// <param name="contant">面向用户消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="detail">消息细节内容。</param>
        /// <returns>生成的对象。</returns>
        public static Notice CharacterNoticeFactory(int receiver, string contant, string type, Object detail)
        {
            var n = new Notice();
            n.ReceiverType = ReceiverTypes.Player;
            n.Receiver = receiver.ToString();
            n.Contant = contant;
            n.Type = type;
            n.Detail = detail;
            return n;
        }
        /// <summary>
        /// 生成发送给某一阵营的消息。
        /// </summary>
        /// <param name="receiver">接受消息的阵营，如:"ThreeBody"</param>
        /// <param name="contant">面向用户消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="detail">消息细节内容。</param>
        /// <returns>生成的对象。</returns>
        public static Notice CampNoticeFactory(string receiver, string contant, string type, Object detail)
        {
            var n = new Notice();
            n.ReceiverType = ReceiverTypes.Camp;
            n.Receiver = receiver;
            n.Contant = contant;
            n.Type = type;
            n.Detail = detail;
            return n;
        }
        /// <summary>
        /// 生成发送给某一讨论组的消息。
        /// </summary>
        /// <param name="receiver">接受消息的讨论组，如:"踩死虫子组"</param>
        /// <param name="contant">面向用户消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="detail">消息细节内容。</param>
        /// <returns>生成的对象。</returns>
        public static Notice GroupNoticeFactory(string receiver, string contant, string type, Object detail)
        {
            var n = new Notice();
            n.ReceiverType = ReceiverTypes.Group;
            n.Receiver = receiver;
            n.Contant = contant;
            n.Type = type;
            n.Detail = detail;
            return n;
        }
        /// <summary>
        /// 生成发送给某一地域的消息。
        /// </summary>
        /// <param name="receiver">接受消息的地域，如:"ThreeBody"</param>
        /// <param name="contant">面向用户消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="detail">消息细节内容。</param>
        /// <returns>生成的对象。</returns>
        public static Notice LocationNoticeFactory(string receiver, string contant, string type, Object detail)
        {
            var n = new Notice();
            n.ReceiverType = ReceiverTypes.Location;
            n.Receiver = receiver;
            n.Contant = contant;
            n.Type = type;
            n.Detail = detail;
            return n;
        }
        #endregion
        public Director.NoticedEventArgs ToArgs()
        {
            return new Director.NoticedEventArgs(this);
        }
        #endregion
        #region 异常
        public class BadNoticeException : Exception
        {
            public BadNoticeException()
                : base("该通知发生数据错误！")
            {

            }
        }
        #endregion
    }
}
