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
 * function: 删除数据的基类
 * history:  created by 金洋 2009-8-18 14:37:55 
 * **********************************************
 */


using System;
using System.Globalization;
using Nature.Common;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 删除数据的页面基类
    /// </summary>
    public class BasePageDelete : PageURL
    {
        #region 初始化 在Page_Load之间执行
        /// <summary>
        /// 通过FunctionID和buttonID来验证。
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //验证当前用户是否可以使用这个页面。
            //通过ModuleID和buttonID来验证。
            MyUser.UserPermission.CheckButtonID(ModuleID, ButtonID.ToString(CultureInfo.InvariantCulture), Dal.DalRole );

            
        }
        #endregion


        #region 验证DataID
        /// <summary>
        /// 验证DataID。
        /// </summary>
        protected override void SetDataID()
        {
            base.SetDataID();
            //必传参数，需要验证.
            //验证是不是数字
            if (!Functions.IsInt(DataID))
            {
                //不是数字，验证是否是GUID
                if (!Functions.IsGuid(DataID))
                {
                    Response.Write("DataID参数不正确！");
                    Response.End();
                }
            }
        }
        #endregion

        #region 验证是否可以删除记录
        /// <summary>
        /// 验证是否可以删除记录
        /// </summary>
        public virtual void CheckCanDelete()
        {
            //调用MyUser里的CheckCanUpdate 来判断是否可以删除指定的记录
            string re = MyUser.UserPermission.CheckCanUpdate(ModuleID,MasterPageViewID, DataID, Dal.DalMetadata,null);
            if (re.Length > 0)
            {
                //不能访问
                Response.Write(re);
                Response.End();
            }
        }
        #endregion
    }
}
