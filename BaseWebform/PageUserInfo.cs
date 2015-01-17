/**
 * 自然框架之信息管理类项目的页面基类
 * http://www.natureFW.com/
 *
 * @author
 * 金洋（金色海洋jyk）
 * 
 * @copyright
 * Copyright (C) 2005-2013 金洋.
 *
 * Licensed under a GNU Lesser General Public License.
 * http://creativecommons.org/licenses/LGPL/2.1/
 *
 * 自然框架之信息管理类项目的页面基类 is free software. You are allowed to download, modify and distribute 
 * the source code in accordance with LGPL 2.1 license, however if you want to use 
 * 自然框架之信息管理类项目的页面基类 on your site or include it in your commercial software, you must  be registered.
 * http://www.natureFW.com/registered
 */

/* ***********************************************
 * author :  金洋（金色海洋jyk）
 * email  :  jyk0011@live.cn  
 * function: 后台管理的页面基类。
 *           验证是否登录
 *           获取当前登录人的信息。
 *           获取BaseUserInfo的实例
 *           权限验证的两个函数：验证FunctionID是否正确，验证FunctionID、ButtonID是否匹配
 * history:  created by 金洋 
 * **********************************************
 */


using System;
using System.Web;
using Nature.DebugWatch;
using Nature.User;
using Nature.Client.SSOApp;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 用户相关操作。
    /// 验证是否登录
    /// 获取当前登录人的信息。
    /// 获取BaseUserInfo的实例
    /// 权限验证的两个函数：验证FunctionID是否正确，验证FunctionID、ButtonID是否匹配
    /// </summary>
    public class PageUserInfo : BasePage
    {
        /// <summary>
        /// 保存当前登录人的一些信息
        /// </summary>
        public UserOnlineInfo MyUser; //= UserManage.UserInfo;
        
        #region 初始化 在OnInit执行
        /// <summary>
        /// 设置当前登录人的信息
        /// 设置元数据的数据库的实例
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            var debugInfo = new NatureDebugInfo { Title = "[Nature.Service.Ashx.BaseAshx]判断Url参数" };

            base.OnInit(e);

            //验证是否已经登录
            //如果已经登录了，加载登录人员的信息，
            var manageUser = new ManageUser { Dal = Dal };

            UserWebappInfo userWebappInfo = AppManage.UserWebappInfoByCookies(debugInfo.DetailList);

            if (userWebappInfo.State != UserState.NormalAccess)
            {
                //没有登录。
                Response.Write("您还没有登录，请到<a href='/default.aspx' target='_top'>这里</a>登录");
                Response.End();
            }

            MyUser = manageUser.CreateUser(Convert.ToString(userWebappInfo.UserWebappID), null);

            //保存访问日志
            //SaveViewLog();
           
        }

        #endregion

        #region 保存用户的浏览记录
        /// <summary>
        /// 记录用户的浏览页面的记录
        /// </summary>
        protected virtual void SaveViewLog()
        {
            const string sql = "insert into Person_User_ViewLog (UserID,UserCode,UserName,IP,UserAgent,URL) values (@UserID,@UserCode,@UserName,@IP,@UserAgent,@URL)";
            var mgrParam = Dal.DalCustomer.ManagerParameter;

            mgrParam.ClearParameter();
            mgrParam.AddNewInParameter("UserID", MyUser.BaseUser.UserID);
            mgrParam.AddNewInParameter("UserCode", MyUser.BaseUser.UserCode);
            mgrParam.AddNewInParameter("UserName", MyUser.BaseUser.PersonName);
            mgrParam.AddNewInParameter("IP", HttpContext.Current.Request.UserHostAddress);
            mgrParam.AddNewInParameter("URL", HttpContext.Current.Request.Url.ToString());
            mgrParam.AddNewInParameter("UserAgent", HttpContext.Current.Request.UserAgent);

            Dal.DalCustomer.ExecuteNonQuery(sql);

        }
        #endregion

      
        
    }

}
