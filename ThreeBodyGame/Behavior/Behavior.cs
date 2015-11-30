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
        public string Contant { get; private set; }
        private Director director;
        public Behavior(Director gDirector)
        {
            director = gDirector;
        }
        /// <summary>
        /// 处理信息的方法。会自动调用旗下相应方法。
        /// </summary>
        /// <param name="msg"></param>
        public void Message(string msg)
        {
            //检测调用的是否为系统自带方法
            foreach(MethodInfo mi in typeof(Behavior).GetMethods())
            {
                if (mi.Name == msg)
                {
                    throw new NoPermissionException();
                }
            }
            try
            {
                MethodInfo mi = this.GetType().GetMethod(msg);
                //检查是否为公有方法
                if (mi.IsPublic == false)
                {
                    throw new NoPermissionException();
                }
                mi.Invoke(this, null);
                HasFinished = true;
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
