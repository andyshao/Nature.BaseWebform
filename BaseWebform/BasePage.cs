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
 * function: 后台管理的页面基类。最基础的基类，定义数据访问的实例，定义css的路径，css的文件名
 * history:  created by 金洋 
 *           2012-9-8 整理
 * **********************************************
 */

using System;
using Nature.Data;
using Nature.Service;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 后台管理的页面基类。定义数据访问的实例
    /// 设置皮肤的路径，css的文件名
    /// 生成数据访问函数库的实例:
    /// 1、访问客户数据
    /// 2、访问元数据
    /// 3、访问角色
    /// 4、访问用户
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        #region 设置皮肤，不成熟的做法
        /// <summary>
        /// 皮肤的路径
        /// </summary>
        public string SkinPath
        {
            get
            {
                //先弄个假的
                return "/App_Themes/default/";
            }
        }

        /// <summary>
        /// 树的Css
        /// </summary>
        public string CssTree
        {
            get
            {
                return "";// "<link type=\"text/css\" rel=\"stylesheet\" href=\"" + SkinPath + "tree.css\"/> ";
            }
        }

        /// <summary>
        /// 列表页面里用的Css
        /// </summary>
        public string CssWebGrid
        {
            get
            {
                return "";// "<link type=\"text/css\" rel=\"stylesheet\" href=\"" + SkinPath + "cssList.css\"/> ";
            }
        } 

        /// <summary> 
        /// 表单页面里用的Css
        /// </summary>
        public string CssWebForm
        {
            get
            {
                return "";// "<link type=\"text/css\" rel=\"stylesheet\" href=\"" + SkinPath + "cssForm.css\"/> ";
            }
        }
        #endregion

        #region 访问数据库的实例，四个

        /// <summary>
        /// 访问数据库的实例的集合，四个
        /// </summary>
        /// <value>
        /// 访问数据库的实例的集合
        /// </value>
        /// user:jyk
        /// time:2012/9/13 10:52
        public DalCollection Dal { set; get; }

        #endregion

        #region 初始化 在OnInit执行
        /// <summary>
        /// 设置当前登录人的信息
        /// 设置元数据的数据库的实例
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //设置元数据的数据库的实例
            Dal = CommonClass.SetMetadataDal();

        }
        #endregion

       

    }

}

