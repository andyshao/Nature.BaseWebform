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
 * function: 表单页面的基类，可以在这里定义一些公用的控件，保存按钮、表单控件。
 *           权限控制（部分），控件的属性设置。
 * history:  created by 金洋 2009-8-18 14:37:55 
 * **********************************************
 */

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Nature.MetaData.Enum;
using Nature.UI.WebControl.MetaControl;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 表单页面的基类，可以在这里定义一些公用的控件，保存按钮、表单控件。
    /// 权限控制（部分），控件的属性设置。
    /// </summary>
    public partial class BasePageForm : PageURL
    {
        #region 定义属性和控件

        protected ButonType ButonType;

        /// <summary>
        /// 保存按钮
        /// </summary>
        protected Button BtnSave;             //保存后关闭窗口按钮

        /// <summary>
        /// 保存后可以继续添加的按钮
        /// </summary>
        protected Button BtnSaveContinue;     //保存后继续添加按钮

        /// <summary>
        /// 表单控件
        /// </summary>
        protected DataForm FrmCommonForm;

        /// <summary>
        /// 显示添加是否成功。
        /// </summary>
        protected Label LblMsg;                //显示信息
        #endregion

        #region 初始化 在OnInit执行
        /// <summary>
        /// 表单页面，设置控件的属性，验证是否可以访问指定的记录
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "/aspnet_client/css/cssForm.css";
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            //Page.Header.Controls.Add(cssLink);

            //验证当前用户是否可以使用这个页面。
            //通过ModuleID和buttonID来验证。
            MyUser.UserPermission.CheckButtonID(ModuleID, ButtonID.ToString(CultureInfo.InvariantCulture), Dal.DalRole);

            //验证是否可以操作记录
            CheckCanUpdate();
            
            //通过ButtonID，设置表单的操作方式，添加、修改、查看
            SetKind();

            //设置表单控件的属性和事件
            SetFormControlInfo();

            //设置流程按钮的属性和事件
            SetButtonInfo();

         
        }

        #region OnInit 之后，Page_Load之前执行。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                //第一次访问要做的事情
                Initialization();
            }

            base.OnLoad(e);
        }
        #endregion


        #endregion

        #region 添加一条空数据
        private void AddNothingData()
        {
            //用于一对一的单表，先添加一条空记录，然后就可以按照修改的方式来处理。

            //string sql = "select top 1 1 from " + FunInfo.CRUDInfo.TableNameAdd + " where " + FunInfo.PagerInfo.PKColumn + " = " + this.DataID;
            string sql = "select top 1 1 from {0} where {1} = {2}" ;
            sql = string.Format(sql, PageViewMeta.ModiflyTableName, PageViewMeta.PKColumn, DataID);

            if (!Dal.DalCustomer.ExecuteExists(sql))
            {
                sql = "insert into {0} ({1}) values ({2}) ";
                sql = string.Format(sql, PageViewMeta.ModiflyTableName, PageViewMeta.PKColumn, DataID);

                Dal.DalCustomer.ExecuteNonQuery(sql);

            }
        }
        #endregion

      
    }
}
