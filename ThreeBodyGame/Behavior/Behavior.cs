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
        /// 判断是否已经执行过。
        /// </summary>
        public bool HasActioned { get; private set; }
        /// <summary>
        /// 判断是否已经被选择过。
        /// </summary>
        public bool HasChosen { get; private set; }
        /// <summary>
        /// 判断选择是否合法。
        /// </summary>
        public bool IsChoiceLegal
        {
            get
            {
                if (HasChosen) return true;
                if (Check(DefaultChoice)) return true;
                return false;
            }
        }
        /// <summary>
        /// 被问询者。也仅有该人和刘慈欣有权限做出回答。刘慈欣回答一般是掉线等情况。
        /// </summary>
        public string Receiver { get; private set; }
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
                if (HasChosen)
                    return choice;
                else if (Check(DefaultChoice))
                    return DefaultChoice;
                else throw new HaveNeverChosenException();
            }
            set
            {
                if (Check(value) == false) throw new NoPermissionException();
                HasChosen = true;
                choice = value;
            }
        }
        private string choice;
        /// <summary>
        /// 如果没有选择就执行时默认的选择。如果没有选择且该项也非法会报错。
        /// </summary>
        public readonly string DefaultChoice;
        private Director director;
        public Behavior(Director gDirector)
        {
            director = gDirector;
            HasActioned = false;
            HasChosen = false;
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
            if(IsChoiceLegal == false)
            {
                throw new HaveNeverChosenException();
            }
            if (HasActioned == true) throw new AlreadyFinishingException();
            try
            {
                MethodInfo mi;
                if (HasChosen)
                    mi = this.GetType().GetMethod(Choice);
                else
                    mi = this.GetType().GetMethod(DefaultChoice);
                //调用方法
                mi.Invoke(this, null);
                HasActioned = true;
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
        public class AlreadyFinishingException : Exception
        {
            public AlreadyFinishingException()
                : base("该行为已经曾经被执行！")
            {

            }
        }
        public class HaveNeverChosenException : Exception
        {
            public HaveNeverChosenException()
                : base("该行为尚未进行选择默认赋值不合法！")
            {

            }
        }

        /// <summary>
        /// 该方法在询问前做的事。默认为仅发送问询语。
        /// </summary>
        protected virtual void PreDo()
        {
            if (Contant == "") return;
            director.SendTo(Contant, "纯消息", null, Receiver);
        }
        /// <summary>
        /// 该方法在Action后无论选择如何一定会完成的方法，手动销毁可选择是否完成该方法。如不覆写则为空。
        /// </summary>
        protected virtual void AfterDone()
        {

        }
        /// <summary>
        /// 行为执行完成后调用该方法。
        /// </summary>
        /// <param name="doAfterDone">选择是否销毁是否调用事后方法AfterDone</param>
        protected void Die(bool doAfterDone = true)
        {
            if (doAfterDone) AfterDone();
        }
        
    }
}
