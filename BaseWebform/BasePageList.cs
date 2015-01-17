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
 * function: 列表页面的基类，定义控件：分页控件、数据显示控件、查询按钮、按钮组、指定执行顺序。
 *           权限控制（部分），控件的属性设置。
 * history:  created by 金洋  2009-8-31 16:49:03 
 * **********************************************
 */


using System;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Nature.UI.WebControl.MetaControl;
using Nature.UI.WebControl.QuickPager;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 列表页面的基类
    /// </summary>
    public partial class BasePageList : PageURL
    {
        #region 定义共用的控件，以便于统一控制
        /// <summary>
        /// 按钮组，功能（操作）按钮
        /// </summary>
        public OperationButtonBar CtlCommonButtonBar;

        /// <summary>
        /// 查询控件
        /// </summary>
        public FindForm CtlCommonFind;

        /// <summary>
        /// 显示数据用的控件
        /// </summary>
        public DataBoundControl CtlCommonGrid;

        /// <summary>
        /// 分页用的控件
        /// </summary>
        public QuickPager CtlCommonPager;

        /// <summary>
        /// 查询按钮
        /// </summary>
        public Button BtnSearch;

        #endregion

        
        #region 在 OnInit 事件里面设置各个自定义控件的属性和关联
        /// <summary>
        /// 在 OnInit 事件里面设置各个自定义控件的属性和关联
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            //父类里面验证参数是否正确
            base.OnInit(e);

            var cssLink = new HtmlLink();
            cssLink.Href = "/aspnet_client/css/cssList.css";
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            //Page.Header.Controls.Add(cssLink);

            //验证是否有权限访问。
            MyUser.UserPermission.CheckModuleID(ModuleID.ToString(CultureInfo.InvariantCulture));

            //设置分页控件的属性和事件
            SetQuickPagerInfo();

            //设置数据表格的属性
            SetGridInfo();

            //设置查询控件的属性
            SetFindControlInfo();

            //设置操作按钮的属性
            SetButtonBarInfo();

        }
        #endregion

       
    }
}
