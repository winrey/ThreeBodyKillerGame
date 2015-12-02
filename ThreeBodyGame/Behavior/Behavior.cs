using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeBodyGame
{
    /// <summary>
    /// 一切问询/通知行为继承该类。
    /// </summary>
    public class Behavior
    {
        /// <summary>
        /// 判断是否已经执行完成
        /// </summary>
        public bool HasFinished { get; private set; }
        /// <summary>
        /// 被问询者。也仅有该人和刘慈欣有权限做出回答。刘慈欣回答一般是掉线等情况。
        /// </summary>
        public string PlayerToChat { get; private set; }
        /// <summary>
        /// 记录向用户首次发送的内容。为空则为不发送。
        /// </summary>
        public string Contant { get; private set; }
        /// <summary>
        /// 记录/查询做出的选择。会检查选择是否有效。
        /// </summary>
        public string Choice
        {
            get
            {
                return choice;
            }
            set
            {
                if (Check(value) == false) throw new NoPermissionException();
                choice = value;
            }
        }
        private string choice;
        private Director director;
        public Behavior(Director gDirector)
        {
            director = gDirector;
        }
        /// <summary>
        /// 检查其中用使用存在某命令。
        /// </summary>
        /// <returns></returns>
        public bool Check(string cmd)
        {
            //检测调用的是否为系统自带方法
            foreach (MethodInfo mi in typeof(Behavior).GetMethods())
            {
                if (mi.Name == cmd)
                {
                    return false;
                }
            }
            try
            {
                MethodInfo mi = this.GetType().GetMethod(cmd);
                //检查是否为公有方法
                if (mi.IsPublic == false)
                {
                    throw new NoPermissionException();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 处理信息的方法。会自动调用旗下相应方法。
        /// </summary>
        /// <param name="cmd"></param>
        public void Commend(string cmd)
        {
            Choice = cmd;
        }
        /// <summary>
        /// 执行做出的选择。
        /// </summary>
        public void Action()
        {
            try
            {
                MethodInfo mi = this.GetType().GetMethod(Choice);
                //调用方法
                mi.Invoke(this, null);
                //调用准销毁函数
                Die();
            }
            catch
            {
                throw;
            }
        }

        public class NoPermissionException : Exception
        {
            public NoPermissionException()
                : base("对不起，您没有此操作的权限。")
            {

            }
        }

        /// <summary>
        /// 该方法在询问前做的事。默认为仅发送问询语。
        /// </summary>
        public virtual void PreDo()
        {
            if (Contant == "") return;
            director.SendToAll(Contant, "纯消息", PlayerToChat);
        }
        /// <summary>
        /// 行为执行完成后调用该方法
        /// </summary>
        protected void Die()
        {
            director.waitingList.Remove(this);
            director.Next();
        }
    }
}
