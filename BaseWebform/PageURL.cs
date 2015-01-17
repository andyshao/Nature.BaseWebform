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
 *           处理URL参数。
 *           接收URL传递过来的模块ID，大部分页面都需要使用这个ID
 * history:  created by 金洋 
 * **********************************************
 */

using System;
using System.Web.UI.WebControls;
using Nature.Common;
using Nature.MetaData.Entity.WebPage;
using Nature.MetaData.ManagerMeta;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 处理URL参数
    /// 接收URL传递过来的模块ID，大部分页面都需要使用这个ID
    /// </summary>
    public partial class PageURL : PageUserInfo
    {
        #region 定义属性，记录URL参数值
        /// <summary>
        /// ModuleID，功能节点ID
        /// </summary>
        public int ModuleID;

        /// <summary>
        /// OpenPageViewID，主视图ID：列表视图ID，或者表单视图ID
        /// </summary>
        public int MasterPageViewID;

        /// <summary>
        /// FindPageViewID，查询视图ID
        /// </summary>
        public int FindPageViewID;

        /// <summary>
        /// 接收URL传递过来的记录ID，用于显示、修改、删除、查询数据
        /// </summary>
        public string DataID;

        /// <summary>
        /// 接收URL传递过来的记录ID，用于显示、修改、删除、查询数据
        /// </summary>
        public string DataIDs;

        /// <summary>
        ///  按钮ID，用于是否可用按钮的验证
        /// </summary>
        public int ButtonID;


        /// <summary>
        /// 部门ID。string.Empty 表示没有传递参数
        /// </summary>
        public int DepartmentID;

        /// <summary>
        /// foreign key column 外键ID。string.Empty 表示没有传递参数
        /// </summary>
        public string ForeignID = string.Empty;

        /// <summary>
        /// foreign key column 外键ID。string.Empty 表示没有传递参数
        /// </summary>
        public string ForeignIDs = string.Empty;

        /// <summary>
        /// 当前节点的描述信息
        /// </summary>
        public PageViewMeta PageViewMeta; 

        #endregion

        /// <summary>
        /// 页面里的标题
        /// </summary>
        public Label LblTitle;             //页面里的标题

        #region 初始化 在OnInit执行
        /// <summary>
        /// 提取URL里面的参数，验证参数
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
           
            //调用函数来设置ModuleID，不同类型的页面就可以用不同的方式来设置FunctionID了。
            SetModuleID();

            //调用函数来设置PageViewID，不同类型的页面就可以用不同的方式来设置FunctionID了。
            SetMasterPageViewID();

            //调用函数来设置PageViewID，不同类型的页面就可以用不同的方式来设置FunctionID了。
            SetFindPageViewID();

            //调用函数来设置ButtonID。
            SetButtonID();

            //调用函数来设置DataID。
            SetDataID();

            //调用函数来设置DepartmentID（部门ID）。
            SetDepartmentID();

            //调用函数来设置ForeignID（外键ID）。
            SetForeignID();

            DataIDs = Request["ids"];
            ForeignIDs = Request["frids"];

            if (!string.IsNullOrEmpty(ForeignIDs))
                ForeignIDs = ForeignIDs.Trim('"');

            if (!Functions.IsIDString(DataIDs))
            {
                DataIDs = "";
            }

            if (!Functions.IsIDString(ForeignIDs))
            {
                ForeignIDs = "";

            }
            if (!Page.IsPostBack)
            {
                //设置标题
                SetPageTitle();
            }

            var managerPageView = new ManagerPageViewMeta { DalCollection = Dal, PageViewID = MasterPageViewID };
            PageViewMeta = managerPageView.GetPageViewMeta(null);

        }
        #endregion

        #region 清除IE缓存
        /// <summary>
        /// 清除IE缓存
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //清除IE缓存
            Response.Cache.SetNoStore();
        }
        #endregion

       
    }
}
