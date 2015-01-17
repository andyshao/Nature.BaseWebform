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
 * function: 数据列表页面的基类
 *           权限控制（部分），控件的属性设置。
 * history:  created by 金洋  2009-8-31 16:49:03 
 *           2011-4-19  金洋  由于分页控件的FunctionID属性被去掉了，因此需要调整一下相关的代码
 * **********************************************
 */


using System;
using System.Web.UI.WebControls;
using Nature.Common;
using Nature.Data;
using Nature.UI.WebControl.MetaControl;
using Nature.UI.WebControl.QuickPager;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 列表的基类。给共用控件设置属性和事件。
    /// </summary>
    public partial class BasePageList 
    {
         
        #region 设置分页控件的属性
        /// <summary>
        /// 从元数据里提取信息，设置分页控件的属性
        /// </summary>
        public virtual void SetQuickPagerInfo()
        {
            
            if (CtlCommonPager != null)
            {
                CtlCommonPager.Dal = Dal.DalCustomer;             //设置客户的数据访问函数库

                CtlCommonPager.PagerSql.Page = this;                   //设置Page，用于保存属性的状态

                if (CtlCommonGrid != null)
                    CtlCommonPager.ShowDataControl = CtlCommonGrid;   //绑定数据显示控件

                LoadPagerInfo(CtlCommonPager, Dal.DalMetadata, ModuleID);             //从配置信息里面提取数据

                //获取配置信息里面的“固定查询条件”
                string tableQueryAlways = CtlCommonPager.PagerSql.TableQueryAlways;
                string tmpQuery ;

                #region 判断是否传入id，如果传入了再判断是否有外键。显示从表的列表的时候使用
                if (DataID != null)
                {
                    string foreignColumn = PageViewMeta.ForeignColumn;
                    if (!string.IsNullOrEmpty(foreignColumn))
                    {
                        //设置固定查询条件
                        if (Functions.IsInt(DataID))
                            tmpQuery = foreignColumn + "=" + DataID;
                        else
                            tmpQuery = foreignColumn + "='" + DataID + "'";

                        if (tableQueryAlways.Length == 0)
                            tableQueryAlways = tmpQuery;
                        else
                            tableQueryAlways += " and " + tmpQuery;
                    }
                }
                #endregion

                #region 判断用户角色是否有记录的过滤方案
                //tmpQuery = MyUser.GetResourceListCastSQL(ModuleID);
                //if (tmpQuery.Length > 0)
                //{
                //    tmpQuery = tmpQuery.Replace("{userid}", MyUser.UserID);
                //    tmpQuery = tmpQuery.Replace("{personid}", MyUser.PersonID);

                //    //有过滤方案，即查询语句。添加到分页控件的固定查询条件里。)
                //    if (tableQueryAlways.Length == 0)
                //        tableQueryAlways = tmpQuery;
                //    else
                //        tableQueryAlways += " and " + tmpQuery;
                //}
                #endregion

                //设置分页控件的固定查询条件。
                CtlCommonPager.PagerSql.TableQueryAlways = tableQueryAlways;

            }

        }
        #endregion

        #region 加载分页控件需要的信息

        /// <summary>
        /// 从元数据里面提取分页用的信息，给分页控件设置属性
        /// </summary>
        /// <param name="myPager"></param>
        /// <param name="dal">连接配置信息所在的数据库的实例</param>
        /// <param name="moduleID">模块ID</param>
        /// <returns></returns>
        public  string LoadPagerInfo(QuickPager myPager, IDal dal, int moduleID)
        {
            //通过元数据管理程序，获取元数据里的分页信息
            var pagerInfo = PageViewMeta.PageTurnMeta;

            if (pagerInfo != null)
            {
                myPager.PagerSql.TableName = pagerInfo.TableNameList;            //表名或者字段名
                myPager.PagerSql.TablePKColumn = pagerInfo.PKColumn;             //主键
                myPager.PagerSql.TableShowColumns = pagerInfo.ShowColumns;       //显示的字段
                myPager.PagerSql.TableOrderByColumns = pagerInfo.OrderColumns;   //排序字段
                myPager.PagerSql.PageSize = pagerInfo.PageSize;                  //一页的记录数
                myPager.NaviCount = pagerInfo.NaviCount;                         //页号导航的数量

                myPager.PagerSql.TableQueryAlways = pagerInfo.QueryAlways;       //固定查询条件
                myPager.PagerSql.TableQuery = pagerInfo.Query;                   //临时查询条件

                //设置分页算法
                //myPager.PagerSql.SetPagerSQLKind = (PagerSQLKind) pagerInfo.PageTurnType;

            }

            return "";
        }
        #endregion

        #region 设置显示数据控件的属性
        /// <summary>
        /// 设置显示数据控件的属性
        /// </summary>
        public virtual void SetGridInfo()
        {
            if (CtlCommonGrid != null)
            {
                SetGridInfo(CtlCommonGrid);
            }
        }

        /// <summary>
        /// 设置数据显示控件实例的属性
        /// </summary>
        /// <param name="grid">数据显示控件MyGrid</param>
        public void SetGridInfo(DataBoundControl grid)
        {
            var tmpGrid = grid as DataGridList;
            if (tmpGrid != null)
            {
                tmpGrid.DalCollection = Dal ;//设置客户数据的数据访问函数库
                
                tmpGrid.PageViewID = MasterPageViewID;
                //设置当前用户可以访问的字段——即权限到列表字段

                tmpGrid.CanUseColumns = MyUser.UserPermission.GetUserColumnIDs(MasterPageViewID, Dal.DalMetadata);
                tmpGrid.UserOnlineInfo = MyUser;
                
            }
        }

        #endregion


        #region 设置查询控件的属性
        /// <summary>
        /// 设置查询控件的属性
        /// </summary>
        public virtual void SetFindControlInfo()
        {
            if (CtlCommonFind != null)
                SetFindControlInfo(CtlCommonFind);

            if (BtnSearch != null)
                BtnSearch.Click += BtnSearchClick;
        }

        /// <summary>
        /// 设置查询控件的属性
        /// </summary>
        /// <param name="find">查询控件的实例</param>
        public void SetFindControlInfo(FindForm find)
        {
            find.DalCollection = Dal;//设置客户数据的数据访问函数库
            find.PageViewID = FindPageViewID;
            
            //Response.Write(PageViewMeta.ColumnCount);
            
            //find.UserOnlineInfo = MyUser; MyUser.GetUserColumnIDs(ModuleID, "3", DalMetadata);
          
        }
        #endregion

        #region 查询按钮触发的事件
        /// <summary>
        /// 查询按钮触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void BtnSearchClick(object sender, EventArgs e)
        {
            string tmp = CtlCommonFind.GetSearchWhere();

            if (tmp.Length > 0)
            {
                if (tmp.Substring(0, 1) == "【")
                {
                    //查询关键字有误，不能查询
                    if (LblTitle != null)
                    {
                        LblTitle.Text = tmp;
                    }
                    else
                    {
                        Functions.PageRegisterAlert(Page, tmp);
                    }
                    return;
                }
            }

            CtlCommonPager.PagerSql.TableQuery = CtlCommonFind.GetSearchWhere();
            CtlCommonPager.PagerSql.CreateSQL();
            CtlCommonPager.BindFirstPage();

            Response.Write(CtlCommonFind.GetSearchWhere());
        }
        #endregion

        #region 设置按钮组
        /// <summary>
        /// 设置按钮组
        /// </summary>
        public virtual void SetButtonBarInfo()
        {
            if (CtlCommonButtonBar != null)
            {
                //CtlCommonButtonBar.PageViewID = MasterPageViewID;
                CtlCommonButtonBar.ModuleID = ModuleID;
                CtlCommonButtonBar.DalCollection = Dal;

                if (MyUser.BaseUser.UserID == "1")
                    //超级管理员不过滤按钮
                    CtlCommonButtonBar.UserCanUseButtonID = "";
                else
                    //其他用户验证按钮
                    CtlCommonButtonBar.UserCanUseButtonID = MyUser.UserPermission.GetUserButtonID(ModuleID, Dal.DalMetadata);

            }
        }
        #endregion

        //验证
        #region 验证DataID
        /// <summary>
        /// 验证DataID。列表页面DataID不是必须传递的参数。
        /// </summary>
        protected override void SetDataID()
        {
            DataID = URLParamVerification.FormDataID(Context, "id");
        }
        #endregion

        #region 验证列表页面与FunctionID是否匹配
        /// <summary>
        /// 验证列表页面与FunctionID是否匹配
        /// </summary>
        protected override void SetMasterPageViewID()
        {
            //根据ModuleID，获取第一个PageViewID
            //string sql = "SELECT TOP (1) PVID FROM Manage_PageView WHERE (ModuleID = {0})  ";
            //MasterPageViewID = Dal.DalMetadata.ExecuteScalar<int>(string.Format(sql, ModuleID));
            
            base.SetMasterPageViewID();

            #region 验证列表页面与FunctionID是否匹配
            string webPath = Request.Url.LocalPath;

            switch (webPath)
            {
                case "/CommonPage/DataList.aspx":     //通用列表页面
                case "/CommonPage/DataListView.aspx":     //通用列表页面
                case "/CommonPage/DataTab.aspx":       //通用标签页面
                case "/CommonPage/DataChoose.aspx":    //通用选择数据的页面
                    //不需要做匹配验证
                    break;

                default:
                    //需要验证
                    //string sql = "select top 1 1 from Manage_Module where ModuleID = " + ModuleID + " and URL like '" + webPath + "%'";
                    string sql = "select top 1 1 from Manage_Module where [ModuleID] = {0} and [URL] like '{1}%'";
                    sql = string.Format(sql, ModuleID, webPath);

                    if (Dal.DalMetadata.ExecuteExists(sql))
                    {
                        //有记录，通过验证
                    }
                    else
                    {
                        //没有记录，不能继续
                        //Response.Write("ModuleID和页面不匹配！");
                        //Response.End();
                    }
                    break;
            }
            #endregion
        }
        #endregion

    }
}
