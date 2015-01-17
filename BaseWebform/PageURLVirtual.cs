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
 * function: URL里面的Virtual函数，获取URL传递过来的信息，进行验证。
 * history:  created by 金洋  2009-9-10 14:19:47 
 * **********************************************
 */


using Nature.Common;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 获取URL传递过来的信息，进行验证。
    /// </summary>
    public partial class PageURL 
    {
        #region 设置标题
        /// <summary>
        /// 设置标题
        /// </summary>
        protected void SetPageTitle()
        {
            //Title = PageViewMeta.Title;
            //if (LblTitle != null)
            //    LblTitle.Text = PageViewMeta.Title;
        }

        #endregion

        #region 用抽象函数的方式设置ModuleID
        /// <summary>
        /// 设置ModuleID。通过URL参数 mid 获取。
        /// </summary>
        protected virtual void SetModuleID()
        {
            //DataList.aspx、DataForm.aspx 页面通过URL里的参数设置。
            ModuleID = URLParamVerification.ModuleID(Context);
        }
        #endregion

        #region 用抽象函数的方式设置SetMasterPageViewID
        /// <summary>
        /// 设置主列表视图ID。通过URL参数 mpvid 获取。
        /// </summary>
        protected virtual void SetMasterPageViewID()
        {
            //验证页面视图ID参数是否是数字。
            MasterPageViewID = URLParamVerification.PageViewID(Context, "mpvid");
            
        }
        #endregion

        #region 用抽象函数的方式设置SetFindPageViewID
        /// <summary>
        /// 设置查询视图ID。通过URL参数 fid 获取。
        /// </summary>
        protected virtual void SetFindPageViewID()
        {
            //验证查询视图ID参数是否是数字。
            FindPageViewID = URLParamVerification.PageViewID(Context, "fpvid");

        }
        #endregion

        #region 用抽象函数的方式设置DataID
        /// <summary>
        /// 设置DataID。通过URL参数 id 获取。
        /// </summary>
        protected virtual void SetDataID()
        {
            //DataList.aspx、DataForm.aspx 页面通过URL里的参数设置。
            //其他页面自行设置
            DataID = URLParamVerification.DataID(Context);

        }
        #endregion

        #region 用抽象函数的方式设置ButtonID
        /// <summary>
        /// 设置ButtonID。通过URL参数 bid 获取。
        /// </summary>
        protected virtual void SetButtonID()
        {
            //验证按钮ID是否是数字
            ButtonID = URLParamVerification.PageViewID(Context, "bid");
        }
        #endregion

        #region 验证部门ID
        /// <summary>
        /// 验证部门ID
        /// </summary>
        protected void SetDepartmentID()
        {
            //验证部门ID参数是否是数字。
            DepartmentID = URLParamVerification.PageViewID(Context, "did");
        }
        #endregion

        #region 验证外键
        /// <summary>
        /// 验证外键。通过URL参数 frid 获取。
        /// </summary>
        protected virtual void SetForeignID()
        {
            //验证外键ID参数是否是数字。
            ForeignID = URLParamVerification.FormDataID(Context, "frid");
           
        }
        #endregion

     

    }
}
