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
 * function: 表单的基类
 *           权限控制（部分），控件的属性设置。
 * history:  created by 金洋  2009-8-31 15:03:24 
 * **********************************************
 */


using System;
using Nature.Common;
using Nature.MetaData.Enum;
using Nature.MetaData.Manager;
using Nature.UI.WebControl.MetaControl;

namespace Nature.BaseWebform
{
    /// <summary>
    /// 表单的基类
    /// </summary>
    public partial class BasePageForm 
    {
        #region 第一次访问要做的事情
        /// <summary>
        /// 第一次访问要做的事情，设置表单控件的属性，和按钮。
        /// </summary>
        protected virtual void Initialization()
        {
            switch (ButonType)
            {
                case ButonType.AddUpdateData: //添加空记录，然后修改记录的情况
                    AddNothingData();
                    FrmCommonForm.OpenButonType = ButonType.UpdateData;
                    break;

                case ButonType.UpdateData:   //修改，设置保存按钮
                    if (BtnSaveContinue != null)
                    {
                        BtnSaveContinue.Text = " 保 存 ";
                        BtnSaveContinue.Visible = false;
                    }
                    break;

                case ButonType.ViewData:
                    if (BtnSaveContinue != null)
                        BtnSaveContinue.Visible = false;
                    if (BtnSave != null)
                        BtnSave.Visible = false;
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 设置lbl_Msg。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            if (LblMsg != null)
            {
                LblMsg.ForeColor = System.Drawing.Color.Black;
                LblMsg.Text = "";
            }
        }

        //表单控件
        #region 通过ButtonID，设置表单的操作方式，添加、修改、查看
        /// <summary>
        /// 通过ButtonID，设置表单的操作方式，添加、修改、查看
        /// </summary>
        protected virtual void SetKind()
        {
            //string sql = "select BtnTypeID from Manage_ButtonBar where ButtonID =" + ButtonID;
            const string sql = "select BtnTypeID from Manage_ButtonBar where ButtonID ={0}"; // +ButtonID;

            string btnTypeID = Dal.DalMetadata.ExecuteString(string.Format(sql, ButtonID));

            if (btnTypeID == null)
            {
                //没有找到按钮类型，不能继续
                Response.Write("按钮的类型不正确！");
                Response.End();
                return;

            }

            #region 验证BtnTypeID是否符合在允许的范围内
            //转换成枚举类型
            ButonType = (ButonType)Int32.Parse(btnTypeID);

            switch (ButonType)
            {
                case ButonType.AddData  :        //添加
                case ButonType.UpdateData :        //修改
                case ButonType.ViewData :        //查看
                case ButonType.AddUpdateData :        //添加后修改
                    //按钮类型正确。
                    break;

                default:
                    Response.Write("按钮类型不正确！");
                    Response.End();
                    break;

            }
            #endregion

         
        }
        #endregion

        #region 设置表单控件需要的属性和事件
        /// <summary>
        /// 设置表单控件需要的属性和事件，FunctionID、ControlKind、SetDataID
        /// </summary>
        protected virtual void SetFormControlInfo()
        {
            //设置表单控件的属性
            if (FrmCommonForm != null)
            {
                SetFormControlInfo(FrmCommonForm);

                //注册表单绑定后触发的事件
                FrmCommonForm.FormBinded += FrmCommonFormFormBinded;
            }
        }

        /// <summary>
        /// 设置表单控件的属性
        /// </summary>
        /// <param name="form">表单控件</param>
        public void SetFormControlInfo(DataForm form)
        {
            
            //myMgrFormMeta.OperationDataKind = FormKind;   //表单的类型：添加、修改、查看、添加后修改
           
            //form.ControlKind = MyFormKind.FormControl;
            //form.User = MyUser;

            form.PageViewID = MasterPageViewID;
            form.DalCollection = Dal;

            //通过按钮ID寻找按钮的操作类型
            const string sql = "select BtnTypeID from Manage_ButtonBar where ButtonID = {0}";
            int btnTypeID = Dal.DalMetadata.ExecuteScalar<int>(string.Format(sql, ButtonID));
            form.OpenButonType = (ButonType)btnTypeID;  // ButonType.ViewData;
            
            form.DataID = DataID;

            form.RepeatColumns = PageViewMeta.ColumnCount ;  //表单的列数
           
           // myMgrFormMeta.RoleColumnID = MyUser.GetUserColumnIDs(functionID, "2", DalMetadata);

            

        }
        #endregion


        //按钮事件
        #region 表单控件绑定后触发的事件
        /// <summary>
        /// 表单控件绑定后触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void FrmCommonFormFormBinded(object sender, EventArgs e)
        {
            #region 设置控件的默认焦点
            Functions.PageRegisterJavascript(Page, "myTxtID=\"" + FrmCommonForm.FirstChildControlClinetID + "\";");
            #endregion

            if (!Page.IsPostBack)
            {
                #region 设置添加人等
                //MyDropDownList lst;
                //设置添加人、添加日期
                //if (ButonType == ButonType.AddData)
                //{
                //    //添加数据
                //    lst = (MyDropDownList)FrmCommonForm.GetControl("1000130");//添加人
                //    if (lst != null)
                //    {
                //        lst.Items[0].Value = MyUser.PersonID;
                //        lst.Items[0].Text = MyUser.PersonName;
                //    }
                //    FrmCommonForm.SetControlValue("1000120", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//添加日期
                //}
                //else if (ButonType == ButonType.UpdateData || ButonType == ButonType.AddUpdateData)
                //{
                //    lst = (MyDropDownList)FrmCommonForm.GetControl("1000130");//添加人
                //    //通过Value 查找添加人姓名
                //    if (lst != null)
                //    {
                //        string personID = lst.Items[0].Value;
                //        lst.Items[0].Text = Dal.DalMetadata.ExecuteString("select 姓名 from Person_Info where PersonID = " + personID);
                //    }
                //}

                #endregion
            }
        }
        #endregion

        #region 保存后关闭
        /// <summary>
        /// 添加、修改记录，添加后关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnSaveClick(object sender, EventArgs e)
        {
            var operateLog = new ManagerLogOperate
            {
                AddUserID = Int32.Parse(MyUser.BaseUser.UserID),
                Dal = Dal.DalCustomer,
                ModuleID = ModuleID,
                ButtonID = ButtonID,
                PageViewID = MasterPageViewID
            };

            //定义数据变更日志
            var dataChangeLog = new ManagerLogDataChange
            {
                AddUserID = Int32.Parse(MyUser.BaseUser.UserID),
                Dal = Dal
            };

            //保存数据
            string err = FrmCommonForm.SaveData(operateLog, dataChangeLog);


            if (err.Length > 0)
            {
                //有错误发生不能继续。
                Response.Write(err);
            }
            else
            {
                switch (ButonType)
                {
                    case ButonType.AddData:
                        //添加后关闭
                        Functions.PageRegisterJavascript(Page, "ReloadFirst(true)");
                        break;
                    case ButonType.UpdateData:
                        //保存后关闭
                        Functions.PageRegisterJavascript(Page, "ReloadForUpdate(true)");
                        break;
                }
            }
        }

        #endregion

        #region 添加后继续
        /// <summary>
        /// 添加、修改记录，添加后可以继续添加新纪录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnSaveContinueClick(object sender, EventArgs e)
        {
            var operateLog = new ManagerLogOperate
            {
                AddUserID = Int32.Parse(MyUser.BaseUser.UserID),
                Dal = Dal.DalCustomer,
                ModuleID = ModuleID,
                ButtonID = ButtonID,
                PageViewID = MasterPageViewID
            };

            //定义数据变更日志
            var dataChangeLog = new ManagerLogDataChange
            {
                AddUserID = Int32.Parse(MyUser.BaseUser.UserID),
                Dal = Dal
            };

            //保存数据
            string err = FrmCommonForm.SaveData(operateLog, dataChangeLog);

            if (err.Length > 0)
            {
                //有错误发生不能继续。
                LblMsg.ForeColor = System.Drawing.Color.Red;
                LblMsg.Text = err;
            }
            else
            {
                switch (ButonType)
                {
                    case ButonType.AddData:
                        //添加后继续
                        Functions.PageRegisterJavascript(Page, "ReloadFirst(false)");
                        break;
                    case ButonType.UpdateData:
                        //修改后关闭
                        Functions.PageRegisterJavascript(Page, "ReloadForUpdate(true)");
                        break;
                }
                LblMsg.ForeColor = System.Drawing.Color.Blue;
                LblMsg.Text = "保存成功！您可以继续添加记录。";
                FrmCommonForm.Reset();
            }

        }
        #endregion


        //按钮
        #region 设置流程按钮的属性和事件
        /// <summary>
        /// 设置流程按钮的属性和事件
        /// </summary>
        protected virtual void SetButtonInfo()
        {
            //给“流程按钮”加前后台事件
            if (BtnSave != null)
            {
                BtnSave.Attributes.Add("onclick", "return myCheck()");
                BtnSave.Click += BtnSaveClick;
            }

            if (BtnSaveContinue != null)
            {
                BtnSaveContinue.Attributes.Add("onclick", "return myCheck()");
                BtnSaveContinue.Click += BtnSaveContinueClick;
            }
        }
        #endregion
        
        //参数验证
        #region 验证DataID
        /// <summary>
        /// 验证DataID。
        /// </summary>
        protected override void SetDataID()
        {
            DataID = URLParamVerification.FormDataID(Context, "id");
        }
        #endregion

        #region 验证表单页面与ButtonID是否匹配
        /// <summary>
        /// 验证表单页面与ButtonID是否匹配，防止伪造URL。
        /// </summary>
        protected override void SetButtonID()
        {
            base.SetButtonID();

            #region 验证表单页面与ButtonID是否匹配
            string webPath = Request.Url.LocalPath;

            switch (webPath)
            {
                case "/CommonPage/DataForm1.aspx":     //通用表单页面
                    //不需要做匹配验证
                    break;

                default:
                    //需要验证
                    //string sql = "select top 1 1 from Manage_ButtonBar where ButtonID  = " + ButtonID + " and URL like '" + webPath + "%'";
                    string sql = "select top 1 1 from Manage_ButtonBar where ButtonID  = {0} and URL like '{1}%'";
                    sql = string.Format(sql, ButtonID, webPath);

                    if (Dal.DalMetadata.ExecuteExists(sql))
                    {
                        //有记录，通过验证
                    }
                    else
                    {
                        //没有记录，不能继续
                        Response.Write("ButtonID和页面不匹配！");
                        Response.End();
                    }
                    break;
            }
            #endregion

        }
        #endregion

        #region 验证是否可以修改记录
        /// <summary>
        /// 验证是否可以修改记录
        /// </summary>
        public virtual void CheckCanUpdate()
        {
            //调用MyUser里的CheckCanUpdate 来判断是否可以修改指定的记录
            string re = MyUser.UserPermission.CheckCanUpdate(ModuleID,MasterPageViewID , DataID, Dal.DalMetadata,null);
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
