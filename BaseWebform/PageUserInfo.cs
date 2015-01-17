/**
 * ��Ȼ���֮��Ϣ��������Ŀ��ҳ�����
 * http://www.natureFW.com/
 *
 * @author
 * ���󣨽�ɫ����jyk��
 * 
 * @copyright
 * Copyright (C) 2005-2013 ����.
 *
 * Licensed under a GNU Lesser General Public License.
 * http://creativecommons.org/licenses/LGPL/2.1/
 *
 * ��Ȼ���֮��Ϣ��������Ŀ��ҳ����� is free software. You are allowed to download, modify and distribute 
 * the source code in accordance with LGPL 2.1 license, however if you want to use 
 * ��Ȼ���֮��Ϣ��������Ŀ��ҳ����� on your site or include it in your commercial software, you must  be registered.
 * http://www.natureFW.com/registered
 */

/* ***********************************************
 * author :  ���󣨽�ɫ����jyk��
 * email  :  jyk0011@live.cn  
 * function: ��̨�����ҳ����ࡣ
 *           ��֤�Ƿ��¼
 *           ��ȡ��ǰ��¼�˵���Ϣ��
 *           ��ȡBaseUserInfo��ʵ��
 *           Ȩ����֤��������������֤FunctionID�Ƿ���ȷ����֤FunctionID��ButtonID�Ƿ�ƥ��
 * history:  created by ���� 
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
    /// �û���ز�����
    /// ��֤�Ƿ��¼
    /// ��ȡ��ǰ��¼�˵���Ϣ��
    /// ��ȡBaseUserInfo��ʵ��
    /// Ȩ����֤��������������֤FunctionID�Ƿ���ȷ����֤FunctionID��ButtonID�Ƿ�ƥ��
    /// </summary>
    public class PageUserInfo : BasePage
    {
        /// <summary>
        /// ���浱ǰ��¼�˵�һЩ��Ϣ
        /// </summary>
        public UserOnlineInfo MyUser; //= UserManage.UserInfo;
        
        #region ��ʼ�� ��OnInitִ��
        /// <summary>
        /// ���õ�ǰ��¼�˵���Ϣ
        /// ����Ԫ���ݵ����ݿ��ʵ��
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            var debugInfo = new NatureDebugInfo { Title = "[Nature.Service.Ashx.BaseAshx]�ж�Url����" };

            base.OnInit(e);

            //��֤�Ƿ��Ѿ���¼
            //����Ѿ���¼�ˣ����ص�¼��Ա����Ϣ��
            var manageUser = new ManageUser { Dal = Dal };

            UserWebappInfo userWebappInfo = AppManage.UserWebappInfoByCookies(debugInfo.DetailList);

            if (userWebappInfo.State != UserState.NormalAccess)
            {
                //û�е�¼��
                Response.Write("����û�е�¼���뵽<a href='/default.aspx' target='_top'>����</a>��¼");
                Response.End();
            }

            MyUser = manageUser.CreateUser(Convert.ToString(userWebappInfo.UserWebappID), null);

            //���������־
            //SaveViewLog();
           
        }

        #endregion

        #region �����û��������¼
        /// <summary>
        /// ��¼�û������ҳ��ļ�¼
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
