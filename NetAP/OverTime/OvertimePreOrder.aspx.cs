﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoPac.WebExpress.Common;
using SinoPac.WebExpress.DAO;
using SinoPac.WebExpress.Work;
using SinoPac.WebExpress.Common.Properties;

public partial class OverTime_OvertimePreOrder : BasePage
{
    private string _overtimeDBName = Util.getAppSetting("app://AattendantDB_OverTime/");
    public static string _eHRMSDB = Util.getAppSetting("app://eHRMSDB_OverTime/"); //20170207 leo modify 特殊人員 查詢組織用
    private Aattendant at = new Aattendant();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblOTCompName.Text = UserInfo.getUserInfo().CompName; //公司名稱
            lblDeptName.Text = UserInfo.getUserInfo().DeptName; //單位名稱
            lblOrganName.Text = UserInfo.getUserInfo().OrganName; //科別名稱
            txtOTEmpID.Text = UserInfo.getUserInfo().UserID;

            //↓↓↓↓↓↓↓↓↓↓特殊人員 20170207 leo modify start↓↓↓↓↓↓↓↓↓↓↓
            DbHelper db = new DbHelper(_overtimeDBName);
            CommandHelper sb = db.CreateCommandHelper();
            sb.Append("Select CompID,''''+Replace (OrgList,',',''',''')+'''' as OrgList,''''+Replace (OrgFlowList,',',''',''')+'''' as OrgFlowList from OverTimeSPUser where EmpID='" + UserInfo.getUserInfo().UserID + "'");
            DataTable SPUser = db.ExecuteDataSet(sb.BuildCommand()).Tables[0];
            if (SPUser.Rows.Count == 0)
            {
                trSPUser.Visible = false;
                trSPUserFlow.Visible = false;
                trSPUserOrgAndFlow.Visible = false;
                trQuery.Attributes.Add("class", "Util_clsRow2");
                trQuery2.Attributes.Add("class", "Util_clsRow1");

            }
            else
            {
                ViewState["CompID"] = SPUser.Rows[0]["CompID"].ToString();
                ViewState["OrgList"] = SPUser.Rows[0]["OrgList"].ToString();
                ViewState["OrgFlowListOrganID"] = SPUser.Rows[0]["OrgFlowList"].ToString();
                ViewState["OrgFlowList"] =
                "select DISTINCT " +
                "case  when O40.UpOrganID is not null then O40.UpOrganID else '' end as O40UpOrganID," +
                "isnull(O40.OrganID,'') as O40OrganID," +
                    //"isnull(O40.OrganID+'-'+O40.OrganName,'') as O40OrganName," +
                "case  when O30.UpOrganID is not null then O30.UpOrganID else '' end as O30UpOrganID," +
                "isnull(O30.OrganID,'') as O30OrganID," +
                    //"isnull(O30.OrganID+'-'+O30.OrganName,'') as O30OrganName," +
                "case  when O20.UpOrganID is not null then O20.UpOrganID else '' end as O20UpOrganID," +
                "isnull(O20.OrganID,'') as O20OrganID," +
                    //"isnull(O20.OrganID+'-'+O20.OrganName,'') as O20OrganName," +
                "case  when O10.UpOrganID is not null then O10.UpOrganID else (case when O0.UpOrganID is not null then (select UpOrganID from " + _eHRMSDB + ".[dbo].[OrganizationFlow] where OrganID=O0.UpOrganID) else'' end) end as O10UpOrganID," +
                "case  when  O0.OrganID is not null then O0.UpOrganID else isnull(O10.OrganID,'') end as O10OrganID," +
                    //"isnull(O10.OrganID+'-'+O10.OrganName,'') as O10OrganName," +
                "case  when  O0.UpOrganID is not null then O0.UpOrganID else '' end as O0UpOrganID," +
                "isnull( O0.OrganID,'') as  O0OrganID," +
                    //"isnull(case O0.OrganID when null then null else '└─' +O0.OrganID+'-'+O0.OrganName end,'') as O0OrganName," +

                //"isnull(O40.OrganID,isnull(O30.OrganID,isnull(O20.OrganID,isnull(O10.OrganID,isnull( O0.OrganID,''))))) as UpOrganID," +
                "isnull( O0.OrganID,isnull(O10.OrganID,isnull(O20.OrganID,isnull(O30.OrganID,isnull(O40.OrganID,''))))) as DownOrganID," +
                "isnull( case O0.OrganID when null then null else '└─' +O0.OrganID+'-'+O0.OrganName end,isnull(O10.OrganID+'-'+O10.OrganName,isnull(O20.OrganID+'-'+O20.OrganName,isnull(O30.OrganID+'-'+O30.OrganName,isnull(O40.OrganID+'-'+O40.OrganName,''))))) as DownOrganName " +
                    //"case O0.OrganID when   null then 'A10' else 'B0' end as RankSort " +
                "from " + _eHRMSDB + ".[dbo].[OrganizationFlow] O " +
                "left join " + _eHRMSDB + ".[dbo].[OrganizationFlow] O40 on  O40.RoleCode='40' and O40.OrganID =O.OrganID " +
                "left join " + _eHRMSDB + ".[dbo].[OrganizationFlow] O30 on  O30.RoleCode='30' and O30.OrganID =O.OrganID " +
                "left join " + _eHRMSDB + ".[dbo].[OrganizationFlow] O20 on  O20.RoleCode='20' and O20.OrganID =O.OrganID " +
                "left join " + _eHRMSDB + ".[dbo].[OrganizationFlow] O10 on  O10.RoleCode='10' and O10.OrganID =O.OrganID " +
                "left join " + _eHRMSDB + ".[dbo].[OrganizationFlow] O0 on  O0.RoleCode='0'and O0.OrganID =O.OrganID " +
                "where O.OrganID in (" + SPUser.Rows[0]["OrgFlowList"].ToString() + ") " +
                "Order by O40OrganID,O30OrganID,O20OrganID,O10OrganID,O0OrganID ";
                string test = ViewState["OrgFlowList"].ToString();
                //ddlGet("OrgType");
                //ddlGetFlow(40);
                OrgAndFlowChanged("");
            }
            //↑↑↑↑↑↑↑↑↑↑特殊人員 20170207 leo modify end↑↑↑↑↑↑↑↑↑↑↑
        }
    }
    //跳到新增頁面
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("OvertimePreOrder_Add.aspx");
    }
    //跳到修改頁(狀態為暫存可修改)
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        LoadCheckData();
        if (ViewState["dt"] == null)
        {
            Util.MsgBox("尚未有資料可以修改");
            return;
        }
        else
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Rows.Count <= 0)
            {
                Util.MsgBox("尚未勾選資料");
                return;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 1)
                    {
                        Util.MsgBox("修改只能一次一筆");
                        return;
                    }
                    else
                    {
                        if (dt.Rows[0]["OTStatusName"].ToString() != "1")
                        {
                            Util.MsgBox("此狀態無法修改");
                            return;
                        }
                        else
                        {
                            Response.Redirect("OvertimePreOrder_Modify.aspx?OTCompID=" + dt.Rows[0]["OTCompID"] + "&OTEmpID=" + dt.Rows[0]["EmpID"] + "&OTStartDate=" + dt.Rows[0]["OTStartDate"] + "&OTEndDate=" + dt.Rows[0]["OTEndDate"] + "&OTStartTime=" + dt.Rows[0]["OTStartTime"] + "&OTEndTime=" + dt.Rows[0]["OTEndTime"] + "&OTTxnID=" + dt.Rows[0]["OTTxnID"] + "&OTFormNO=" + dt.Rows[0]["OTFormNO"]);
                        }
                    }
                }

            }
        }
    }

    //清除按鈕
    protected void btnActionX_Click(object sender, EventArgs e)
    {
        ddlQueryConditions.SelectedValue = "0";
        txtOTEmpID.Text = UserInfo.getUserInfo().UserID;
        txtOTEmpName.Text = "";
        txtOTFormNO.Text = "";
        ddlOTStatus.SelectedValue = "0";
        txtOTDateBegin.ucSelectedDate = "";
        txtOTDateEnd.ucSelectedDate = "";
        gvMain.Visible = false;
        //20170210 leo modify
        if (trSPUser.Visible && trSPUserFlow.Visible)
        {
            ddlGet("OrgType");
            ddlGetFlow(40);
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //if (txtOTEmpID.Text == "" && !trSPUser.Visible)  // 20170213 leo modify
        //{
        //    Util.MsgBox("員工編號為必輸欄位");
        //    return;
        //}
        //else if (txtOTEmpID.Text == "" && ddlOrgAndFlow.SelectedValue == "" && ddlQueryConditions.SelectedValue != "0")
        //{
        //    Util.MsgBox("員工編號為必輸欄位");
        //    return;
        //}
        //當查詢條件為填單人員或加班人員，員工編號必輸
        if (txtOTEmpID.Text == "" && ddlQueryConditions.SelectedValue != "0")
        {
            Util.MsgBox("員工編號為必輸欄位");
            return;
        }
        else
        {
            RefreshGrid();
        }
    }

    //刪除按鈕(狀態為暫存可刪除，OTStatus='5')
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LoadCheckData();
        if (gvMain.Visible == false && gvMain.Rows.Count <= 0)
        {
            Util.MsgBox("尚未有資料可以刪除");
            return;
        }
        else if (gvMain.Visible == true && gvMain.Rows.Count == 0)
        {
            Util.MsgBox("尚未有資料可以刪除");
            return;
        }
        else if (ViewState["dt"] != null)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoDelete();", true);
            }
            else
            {
                Util.MsgBox("尚未勾選資料");
                return;
            }
        }
    }
    protected void btnDeleteInvisible_Click(object sender, EventArgs e)
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        DbConnection cn = db.OpenConnection();
        DbTransaction tx = cn.BeginTransaction();
        LoadCheckData();
        if (gvMain.Visible == true && gvMain.Rows.Count > 0)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Rows.Count > 0)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["OTStatusName"].ToString() != "1")
                    {
                        Util.MsgBox("狀態無法刪除");
                        return;
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["OTStartDate"].ToString() == dt.Rows[i]["OTEndDate"].ToString())
                    {
                        sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='5'");
                        sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                        sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                        sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'");
                        sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "'");
                        sb.Append(" AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "'");
                        sb.Append(" AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "'");
                        sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                    }
                    else
                    {
                        string crossDayArray = dt.Rows[i]["OTStartDate"] + "," + dt.Rows[i]["OTEndDate"];
                        for (int j = 0; j < crossDayArray.Split(',').Length; j++)
                        {
                            if (crossDayArray.Split(',')[j] == dt.Rows[i]["OTStartDate"].ToString())
                            {
                                sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='5'");
                                sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                                sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                                sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'");
                                sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTStartDate"] + "'");
                                sb.Append(" AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "'");
                                sb.Append(" AND OTEndTime='2359'");
                                sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                            }
                            else
                            {
                                sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='5'");
                                sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                                sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                                sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTEndDate"] + "'");
                                sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "'");
                                sb.Append(" AND OTStartTime='0000'");
                                sb.Append(" AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "'");
                                sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                            }
                        }
                    }
                }
            }
            try
            {
                db.ExecuteNonQuery(sb.BuildCommand(), tx);
                tx.Commit();
                Util.MsgBox("刪除成功！");
                RefreshGrid();
            }
            catch (Exception ex)
            {
                LogHelper.WriteSysLog(ex); //將 Exception 丟給 Log 模組
                tx.Rollback();//資料更新失敗
                Util.MsgBox("刪除失敗！");
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                tx.Dispose();
            }
        }
    }

    //取消按鈕
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        LoadCheckData();
        if (gvMain.Visible == false && gvMain.Rows.Count <= 0)
        {
            Util.MsgBox("尚未有資料可以取消");
            return;
        }
        else if (gvMain.Visible == true && gvMain.Rows.Count == 0)
        {
            Util.MsgBox("尚未有資料可以取消");
            return;
        }
        else if (ViewState["dt"] != null)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoCancel();", true);
            }
            else
            {
                Util.MsgBox("尚未勾選資料");
                return;
            }
        }
    }
    protected void btnCancelInvisible_Click(object sender, EventArgs e)
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        DbConnection cn = db.OpenConnection();
        DbTransaction tx = cn.BeginTransaction();
        CommandHelper sbDecl = db.CreateCommandHelper();
        FlowExpress flow = new FlowExpress(Util.getAppSetting("app://AattendantDB_OverTime/"));
        LoadCheckData();
        string OTDateDecla = "";
        string OTFormNODecla = "";
        string strbegin = "將同步取消";
        string strend = "的加班申報單";
        string msg = "";
        string msg0 = "內含申報已送簽、核准資料，請重新選取<br/>";
        string msg1 = "";
        DataTable dtDecl = null;
        if (gvMain.Visible == true && gvMain.Rows.Count > 0)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Rows.Count > 0)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["OTStatusName"].ToString() == "3" || dt.Rows[j]["OTStatusName"].ToString() == "2") //"核准""送簽"
                    {
                        if (dt.Rows[j]["OTStatusName"].ToString() == "3")
                        {
                            sbDecl.Reset();
                            sbDecl.AppendStatement(" SELECT OTCompID,OTEmpID,OTStartDate,OTEndDate,OTStartTime,OTEndTime,OTSeq,OTStatus,P.NameN FROM OverTimeDeclaration D");
                            sbDecl.Append(" LEFT JOIN " + _eHRMSDB + ".[dbo].[Personal] P ON D.OTCompID = P.CompID AND D.OTEmpID = P.EmpID ");
                            sbDecl.Append(" WHERE OTFromAdvanceTxnId='" + dt.Rows[j]["OTTxnID"] + "'");
                            DataTable dtAdv1 = db.ExecuteDataSet(sbDecl.BuildCommand()).Tables[0];
                            if (dtAdv1.Rows.Count > 0 && dtAdv1.Rows[0]["OTStatus"].ToString() != "1")
                            {
                                if (dtAdv1.Rows.Count == 1) //不跨日單
                                {
                                    msg1 += dtAdv1.Rows[0]["OTEmpID"].ToString() + "‧";
                                    msg1 += dtAdv1.Rows[0]["NameN"].ToString() + "‧";
                                    msg1 += dtAdv1.Rows[0]["OTStartDate"].ToString() + "~" + dtAdv1.Rows[0]["OTStartDate"].ToString() + "‧";
                                    msg1 += dtAdv1.Rows[0]["OTStartTime"].ToString().Substring(0, 2) + ":" + dtAdv1.Rows[0]["OTStartTime"].ToString().Substring(2, 2) + "~" + dtAdv1.Rows[0]["OTEndTime"].ToString().Substring(0, 2) + ":" + dtAdv1.Rows[0]["OTEndTime"].ToString().Substring(2, 2);
                                    msg1 += "\r\n";
                                }
                                else
                                {
                                    msg1 += dtAdv1.Rows[0]["OTEmpID"].ToString() + "‧";
                                    msg1 += dtAdv1.Rows[0]["NameN"].ToString() + "‧";
                                    msg1 += dtAdv1.Rows[0]["OTStartDate"].ToString() + "~" + dtAdv1.Rows[1]["OTStartDate"].ToString() + "‧";
                                    msg1 += dtAdv1.Rows[0]["OTStartTime"].ToString().Substring(0, 2) + ":" + dtAdv1.Rows[0]["OTStartTime"].ToString().Substring(2, 2) + "~" + dtAdv1.Rows[1]["OTEndTime"].ToString().Substring(0, 2) + ":" + dtAdv1.Rows[1]["OTEndTime"].ToString().Substring(2, 2);
                                    msg1 += "\r\n";
                                }
                                continue;
                            }
                        }
                        continue;
                    }
                    else
                    {
                        Util.MsgBox("狀態無法取消");
                        return;
                    }
                }
                if (msg1.Length > 0)
                {
                    Util.MsgBox(msg0 + msg1);
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    /******************************************/
                    //找事後單
                    sbDecl.Reset();
                    sbDecl.AppendStatement(" SELECT OTCompID,OTEmpID,OTStartDate,OTEndDate,OTStartTime,OTEndTime,OTSeq,OTStatus,P.NameN,OTFormNO FROM OverTimeDeclaration D");
                    sbDecl.Append(" LEFT JOIN " + _eHRMSDB + ".[dbo].[Personal] P ON D.OTCompID = P.CompID AND D.OTEmpID = P.EmpID ");
                    sbDecl.Append(" WHERE OTFromAdvanceTxnId='" + dt.Rows[i]["OTTxnID"] + "'");
                    dtDecl = db.ExecuteDataSet(sbDecl.BuildCommand()).Tables[0];
                    if (dtDecl.Rows.Count > 0)
                    {
                        if (dt.Rows[i]["OTStatusName"].ToString() == "3")
                        {
                            if (dtDecl.Rows.Count == 1) //不跨日單
                            {
                                if (dtDecl.Rows[0]["OTStatus"].ToString() == "1") //Update事後申報狀態為刪除
                                {
                                    OTDateDecla = "";
                                    OTFormNODecla = "";
                                    OTDateDecla += dtDecl.Rows[0]["OTStartDate"].ToString() + "~" + dtDecl.Rows[0]["OTEndDate"].ToString() + "\t";
                                    OTFormNODecla += dtDecl.Rows[0]["OTFormNO"].ToString() + " ";
                                    msg += OTDateDecla + OTFormNODecla;
                                }
                            }
                            else if (dtDecl.Rows.Count == 2) //跨日單
                            {
                                OTDateDecla = "";
                                OTFormNODecla = "";
                                for (int m = 0; m < dtDecl.Rows.Count; m++)
                                {
                                    if (m == 0)//起日
                                    {
                                        OTDateDecla += dtDecl.Rows[0]["OTStartDate"].ToString() + "~";
                                    }
                                    else if (m == 1) //迄日
                                    {
                                        OTDateDecla += dtDecl.Rows[1]["OTEndDate"].ToString() + " ";
                                        OTFormNODecla += dtDecl.Rows[1]["OTFormNO"].ToString() + " ";
                                        msg += OTDateDecla + OTFormNODecla;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (msg.Length > 0)
            {
                msg = strbegin + msg + strend;
                ShowUpdateDeclaraConfirm(msg);
            }
            else
            {
                CancelTwoTable();
            }
        }
    }
    private void CancelTwoTable()
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        DbConnection cn = db.OpenConnection();
        DbTransaction tx = cn.BeginTransaction();
        CommandHelper sbDecl = db.CreateCommandHelper();
        FlowExpress flow = new FlowExpress(Util.getAppSetting("app://AattendantDB_OverTime/"));
        LoadCheckData();
        string strFlowCaseID = "";
        string OTDateDecla = "";
        string OTFormNODecla = "";
        string msg = "";
        DataTable dtDecl = null;
        DataTable dt1 = null;
        DataTable dt = (DataTable)ViewState["dt"];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            /******************************************/
            //找事後單
            sbDecl.Reset();
            sbDecl.AppendStatement(" SELECT OTCompID,OTEmpID,OTStartDate,OTEndDate,OTStartTime,OTEndTime,OTSeq,OTStatus,P.NameN,OTFormNO FROM OverTimeDeclaration D");
            sbDecl.Append(" LEFT JOIN " + _eHRMSDB + ".[dbo].[Personal] P ON D.OTCompID = P.CompID AND D.OTEmpID = P.EmpID ");
            sbDecl.Append(" WHERE OTFromAdvanceTxnId='" + dt.Rows[i]["OTTxnID"] + "'");
            dtDecl = db.ExecuteDataSet(sbDecl.BuildCommand()).Tables[0];
            if (dtDecl.Rows.Count > 0)
            {
                if (dt.Rows[i]["OTStatusName"].ToString() == "3")
                {
                    if (dtDecl.Rows.Count == 1) //不跨日單
                    {
                        if (dtDecl.Rows[0]["OTStatus"].ToString() == "1") //Update事後申報狀態為刪除
                        {
                            OTDateDecla = "";
                            OTFormNODecla = "";
                            sb.AppendStatement("UPDATE OverTimeDeclaration SET OTStatus='5'");
                            sb.Append(" WHERE OTCompID='" + dtDecl.Rows[0]["OTCompID"] + "'");
                            sb.Append(" AND OTEmpID='" + dtDecl.Rows[0]["OTEmpID"] + "'");
                            sb.Append(" AND OTStartDate='" + dtDecl.Rows[0]["OTStartDate"] + "'");
                            sb.Append(" AND OTEndDate='" + dtDecl.Rows[0]["OTEndDate"] + "'");
                            sb.Append(" AND OTStartTime='" + dtDecl.Rows[0]["OTStartTime"] + "'");
                            sb.Append(" AND OTEndTime='" + dtDecl.Rows[0]["OTEndTime"] + "'");
                            sb.Append(" AND OTSeq='" + dtDecl.Rows[0]["OTSeq"] + "'");
                            sb.Append(" AND OTSeqNo='1';");
                            OTDateDecla += dtDecl.Rows[0]["OTStartDate"].ToString() + "~" + dtDecl.Rows[0]["OTEndDate"].ToString() + "\t";
                            OTFormNODecla += dtDecl.Rows[0]["OTFormNO"].ToString() + " ";
                            msg += OTDateDecla + OTFormNODecla;
                        }
                    }
                    else if (dtDecl.Rows.Count == 2) //跨日單
                    {
                        OTDateDecla = "";
                        OTFormNODecla = "";
                        for (int m = 0; m < dtDecl.Rows.Count; m++)
                        {
                            if (m == 0)
                            {
                                //起日
                                sb.AppendStatement("UPDATE OverTimeDeclaration SET OTStatus='5'");
                                sb.Append(" WHERE OTCompID='" + dtDecl.Rows[m]["OTCompID"] + "'");
                                sb.Append(" AND OTEmpID='" + dtDecl.Rows[m]["OTEmpID"] + "'");
                                sb.Append(" AND OTStartDate='" + dtDecl.Rows[m]["OTStartDate"] + "'");
                                sb.Append(" AND OTEndDate='" + dtDecl.Rows[m]["OTEndDate"] + "'");
                                sb.Append(" AND OTStartTime='" + dtDecl.Rows[m]["OTStartTime"] + "'");
                                sb.Append(" AND OTEndTime='2359'");
                                sb.Append(" AND OTSeq='" + dtDecl.Rows[m]["OTSeq"] + "'");
                                sb.Append(" AND OTSeqNo='1';");
                                OTDateDecla += dtDecl.Rows[0]["OTStartDate"].ToString() + "~";
                            }
                            else if (m == 1)
                            {
                                //迄日
                                sb.AppendStatement("UPDATE OverTimeDeclaration SET OTStatus='5'");
                                sb.Append(" WHERE OTCompID='" + dtDecl.Rows[m]["OTCompID"] + "'");
                                sb.Append(" AND OTEmpID='" + dtDecl.Rows[m]["OTEmpID"] + "'");
                                sb.Append(" AND OTStartDate='" + dtDecl.Rows[m]["OTStartDate"] + "'");
                                sb.Append(" AND OTEndDate='" + dtDecl.Rows[m]["OTEndDate"] + "'");
                                sb.Append(" AND OTStartTime='0000'");
                                sb.Append(" AND OTEndTime='" + dtDecl.Rows[m]["OTEndTime"] + "'");
                                sb.Append(" AND OTSeq='" + dtDecl.Rows[m]["OTSeq"] + "'");
                                sb.Append(" AND OTSeqNo='2';");

                                OTDateDecla += dtDecl.Rows[1]["OTEndDate"].ToString() + " ";
                                OTFormNODecla += dtDecl.Rows[1]["OTFormNO"].ToString() + " ";
                                msg += OTDateDecla + OTFormNODecla;
                            }
                        }
                    }
                }
            }
            /******************************************/
            if (dt.Rows[i]["OTStartDate"].ToString() == dt.Rows[i]["OTEndDate"].ToString())
            {
                strFlowCaseID = at.QueryColumn("FlowCaseID", "OverTimeAdvance", "AND OTStatus in ('2','3') AND OTEmpID='" + dt.Rows[i]["EmpID"] + "' AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "' AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "' AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "' AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "' AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='9'");
                sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'");
                sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "'");
                sb.Append(" AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "'");
                sb.Append(" AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "'");
                sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
            }
            else
            {
                string crossDayArray = dt.Rows[i]["OTStartDate"] + "," + dt.Rows[i]["OTEndDate"];
                for (int j = 0; j < crossDayArray.Split(',').Length; j++)
                {
                    strFlowCaseID = at.QueryColumn("FlowCaseID", "OverTimeAdvance", "AND OTStatus in ('2','3') AND OTEmpID='" + dt.Rows[i]["EmpID"] + "' AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'  AND OTEndDate='" + dt.Rows[i]["OTStartDate"] + "' AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "' AND OTEndTime='2359' AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                    if (crossDayArray.Split(',')[j] == dt.Rows[i]["OTStartDate"].ToString())
                    {
                        sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='9'");
                        sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                        sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                        sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'");
                        sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTStartDate"] + "'");
                        sb.Append(" AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "'");
                        sb.Append(" AND OTEndTime='2359'");
                        sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                    }
                    else
                    {
                        sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='9'");
                        sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                        sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                        sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTEndDate"] + "'");
                        sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "'");
                        sb.Append(" AND OTStartTime='0000'");
                        sb.Append(" AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "'");
                        sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                    }
                }
            }

            dt1 = at.QueryData("TOP 1 *", flow.FlowCustDB + "FlowFullLog", " AND FlowCaseID='" + strFlowCaseID + "' ORDER BY FlowLogBatNo DESC");
            if (dt1.Rows.Count > 0)
            {
                string strLastLogBatNo = (Convert.ToInt32(dt1.Rows[0]["FlowLogBatNo"].ToString()) + 1).ToString();
                string strLastLogSeqNo = (Convert.ToInt32(dt1.Rows[0]["FlowLogBatNo"].ToString()) + 1).ToString();
                string strIsProxy = "N";

                sb.AppendStatement("UPDATE " + flow.FlowCustDB + "FlowCase SET FlowCaseStatus='Close',FlowCurrStepID='Z03',FlowCurrStepName='取消結案',");
                sb.Append(" LastLogBatNo='" + strLastLogBatNo + "',LastLogSeqNo='" + strLastLogSeqNo + "',UpdDateTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                sb.Append(" WHERE FlowCaseID='" + strFlowCaseID + "'");

                sb.AppendStatement("UPDATE " + flow.FlowCustDB + "FlowFullLog SET FlowLogIsClose='Y',FlowStepBtnID='btnCancel',FlowStepBtnCaption='取消結案'");
                sb.Append(" ,LogUpdDateTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                sb.Append(" ,ToDept='" + UserInfo.getUserInfo().DeptID + "'");
                sb.Append(" ,ToDeptName='" + UserInfo.getUserInfo().DeptName + "'");
                sb.Append(" ,ToUser='" + UserInfo.getUserInfo().UserID + "'");
                sb.Append(" ,ToUserName='" + UserInfo.getUserInfo().UserName + "'");
                sb.Append(" ,IsProxy='" + strIsProxy + "'");
                sb.Append(" WHERE FlowLogID='" + dt1.Rows[0]["FlowLogID"] + "'");

                string b = Convert.ToString(strFlowCaseID + "." + (Convert.ToInt32(dt1.Rows[0]["FlowLogBatNo"]) + 1).ToString("00000"));

                sb.AppendStatement(" INSERT INTO " + flow.FlowCustDB + "FlowFullLog(FlowCaseID,FlowLogBatNo,FlowLogID,FlowStepID,FlowStepName,FlowStepBtnID,FlowStepBtnCaption,FlowStepOpinion,FlowLogIsClose,IsProxy,AttachID,FromDept,FromDeptName,FromUser,FromUserName,AssignTo,AssignToName,ToDept,ToDeptName,ToUser,ToUserName,LogCrDateTime,LogUpdDateTime,LogRemark) ");
                sb.Append(" VALUES('" + strFlowCaseID + "', '" + strLastLogBatNo + "', '" + b + "',");
                sb.Append(" 'Z03','取消結案','','','','Y','','',");
                sb.Append(" '" + UserInfo.getUserInfo().DeptID + "','" + UserInfo.getUserInfo().DeptName + "','" + UserInfo.getUserInfo().UserID + "','" + UserInfo.getUserInfo().UserName + "','" + UserInfo.getUserInfo().UserID + "','" + UserInfo.getUserInfo().UserName + "','" + UserInfo.getUserInfo().DeptID + "','" + UserInfo.getUserInfo().DeptName + "','" + UserInfo.getUserInfo().UserID + "','" + UserInfo.getUserInfo().UserName + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                sb.Append(" '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','')");

                sb.AppendStatement(" DELETE FROM " + flow.FlowCustDB + "FlowOpenLog WHERE FlowCaseID='" + strFlowCaseID + "'");

                //加進HROverTimeLog
                DataTable dtHROverTimeLog = at.QueryData("top 1 *", "HROverTimeLog", " AND FlowCaseID='" + strFlowCaseID + "' order by Seq desc");
                if (dtHROverTimeLog.Rows.Count > 0)
                {
                    FlowUtility.InsertHROverTimeLogCommand(strFlowCaseID, strLastLogBatNo, b,
                          "A", dtHROverTimeLog.Rows[0]["OTEmpID"].ToString(), dtHROverTimeLog.Rows[0]["EmpOrganID"].ToString(), dtHROverTimeLog.Rows[0]["EmpFlowOrganID"].ToString(), UserInfo.getUserInfo().UserID,
                          dtHROverTimeLog.Rows[0]["FlowCode"].ToString(), dtHROverTimeLog.Rows[0]["FlowSN"].ToString(), dtHROverTimeLog.Rows[0]["FlowSeq"].ToString(), dtHROverTimeLog.Rows[0]["SignLine"].ToString(),
                          UserInfo.getUserInfo().CompID, UserInfo.getUserInfo().UserID, UserInfo.getUserInfo().OrganID, "", "4",
                          false, ref sb, Convert.ToInt32(dtHROverTimeLog.Rows[0]["Seq"].ToString()) + 1, "0", i);
                }
            }
        }
        try
        {
            db.ExecuteNonQuery(sb.BuildCommand(), tx);
            tx.Commit();
            Util.MsgBox("取消成功！");
            RefreshGrid();
        }
        catch (Exception ex)
        {
            LogHelper.WriteSysLog(ex); //將 Exception 丟給 Log 模組
            tx.Rollback();//資料更新失敗
            Util.MsgBox("取消失敗！");
        }
        finally
        {
            cn.Close();
            cn.Dispose();
            tx.Dispose();
        }
    }
    protected void ShowUpdateDeclaraConfirm(string msg)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoUpdateDeclara('" + msg + "');", true);//
    }
    protected void CancelUpdateDecla_Click(object sender, EventArgs e)
    {
        CancelTwoTable();
    }

    //送簽按鈕
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        LoadCheckData();
        if (gvMain.Visible == false && gvMain.Rows.Count <= 0)
        {
            Util.MsgBox("尚未有資料可以送簽");
            return;
        }
        else if (gvMain.Visible == true && gvMain.Rows.Count == 0)
        {
            Util.MsgBox("尚未有資料可以送簽");
            return;
        }
        else if (ViewState["dt"] != null)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Rows.Count > 0)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoSubmit();", true);
                //ucLightBox.ucLightBoxMsg = "送簽中，請稍後";
                //btnSubmitInvisible.OnClientClick = ucLightBox.ucShowClientJS;
                if (checkMulitData())
                {
                    ShowSubmitConfirm();
                }
            }
            else
            {
                Util.MsgBox("尚未勾選資料");
                return;
            }
        }
    }
    public bool checkMulitData()
    {
        if (gvMain.Visible && gvMain.Rows.Count > 0)
        {
            DataTable _dtPara = at.Json2DataTable(at.QueryColumn("Para", "OverTimePara", " AND CompID = '" + UserInfo.getUserInfo().CompID + "'"));
            DataTable dt = (DataTable)ViewState["dt"];
            string strMsg = at.GetMulitTotal(dt, Convert.ToDouble(_dtPara.Rows[0]["MonthLimitHour"].ToString()), "OverTimeAdvance");
            string message = "";
            ViewState["message"] = message;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (dt.Rows[j]["OTStatusName"].ToString() == "2") //if (dt.Rows[j]["OTStatusName"].ToString() == "送簽")
                {
                    Util.MsgBox("此狀態已是送簽");
                    return false;
                }
                if (dt.Rows[j]["OTStatusName"].ToString() != "1") //if (dt.Rows[j]["OTStatusName"].ToString() != "暫存")
                {
                    Util.MsgBox("狀態無法送簽");
                    return false;
                }
                //if (Convert.ToDateTime(dt.Rows[j]["OTStartDate"].ToString()) > DateTime.Now.Date)
                //{
                //    Util.MsgBox("日期為未來日期，無法送簽");
                //    return false;
                //}
                //else if (Convert.ToDateTime(dt.Rows[j]["OTStartDate"].ToString()) == DateTime.Now.Date)
                //{
                //    if (Convert.ToInt32(dt.Rows[j]["OTStartTime"].ToString().Substring(0, 2)) > Convert.ToInt32(DateTime.Now.Hour))
                //    {
                //        Util.MsgBox("日期為未來日期，無法送簽");
                //        return false;
                //    }
                //    else if (Convert.ToInt32(dt.Rows[j]["OTStartTime"].ToString().Substring(0, 2)) == Convert.ToInt32(DateTime.Now.Hour) && Convert.ToInt32(dt.Rows[j]["OTStartTime"].ToString().Substring(2, 2)) > Convert.ToInt32(DateTime.Now.Minute))
                //    {
                //        Util.MsgBox("日期為未來日期，無法送簽");
                //        return false;
                //    }
                //    if (Convert.ToInt32(dt.Rows[j]["OTEndTime"].ToString().Substring(0, 2)) > Convert.ToInt32(DateTime.Now.Hour))
                //    {
                //        Util.MsgBox("日期為未來日期，無法送簽");
                //        return false;
                //    }
                //    else if (Convert.ToInt32(dt.Rows[j]["OTEndTime"].ToString().Substring(0, 2)) == Convert.ToInt32(DateTime.Now.Hour) && Convert.ToInt32(dt.Rows[j]["OTEndTime"].ToString().Substring(2, 2)) > Convert.ToInt32(DateTime.Now.Minute))
                //    {
                //        Util.MsgBox("日期為未來日期，無法送簽");
                //        return false;
                //    }
                //}
            }
            //檢查每天的上限
            double dayNLimit = Convert.ToDouble(_dtPara.Rows[0]["DayLimitHourN"].ToString());//平日可申請
            double dayHLimit = Convert.ToDouble(_dtPara.Rows[0]["DayLimitHourH"].ToString());//假日可申請
            string strcheckOverTimeIsOver = at.GetCheckOverTimeIsOver(dt, dayNLimit, dayHLimit, "OverTimeAdvance");
            if (!Convert.ToBoolean(strcheckOverTimeIsOver.Split(';')[0]))
            {
                if (_dtPara == null)
                {
                    Util.MsgBox("請聯絡HR確認是否有設定參數值");
                    return false;
                }
                else
                {

                    if (_dtPara.Rows[0]["MonthLimitFlag"].ToString() == "1")
                    {
                        Util.MsgBox("員編(" + strcheckOverTimeIsOver.Split(';')[1] + ")" + strcheckOverTimeIsOver.Split(';')[2] + "已超過每天上限加班時數" + strcheckOverTimeIsOver.Split(';')[3] + "小時");
                        return false;
                    }
                    else
                    {
                        message = "員編(" + strcheckOverTimeIsOver.Split(';')[1] + ")" + strcheckOverTimeIsOver.Split(';')[2] + "已超過每天上限加班時數" + strcheckOverTimeIsOver.Split(';')[3] + "小時";
                        ViewState["message"] = message;
                    }
                }
            }
            //檢查每個月的上限
            if (!Convert.ToBoolean(strMsg.Split(';')[0]))
            {
                if (_dtPara == null)
                {
                    Util.MsgBox("請聯絡HR確認是否有設定參數值");
                    return false;
                }
                else
                {

                    if (_dtPara.Rows[0]["MonthLimitFlag"].ToString() == "1")
                    {
                        Util.MsgBox("員編(" + strMsg.Split(';')[1] + ")" + (strMsg.Split(';')[2]).ToString().Substring(5, 2) + "月已超過每月上限加班時數" + _dtPara.Rows[0]["MonthLimitHour"] + "小時");
                        return false;
                    }
                    else
                    {
                        message = "員編(" + strMsg.Split(';')[1] + ")" + (strMsg.Split(';')[2]).ToString().Substring(5, 2) + "月已超過每月上限加班時數" + _dtPara.Rows[0]["MonthLimitHour"] + "小時";
                        ViewState["message"] = message;
                    }
                }
            }
            string strGetCheckOTLimitDay = at.GetCheckOTLimitDay(dt, _dtPara.Rows[0]["OTLimitDay"].ToString(), "OverTimeAdvance");
            if (!Convert.ToBoolean(strGetCheckOTLimitDay.Split(';')[0]))
            {

                if (_dtPara.Rows[0]["OTLimitFlag"].ToString() == "1")
                {
                    Util.MsgBox("員編(" + strGetCheckOTLimitDay.Split(';')[1] + ")" + "不得連續上班超過" + _dtPara.Rows[0]["OTLimitDay"].ToString() + "天");
                    return false;
                }
                else
                {
                    message = "員編(" + strGetCheckOTLimitDay.Split(';')[1] + ")" + "不得連續上班超過" + _dtPara.Rows[0]["OTLimitDay"].ToString() + "天";
                    ViewState["message"] = message;
                }
            }
        }
        return true;
    }
    protected void ShowSubmitConfirm()
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoSubmit();", true);
        ////燈箱
        //ucLightBox.ucLightBoxMsg = "送簽中，請稍後";
        //btnSubmitInvisible.OnClientClick = ucLightBox.ucShowClientJS;
        string msg = "";
        if (string.IsNullOrEmpty(ViewState["message"].ToString()))
        {
            msg = "是否要送簽？";
            ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoSubmit('" + msg + "');", true);
            //燈箱
            ucLightBox.ucLightBoxMsg = "送簽中，請稍後";
            btnSubmitInvisible.OnClientClick = ucLightBox.ucShowClientJS;
        }
        else
        {
            msg = ViewState["message"].ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoSubmit_Reminder('" + msg + "');", true);
        }
    }
    protected void btnSubmitInvisible_Reminder_Click(object sender, EventArgs e)
    {
        string msg = "是否要送簽？";
        ClientScript.RegisterStartupScript(this.GetType(), "confirm", "funYesOrNoSubmit('" + msg + "');", true);
        //燈箱
        ucLightBox.ucLightBoxMsg = "送簽中，請稍後";
        btnSubmitInvisible.OnClientClick = ucLightBox.ucShowClientJS;
    }

    protected void btnSubmitInvisible_Click(object sender, EventArgs e)
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        FlowExpress flow = new FlowExpress(Util.getAppSetting("app://AattendantDB_OverTime/"));
        Dictionary<string, string> oAssTo = new Dictionary<string, string>();
        var isSuccess = false;
        var toUserData = new Dictionary<string, string>();
        var empData = new Dictionary<string, string>();
        var flowCode = "";
        var flowSN = "";
        var nextIsLastFlow = false;
        var meassge = "";
        string IndexMsg = "";
        string eHRMSDB = Util.getAppSetting("app://eHRMSDB_OverTime/");
        DataTable _dtPara = at.Json2DataTable(at.QueryColumn("Para", "OverTimePara", " AND CompID = '" + UserInfo.getUserInfo().CompID + "'"));
        bool DoIt = false;
        LoadCheckData(); //產生ViewState["dt"]

        if (gvMain.Visible == true && gvMain.Rows.Count > 0)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DoIt = false;
                    #region"FlowUtility共用檔撈取審核人員資料"
                    isSuccess = FlowUtility.QueryFlowDataAndToUserData_First(dt.Rows[i]["OTCompID"].ToString(), "", "", dt.Rows[i]["EmpID"].ToString(), UserInfo.getUserInfo().UserID, dt.Rows[i]["OTStartDate"].ToString(), "0",
                    out empData, out toUserData, out flowCode, out flowSN, out nextIsLastFlow, out meassge);
                    if (!isSuccess)
                    {
                        //Util.MsgBox(meassge);
                        IndexMsg = IndexMsg + "第" + dt.Rows[i]["RowIndex"].ToString() + "項-查無加班人相關資料，故無法送簽。" + "\n";
                        continue;
                    }
                    if ("".Equals(toUserData["SignID"]))
                    {
                        //Util.MsgBox("查無審核人員，故無法送簽。");
                        IndexMsg = IndexMsg + "第" + dt.Rows[i]["RowIndex"].ToString() + "項-查無審核人員，故無法送簽。" + "\n";
                        continue;
                    }
                    #endregion"FlowUtility共用檔撈取審核人員資料"

                    #region"流程所需資料前置"
                    //逐筆送簽
                    oAssTo.Clear();
                    string name = at.QueryColumn("NameN", eHRMSDB + ".[dbo].[Personal]", " AND EmpID='" + toUserData["SignID"] + "' AND CompID='" + toUserData["SignIDComp"] + "'");
                    //需加部門
                    string organName = "";

                    if (toUserData["SignLine"] == "1")
                    {
                        organName = at.QueryColumn("OrganName", eHRMSDB + ".[dbo].[Organization]", " AND OrganID=(SELECT DeptID FROM " + eHRMSDB + ".[dbo].[Personal] WHERE EmpID='" + toUserData["SignID"] + "' AND CompID='" + toUserData["SignIDComp"] + "')");
                    }
                    else if (toUserData["SignLine"] == "2")
                    {
                        organName = at.QueryColumn("OrganName", eHRMSDB + ".[dbo].[OrganizationFlow]", " AND OrganID=(SELECT DeptID FROM " + eHRMSDB + ".[dbo].[Personal] WHERE EmpID='" + toUserData["SignID"] + "' AND CompID='" + toUserData["SignIDComp"] + "')");
                    }
                    else if (toUserData["SignLine"] == "3")
                    {
                        organName = at.QueryColumn("OrganName", eHRMSDB + ".[dbo].[Organization]", " AND OrganID=(SELECT DeptID FROM " + eHRMSDB + ".[dbo].[Personal] WHERE EmpID='" + toUserData["SignID"] + "' AND CompID='" + toUserData["SignIDComp"] + "')");
                    }

                    oAssTo.Add(toUserData["SignID"], organName + "-" + name);//oAssTo.Add(toUserData["SignID"], name);

                    string strStartTime = (dt.Rows[i]["OTStartTime"]).ToString().Substring(0, 2) + "：" + (dt.Rows[i]["OTStartTime"]).ToString().Substring(2, 2);
                    string strEndTime = (dt.Rows[i]["OTEndTime"]).ToString().Substring(0, 2) + "：" + (dt.Rows[i]["OTEndTime"]).ToString().Substring(2, 2);
                    string OTSeq = at.QueryColumn("OTSeq", " OverTimeAdvance", " AND OTCompID='" + dt.Rows[i]["OTCompID"].ToString() + "' AND OTEmpID='" + dt.Rows[i]["EmpID"] + "' AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                    string FlowKeyValue = "A," + dt.Rows[i]["OTCompID"] + "," + dt.Rows[i]["EmpID"] + "," + dt.Rows[i]["OTStartDate"] + "," + dt.Rows[i]["OTEndDate"] + "," + int.Parse(OTSeq).ToString("00");
                    string strName = at.QueryColumn("NameN", eHRMSDB + ".[dbo].[Personal]", " AND CompID='" + dt.Rows[i]["OTCompID"] + "' AND EmpID='" + dt.Rows[i]["EmpID"] + "' ");
                    string FlowShowValue = dt.Rows[i]["EmpID"] + "," + strName + "," + dt.Rows[i]["OTStartDate"] + "," + strStartTime + "," + dt.Rows[i]["OTEndDate"] + "," + strEndTime;

                    #endregion"流程所需資料前置"

                    //永豐流程送簽
                    if (FlowExpress.IsFlowInsVerify(flow.FlowID, FlowKeyValue.Split(','), FlowShowValue.Split(','), nextIsLastFlow ? "btnBeforeLast" : "btnBefore", oAssTo, ""))
                    {
                        DbConnection cn = db.OpenConnection();
                        DbTransaction tx = cn.BeginTransaction();
                        sb.Reset();
                        string strFlowCaseID = FlowExpress.getFlowCaseID(flow.FlowID, FlowKeyValue);
                        //更新AssignToName(部門+員工姓名)

                        #region"組SQL字串sb:回壓狀態"
                        if (dt.Rows[i]["OTStartDate"].ToString() == dt.Rows[i]["OTEndDate"].ToString())
                        {
                            sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='2',");
                            sb.Append(" OTValidDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                            sb.Append(" OTValidID='" + UserInfo.getUserInfo().UserID + "'");
                            sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                            sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                            sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'");
                            sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "'");
                            sb.Append(" AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "'");
                            sb.Append(" AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "'");
                            sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                        }
                        else
                        {
                            string crossDayArray = dt.Rows[i]["OTStartDate"] + "," + dt.Rows[i]["OTEndDate"];
                            for (int j = 0; j < crossDayArray.Split(',').Length; j++)
                            {
                                if (crossDayArray.Split(',')[j] == dt.Rows[i]["OTStartDate"].ToString())
                                {
                                    sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='2',");
                                    sb.Append(" OTValidDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                                    sb.Append(" OTValidID='" + UserInfo.getUserInfo().UserID + "'");
                                    sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                                    sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                                    sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'");
                                    sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTStartDate"] + "'");
                                    sb.Append(" AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "'");
                                    sb.Append(" AND OTEndTime='2359'");
                                    sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                                }
                                else
                                {
                                    sb.AppendStatement("UPDATE OverTimeAdvance SET OTStatus='2',");
                                    sb.Append(" OTValidDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                                    sb.Append(" OTValidID='" + UserInfo.getUserInfo().UserID + "'");
                                    sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                                    sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                                    sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTEndDate"] + "'");
                                    sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "'");
                                    sb.Append(" AND OTStartTime='0000'");
                                    sb.Append(" AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "'");
                                    sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                                }
                            }
                        }
                        #endregion"組SQL字串sb:回壓狀態"

                        #region"組SQL字串sb:跨日單回壓FlowCaseID"
                        if (dt.Rows[i]["OTStartDate"].ToString() != dt.Rows[i]["OTEndDate"].ToString())//當跨日的時候須回寫FlowCaseID到迄日
                        {
                            sb.AppendStatement("UPDATE OverTimeAdvance SET FlowCaseID=A.FlowCaseID");
                            sb.Append(" FROM (SELECT FlowCaseID FROM OverTimeAdvance");
                            sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                            sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                            sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTStartDate"] + "'");
                            sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTStartDate"] + "'");
                            sb.Append(" AND OTStartTime='" + dt.Rows[i]["OTStartTime"] + "'");
                            sb.Append(" AND OTEndTime='2359'");
                            sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                            sb.Append(" ) A ");
                            sb.Append(" WHERE OTCompID='" + dt.Rows[i]["OTCompID"] + "'");
                            sb.Append(" AND OTEmpID='" + dt.Rows[i]["EmpID"] + "'");
                            sb.Append(" AND OTStartDate='" + dt.Rows[i]["OTEndDate"] + "'");
                            sb.Append(" AND OTEndDate='" + dt.Rows[i]["OTEndDate"] + "'");
                            sb.Append(" AND OTStartTime='0000'");
                            sb.Append(" AND OTEndTime='" + dt.Rows[i]["OTEndTime"] + "'");
                            sb.Append(" AND OTTxnID='" + dt.Rows[i]["OTTxnID"] + "'");
                        }
                        #endregion"組SQL字串sb:跨日單回壓FlowCaseID"

                        #region"加進HROverTimeLog"
                        FlowUtility.InsertHROverTimeLogCommand(strFlowCaseID, "1", strFlowCaseID + ".00001",
                               "A", empData["EmpID"], empData["OrganID"], empData["FlowOrganID"], UserInfo.getUserInfo().UserID,
                               flowCode, flowSN, "1", toUserData["SignLine"],
                               toUserData["SignIDComp"], toUserData["SignID"], toUserData["SignOrganID"], toUserData["SignFlowOrganID"], "0",
                               false, ref sb, 1, "1");
                        #endregion"加進HROverTimeLog"

                        #region"執行SQL字串sb"
                        if (DoIt)
                        try
                        {
                            /************************************************/
                            /*測試用test                                                                  */
                            //sb.Append("test我是來製作錯誤的!!");                    
                            /************************************************/
                            db.ExecuteNonQuery(sb.BuildCommand(), tx);
                            tx.Commit();
                            DoIt = true;
                            //RefreshGrid();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteSysLog(ex); //將 Exception 丟給 Log 模組
                            tx.Rollback();//資料更新失敗
                            DoIt = false;
                            FlowExpress.IsFlowCaseDeleted(flow.FlowID, strFlowCaseID, true); //永豐流程刪除
                            //IndexMsg = IndexMsg + dt.Rows[i]["RowIndex"].ToString() + ","; //失敗資訊
                            IndexMsg = IndexMsg + "第" + dt.Rows[i]["RowIndex"].ToString() + "項-送簽失敗" + "\n";
                        }
                        finally
                        {
                            cn.Close();
                            cn.Dispose();
                            tx.Dispose();
                        }
                        #endregion"執行sb"

                        #region"Mail 組SQL字串sb1+執行"
                        if (!string.IsNullOrEmpty(strFlowCaseID) && DoIt)
                        {
                            CommandHelper sb1 = db.CreateCommandHelper();
                            //加進HROverTimeLog

                            string Subject_1 = "";
                            string Content_1 = "";
                            string mail = "";
                            CommandHelper sbselect = db.CreateCommandHelper();
                            sbselect.Append("SELECT isnull(C.EMail,'') AS EMail FROM " + eHRMSDB + ".[dbo].[Personal] P");
                            sbselect.Append(" LEFT JOIN " + eHRMSDB + ".[dbo].[Communication] C ON P.IDNo=C.IDNo");
                            sbselect.Append(" WHERE P.CompID='" + toUserData["SignIDComp"] + "' AND P.EmpID='" + toUserData["SignID"] + "'");
                            DataTable dtSelect = db.ExecuteDataSet(sbselect.BuildCommand()).Tables[0];
                            if (dtSelect.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dtSelect.Rows[0]["EMail"].ToString()))
                                {
                                    mail = HREmail();
                                    Subject_1 = "系統查無通知者E-mail";
                                    Content_1 = "OverTimeExpedite||BM@QuitMailContent1||系統查無通知者<br/>" + toUserData["SignID"] + "-" + name;
                                }
                                else
                                {
                                    mail = dtSelect.Rows[0]["EMail"].ToString();
                                    Subject_1 = "加班單流程待辦案件通知";
                                    Content_1 = "OverTimeTodoList||BM@QuitMailContent1||" + name + "||BM@QuitMailContent2||" + dt.Rows[i]["OTFormNO"] + "||BM@QuitMailContent3||" + FlowKeyValue.Split(',')[2] + "||BM@QuitMailContent4||" + strName + "||BM@QuitMailContent5||" + FlowShowValue.Split(',')[2] + "~" + FlowShowValue.Split(',')[4] + "||BM@QuitMailContent6||" + FlowShowValue.Split(',')[3] + "||BM@QuitMailContent7||" + FlowShowValue.Split(',')[5] + "||BM@QuitMailContent8||" + dt.Rows[i]["OTTotalTime"];
                                }
                            }
                            //加進MailLog
                            Aattendant.InsertMailLogCommand("人力資源處", toUserData["SignIDComp"], toUserData["SignID"], mail, "", "", Subject_1, Content_1, false, ref sb1);
                            try
                            {
                                db.ExecuteNonQuery(sb1.BuildCommand());
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteSysLog(ex); //將 Exception 丟給 Log 模組
                                //tx.Rollback();//資料更新失敗
                                //Util.MsgBox("送簽失敗(6)"); //提案送審失敗
                                //IndexMsg = IndexMsg + dt.Rows[i]["RowIndex"].ToString() + ",";
                            }
                        #endregion"Mail組SQL字串sb1+執行"
                        }
                    }
                    else
                    {
                        IndexMsg = IndexMsg + "第" + dt.Rows[i]["RowIndex"].ToString() + "項-進流程失敗" + "\n";
                        DoIt = false;
                    }
                }//for
                #region"錯誤訊息視窗+失敗刪除FlowCaseID+gridview查詢"
                //訊息視窗
                if (IndexMsg.Length > 0)
                {
                    sb.Reset();
                    sb.AppendStatement("UPDATE OverTimeAdvance SET FlowCaseID='' ");
                    sb.Append(" WHERE FlowCaseID <>'' ");
                    sb.Append(" AND OTStatus='1' ");
                    try { db.ExecuteNonQuery(sb.BuildCommand()); }
                    catch (Exception) { }
                    Util.MsgBox(IndexMsg + "共" + (IndexMsg.Split("\n").Length - 1) + "筆-送簽失敗");//提案送審失敗
                }
                else
                {
                    Util.MsgBox("送簽成功");
                }
                try { RefreshGrid(); }
                catch (Exception) { }//重新查詢
                #endregion"錯誤訊息視窗+失敗刪除FlowCaseID+gridview查詢"
            }//if count>0
        }
    }
    protected string HREmail()
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        string mail = "";
        sb.Append("SELECT SC.UserID,C.EMail FROM " + Util.getAppSetting("app://HRDB_OverTime/") + ".[dbo].[SC_UserGroup] SC");
        sb.Append(" LEFT JOIN " + Util.getAppSetting("app://eHRMSDB_OverTime/") + ".[dbo].[Personal] P ON P.EmpID=SC.UserID AND P.CompID=SC.CompID");
        sb.Append(" LEFT JOIN " + Util.getAppSetting("app://eHRMSDB_OverTime/") + ".[dbo].[Communication] C ON P.IDNo=C.IDNo");
        sb.Append(" WHERE SC.CompID='" + UserInfo.getUserInfo().CompID + "' AND SC.GroupID='ADM-OT'");
        DataTable dt = db.ExecuteDataSet(sb.BuildCommand()).Tables[0];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            mail += dt.Rows[i]["EMail"].ToString() + ";";
        }
        return mail;
    }

    public void RefreshGrid()
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        sb.Append(" SELECT * FROM (");
        sb.Append(" SELECT OT.OTEmpID,OT.OTCompID,OT.DeptID,OT.OrganID,OT.OTFormNO,P.NameN,OT.OTRegisterID,OT.OTTxnID,OTT.CodeCName,ISNULL(AI.FileName,'') AS FileName,OT.OTStatus,OT.FlowCaseID,");
        sb.Append(" Case OT.OTStatus WHEN '1' THEN '暫存' WHEN '2' THEN '送簽' WHEN '3' THEN '核准' WHEN '4' THEN '駁回' WHEN '5' THEN '刪除' WHEN '9' THEN '取消' END AS OTStatusName,");
        sb.Append(" (OT.OTStartDate+'~'+isnull(OT2.OTEndDate,OT.OTEndDate)) AS OTDate,");
        sb.Append(" (Left(OT.OTStartTime,2)+':'+Right(OT.OTStartTime,2)+'~'+ isnull(Left(OT2.OTEndTime,2)+':'+Right(OT2.OTEndTime,2),Left(OT.OTEndTime,2)+':'+Right(OT.OTEndTime,2))) AS OTTime,");
        sb.Append(" Convert(Decimal(10,1),Round(Convert(Decimal(10,2),((CAST(OT.OTTotalTime AS FLOAT)-CAST(OT.MealTime AS FLOAT))/CAST(60 AS FLOAT))+((CAST(ISNULL(OT2.OTTotalTime,0) AS FLOAT)-CAST(ISNULL(OT2.MealTime,0) AS FLOAT))/CAST(60 AS FLOAT))),1),1) AS OTTotalTime");
        //sb.Append(" Convert(Decimal(10,1),((CAST(OT.OTTotalTime AS FLOAT)-CAST(OT.MealTime AS FLOAT))/CAST(60 AS FLOAT))+((CAST(ISNULL(OT2.OTTotalTime,0) AS FLOAT)-CAST(ISNULL(OT2.MealTime,0) AS FLOAT))/CAST(60 AS FLOAT))) AS OTTotalTime");
        sb.Append(" FROM OverTimeAdvance OT ");
        sb.Append(" LEFT JOIN OverTimeAdvance OT2 on OT2.OTTxnID=OT.OTTxnID AND OT2.OTSeqNo=2");
        sb.Append(" LEFT JOIN " + Util.getAppSetting("app://eHRMSDB_OverTime/") + ".[dbo].[Personal] P ON P.EmpID=OT.OTEmpID AND P.CompID = OT.OTCompID ");
        sb.Append(" LEFT JOIN AT_CodeMap AS OTT ON OT.OTTypeID = OTT.Code AND OTT.TabName='OverTime' AND OTT.FldName='OverTimeType'");
        sb.Append(" LEFT JOIN AttachInfo AI ON AI.AttachID IS NOT NULL AND AI.AttachID <> '' AND AI.AttachID = OT.OTAttachment AND AI.FileSize>0 WHERE OT.OTSeqNo=1) A");
        sb.Append(" WHERE 1=1 AND ((A.OTCompID = '" + UserInfo.getUserInfo().CompID + "'");  //20170207 leo modify 特殊人員
        /*20170207 leo modify 特殊人員DeptID 條件where1=1 and (("原始")OR("追加"))
         *原始 sb.Append(" WHERE 1=1 AND A.OTCompID = '" + UserInfo.getUserInfo().CompID + "'");
         *修改增加一個"("，配合後面條件 sb.Append(" WHERE 1=1 AND ((A.OTCompID = '" + UserInfo.getUserInfo().CompID + "'");
         * 
         */
        //查詢條件
        if (!string.IsNullOrEmpty(txtOTEmpID.Text)) // 20170209 leo modify 特殊人員登入後，允許空白查詢有權限的部門的所有員工
        {
            if (ddlQueryConditions.SelectedValue == "0") //填單人員(OTRegisterID)或加班人員(OTEmpID)
            {
                if (ddlOrgAndFlow.SelectedValue == "1" || ddlOrgAndFlow.SelectedValue == "2" || ddlOrgAndFlow.SelectedValue == "3")
                {
                    sb.Append(" AND ( A.OTEmpID='" + txtOTEmpID.Text + "' OR  A.OTRegisterID='" + txtOTEmpID.Text + "')");
                }
                else
                {
                    if (UserInfo.getUserInfo().UserID == txtOTEmpID.Text)
                    {
                        sb.Append(" AND (A.OTRegisterID='" + UserInfo.getUserInfo().UserID + "' OR A.OTEmpID='" + UserInfo.getUserInfo().UserID + "')");
                    }
                    else
                    {
                        sb.Append(" AND( ((A.OTEmpID='" + txtOTEmpID.Text + "' AND A.OTRegisterID='" + UserInfo.getUserInfo().UserID + "')");
                        sb.Append(" OR (A.OTEmpID='" + UserInfo.getUserInfo().UserID + "' AND A.OTRegisterID='" + txtOTEmpID.Text + "') ))");
                    }
                }
            }
            else if (ddlQueryConditions.SelectedValue == "1") //填單人員
            {
                if (ddlOrgAndFlow.SelectedValue == "1" || ddlOrgAndFlow.SelectedValue == "2" || ddlOrgAndFlow.SelectedValue == "3")
                {
                    sb.Append(" AND (A.OTRegisterID='" + txtOTEmpID.Text + "')");
                }
                else
                {
                    if (UserInfo.getUserInfo().UserID == txtOTEmpID.Text)
                    {
                        sb.Append(" AND A.OTRegisterID='" + UserInfo.getUserInfo().UserID + "'");
                    }
                    else
                    {
                        sb.Append(" AND A.OTRegisterID = '" + txtOTEmpID.Text + "' AND A.OTEmpID='" + UserInfo.getUserInfo().UserID + "'");
                    }
                }
            }
            else if (ddlQueryConditions.SelectedValue == "2") //加班人員
            {
                if (ddlOrgAndFlow.SelectedValue == "1" || ddlOrgAndFlow.SelectedValue == "2" || ddlOrgAndFlow.SelectedValue == "3")
                {
                    sb.Append(" AND (A.OTEmpID='" + txtOTEmpID.Text + "')");
                }
                else
                {
                    if (UserInfo.getUserInfo().UserID == txtOTEmpID.Text)
                    {
                        sb.Append(" AND A.OTEmpID='" + UserInfo.getUserInfo().UserID + "'");
                    }
                    else
                    {
                        sb.Append(" AND A.OTEmpID= '" + txtOTEmpID.Text + "' AND A.OTRegisterID='" + UserInfo.getUserInfo().UserID + "' ");//
                    }
                }
            }
        }
        //沒有特殊人員與不使用時，員編不必輸，若員編沒輸入時，則帶出填單人員或加班人員為登入者的資料
        if (string.IsNullOrEmpty(txtOTEmpID.Text) && ddlQueryConditions.SelectedValue == "0" && ddlOrgAndFlow.SelectedValue == "")
        {
            sb.Append(" AND (A.OTRegisterID='" + UserInfo.getUserInfo().UserID + "' OR A.OTEmpID='" + UserInfo.getUserInfo().UserID + "')");
        }

        //員工姓名
        if (!string.IsNullOrEmpty(txtOTEmpName.Text))
        {
            sb.Append(" AND A.NameN LIKE '%" + txtOTEmpName.Text + "%' ");
        }
        //表單編號
        if (!string.IsNullOrEmpty(txtOTFormNO.Text))
        {
            sb.Append(" AND A.OTFormNO = '" + txtOTFormNO.Text + "' ");
        }
        //狀態(請選擇為全部查詢，)
        if (ddlOTStatus.SelectedValue != "0")
        {
            sb.Append(" AND A.OTStatus = '" + ddlOTStatus.SelectedValue + "' ");
        }
        //加班日期
        if (!string.IsNullOrEmpty(txtOTDateBegin.ucSelectedDate) && !string.IsNullOrEmpty(txtOTDateEnd.ucSelectedDate))
        {
            sb.Append(" AND ( ");
            sb.Append("(CONVERT(DATETIME,LEFT(A.OTDate,10),111) >= '" + txtOTDateBegin.ucSelectedDate + "' ");
            sb.Append(" AND CONVERT(DATETIME,LEFT(A.OTDate,10),111) <= '" + txtOTDateEnd.ucSelectedDate + "') ");
            sb.Append(" ) ");
        }
        else if (!string.IsNullOrEmpty(txtOTDateBegin.ucSelectedDate) && string.IsNullOrEmpty(txtOTDateEnd.ucSelectedDate))//只有起日
        {
            sb.Append(" AND CONVERT(DATETIME,LEFT(A.OTDate,10),111) >= '" + txtOTDateBegin.ucSelectedDate + "' ");
        }
        else if (string.IsNullOrEmpty(txtOTDateBegin.ucSelectedDate) && !string.IsNullOrEmpty(txtOTDateEnd.ucSelectedDate)) //只有迄日
        {
            sb.Append(" AND CONVERT(DATETIME,LEFT(A.OTDate,10),111) <= '" + txtOTDateEnd.ucSelectedDate + "' ");
        }
        //↓↓↓↓↓↓↓20170207 leo modify 特殊人員DeptID查詢↓↓↓↓↓↓↓
        sb.Append(" )");  //B
        //if (trSPUser.Visible)
        if (trSPUser.Visible && ddlOrgAndFlow.SelectedValue != "")
        {
            sb.Append("and ( 1!=1");  //A
            if (ddlOrgAndFlow.SelectedValue != "")
            {
                if (ViewState["DeptID"].ToString() != "" && ddlOrgAndFlow.SelectedValue != "2")
                    sb.Append(" OR (A.OrganID in(" + ViewState["DeptID"] + ") and A.OrganID in (" + ViewState["OrgList"].ToString() + "))");
                if (ViewState["DeptIDFlow"].ToString() != "" && ddlOrgAndFlow.SelectedValue != "1")
                    sb.Append(" OR (A.OrganID in(" + ViewState["OrgFlowListOrganID"] + ") and A.OrganID in (" + ViewState["OrgFlowListOrganID"] + "))");
            }
            sb.Append(")");   //A
        }
        else
        {
            sb.Append("and A.OrganID='" + UserInfo.getUserInfo().OrganID + "'");
        }

        sb.Append(")");  //B

        //↑↑↑↑↑↑↑20170207 leo modify 特殊人員DeptID查詢↑↑↑↑↑↑↑
        sb.Append(" ORDER BY Convert(datetime,Left(A.OTDate,10)) desc,Left(A.OTTime,4) desc,A.OTEmpID asc,A.OTTxnID");
        DataTable dt = db.ExecuteDataSet(sb.BuildCommand()).Tables[0];

        gvMain.DataSource = dt;
        gvMain.DataBind();
        gvMain.Visible = true;
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[4].Text == "送簽")
            {
                e.Row.BackColor = System.Drawing.Color.Yellow;
            }
            else if (e.Row.Cells[4].Text == "暫存" || e.Row.Cells[4].Text == "核准")
            {
                e.Row.BackColor = System.Drawing.Color.White;
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;
                CheckBox chk = (CheckBox)e.Row.FindControl("chkChoose");
                chk.Enabled = false;
            }
            e.Row.Cells[3].Style.Add("word-break", "break-all");
        }
    }
    public void LoadCheckData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EmpID");
        dt.Columns.Add("OTStartDate");
        dt.Columns.Add("OTEndDate");
        dt.Columns.Add("OTStartTime");
        dt.Columns.Add("OTEndTime");
        dt.Columns.Add("OTCompID");
        dt.Columns.Add("OTFormNO");
        dt.Columns.Add("OTStatusName");
        dt.Columns.Add("OTTotalTime");
        dt.Columns.Add("OTTxnID");
        dt.Columns.Add("RowIndex");
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk_chkOverTime = (CheckBox)row.Cells[0].FindControl("chkChoose");
                if (chk_chkOverTime.Checked)
                {
                    DataRow dr = dt.NewRow();
                    dr["EmpID"] = gvMain.DataKeys[row.RowIndex].Values["OTEmpID"].ToString();
                    dr["OTStartDate"] = gvMain.DataKeys[row.RowIndex].Values["OTDate"].ToString().Split('~')[0];
                    dr["OTEndDate"] = gvMain.DataKeys[row.RowIndex].Values["OTDate"].ToString().Split('~')[1];
                    dr["OTStartTime"] = (gvMain.DataKeys[row.RowIndex].Values["OTTime"].ToString().Split('~')[0]).Replace(":", "");
                    dr["OTEndTime"] = (gvMain.DataKeys[row.RowIndex].Values["OTTime"].ToString().Split('~')[1]).Replace(":", "");
                    dr["OTTotalTime"] = gvMain.DataKeys[row.RowIndex].Values["OTTotalTime"].ToString();
                    dr["OTTxnID"] = gvMain.DataKeys[row.RowIndex].Values["OTTxnID"].ToString();
                    dr["OTCompID"] = gvMain.DataKeys[row.RowIndex].Values["OTCompID"].ToString();
                    dr["OTFormNO"] = gvMain.DataKeys[row.RowIndex].Values["OTFormNO"].ToString();
                    dr["OTStatusName"] = gvMain.DataKeys[row.RowIndex].Values["OTStatus"].ToString();
                    dr["RowIndex"] = row.RowIndex + 1;
                    dt.Rows.Add(dr);
                }
                ViewState["dt"] = dt;
            }
        }
    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        FlowExpress flow = new FlowExpress(Util.getAppSetting("app://AattendantDB_OverTime/"));
        switch (e.CommandName)
        {
            case "Detail":
                ucModalPopup1.Reset();
                ucModalPopup1.ucPopupWidth = (ScreenWidth.Value == "" ? 1020 : (int.Parse(ScreenWidth.Value) > 1020 ? 1020 : int.Parse(ScreenWidth.Value) - 100));//ucModalPopup.ucPopupWidth = 1500;
                ucModalPopup1.ucPopupHeight = (ScreenHeight.Value == "" ? 625 : (int.Parse(ScreenHeight.Value) > 625 ? 625 : int.Parse(ScreenHeight.Value) - 100));//ucModalPopup.ucPopupHeight = 750;
                ucModalPopup1.ucPopupHeader = "";
                ucModalPopup1.ucFrameURL = "OvertimePreOrder_Detail.aspx?OTCompID=" + gvMain.DataKeys[clickedRow.RowIndex].Values["OTCompID"].ToString() + "&EmpID=" + gvMain.Rows[clickedRow.RowIndex].Cells[5].Text + "&OTStartDate=" + (gvMain.Rows[clickedRow.RowIndex].Cells[8].Text).Split('~')[0] + "&OTEndDate=" + (gvMain.Rows[clickedRow.RowIndex].Cells[8].Text).Split('~')[1] + "&OTStartTime=" + gvMain.Rows[clickedRow.RowIndex].Cells[9].Text.Split('~')[0].Replace(":", "") + "&OTEndTime=" + gvMain.Rows[clickedRow.RowIndex].Cells[9].Text.Split('~')[1].Replace(":", "") + "&FlowID=" + flow.FlowID + "&FlowCaseID=" + gvMain.DataKeys[clickedRow.RowIndex].Values["FlowCaseID"].ToString() + "&OTTxnID=" + gvMain.DataKeys[clickedRow.RowIndex].Values["OTTxnID"].ToString();
                ucModalPopup1.Show();
                break;
        }
    }

    //20170207 leo modify (增加特殊人員登入後的處/部/科下拉)
    private void ColumnAdd(DataTable dt, string Name, string DataType)
    {
        DataColumn NewColumn;
        NewColumn = new DataColumn();
        NewColumn.DataType = Type.GetType(DataType);
        NewColumn.ColumnName = Name;
        dt.Columns.Add(NewColumn);
    }
    //下拉function 非共用
    private void FillDDL(string str, DropDownList DDL) //行政組織
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        DataTable dt = db.ExecuteDataSet(str).Tables[0];
        FillDDL(dt, DDL, "Code", "CodeName");
    }
    private void FillDDL(DataTable dt, DropDownList DDL, string DataValueField, string DataTextField) //功能組織
    {
        DDL.Items.Clear();
        DDL.DataSource = dt.DefaultView.ToTable(true, DataValueField, DataTextField);
        DDL.DataValueField = DataValueField;
        DDL.DataTextField = DataTextField;
        DDL.DataBind();
        DDL.Items.Insert(0, new ListItem("----請選擇----", ""));
    }
    private void save_DeptID(string str, string Type) //行政組織
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        DataTable dt = db.ExecuteDataSet(str).Tables[0];
        save_DeptID(dt, Type, "Code", "CodeName");
    }
    private void save_DeptID(DataTable dt, string Type, string DataValueField, string DataTextField) //功能組織
    {
        if (Type == "DeptID")
        {
            if (dt.Rows.Count > 0)
            {
                DataTable dt1 = dt.DefaultView.ToTable(true, DataValueField, DataTextField).Select(DataValueField + "<>''").CopyToDataTable();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    ViewState["DeptID"] = "'" + dt1.Rows[i][DataValueField].ToString() + "'," + ViewState["DeptID"];
                }
            }
        }
        else if (Type == "DeptIDFlow")
        {
            if (dt.Rows.Count > 0)
            {
                DataTable dt1 = NoWhite(dt.DefaultView.ToTable(true, DataValueField, DataTextField), DataValueField);
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    ViewState["DeptIDFlow"] = "'" + dt1.Rows[i][DataValueField].ToString() + "'," + ViewState["DeptIDFlow"];
                }
            }
        }
    }
    private DataTable NoWhite(DataTable dt, string self) //去除空白資料and檢查是否為空Table
    {
        DataTable dClear = dt.Copy();
        dClear.Clear();
        dt = dt.Select(self + "<>''").Length > 0 ? dt.Select(self + "<>''").CopyToDataTable() : dClear;
        return dt;
    }
    private DataTable SelectRoleCode(DataTable dt, string ddlRoleCodeSelect, string DDL, string View)
    {
        if (ddlRoleCodeSelect == "")
        {
            return NoWhite(dt, "O" + View + "OrganID");
        }
        else
        {
            if (dt.Select("O" + View + "UpOrganID = '" + ddlRoleCodeSelect + "'").Length > 0)
                return dt.Select("O" + View + "UpOrganID = '" + ddlRoleCodeSelect + "'").CopyToDataTable();
            else
                dt.Clear();
        }
        return dt;
    }
    //功能組織下拉
    private void ddlGetFlow(int RoleCode)
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        DataTable dt = db.ExecuteDataSet(ViewState["OrgFlowList"].ToString()).Tables[0];
        DataTable dClear = dt.Copy();
        dClear.Clear();
        ViewState["DeptIDFlow"] = "";
        string self = "";
        switch (RoleCode)
        {
            case 40: //PageLoad
                FillDDL(NoWhite(dt, "O40OrganID"), ddlRoleCode40, "O40OrganID", "DownOrganName");
                save_DeptID(NoWhite(dt, "O40OrganID"), "DeptIDFlow", "O40OrganID", "DownOrganName");
                goto case 30;
            case 30: //ddlRoleCode40 selectchange
                FillDDL(SelectRoleCode(dt, ddlRoleCode40.SelectedValue, "40", "30"), ddlRoleCode30, "O30OrganID", "DownOrganName");
                save_DeptID(NoWhite(dt, "O30OrganID"), "DeptIDFlow", "O30OrganID", "DownOrganName");
                goto case 20;
            case 20: //ddlRoleCode30 selectchange
                FillDDL(SelectRoleCode(dt, ddlRoleCode30.SelectedValue, "30", "20"), ddlRoleCode20, "O20OrganID", "DownOrganName");
                save_DeptID(NoWhite(dt, "O20OrganID"), "DeptIDFlow", "O20OrganID", "DownOrganName");
                goto case 10;
            case 10:
                dt = SelectRoleCode(dt, ddlRoleCode20.SelectedValue, "20", "10");
                FillDDL(dt, ddlRoleCode10, "DownOrganID", "DownOrganName");
                if (ddlRoleCode10.SelectedItem.Text.ToString().Left(1) != "└" && ddlRoleCode10.SelectedItem.Text.ToString().Left(1) != "")
                {
                    save_DeptID(NoWhite(dt, "O0OrganID"), "DeptIDFlow", "DownOrganID", "DownOrganName");
                }
                break;
            case 0:
                if (ddlRoleCode10.SelectedItem.Text.ToString().Left(1) != "└")
                {
                    dt = NoWhite(dt.Select("O10OrganID = '" + ddlRoleCode10.SelectedValue + "'").CopyToDataTable(), "O0OrganID");
                    save_DeptID(NoWhite(dt, "O0OrganID"), "DeptIDFlow", "O0OrganID", "DownOrganName");
                }
                else
                    ViewState["DeptIDFlow"] = "";
                break;
        }
        self = ddlRoleCode10.SelectedValue != "" ? ddlRoleCode10.SelectedValue : ddlRoleCode20.SelectedValue != "" ? ddlRoleCode20.SelectedValue : ddlRoleCode30.SelectedValue != "" ? ddlRoleCode30.SelectedValue : ddlRoleCode40.SelectedValue;

        if (self != "")
            ViewState["DeptIDFlow"] = ViewState["DeptIDFlow"].ToString() + "'" + self + "'";
        else
            ViewState["DeptIDFlow"] = ViewState["DeptIDFlow"].ToString().TrimEnd(',');
        string test = ViewState["DeptIDFlow"].ToString();
    }
    //行政組織下拉
    private void ddlGet(string Type)
    {
        DbHelper db = new DbHelper(_overtimeDBName);
        CommandHelper sb = db.CreateCommandHelper();
        ViewState["DeptID"] = "";
        string self = "";
        switch (Type)
        {
            case "OrgType":
                sb.Append("select DISTINCT O1.OrgType as Code,O1.OrgType+'-'+O2.OrganName as CodeName from " + _eHRMSDB + ".[dbo].[Organization] O1 ");
                sb.Append("left join " + _eHRMSDB + ".[dbo].[Organization] O2 on O1.OrgType=O2.OrganID ");
                sb.Append("where O1.OrganID in(" + ViewState["OrgList"].ToString() + ") ");
                sb.Append("and O1.CompID='" + ViewState["CompID"] + "' ");
                FillDDL(sb.ToString(), ddlOrgType);
                save_DeptID(sb.ToString(), "DeptID");
                sb.Reset();
                goto case "DeptID";
            case "DeptID":
                //self = ddlOrgType.SelectedValue;
                sb.Append("select DISTINCT O1.DeptID as Code,O1.DeptID+'-'+O2.OrganName as CodeName from " + _eHRMSDB + ".[dbo].[Organization] O1 ");
                sb.Append("left join " + _eHRMSDB + ".[dbo].[Organization] O2 on O1.DeptID=O2.OrganID ");
                sb.Append("where O1.OrganID in(" + ViewState["OrgList"].ToString() + ") ");
                sb.Append("and O1.CompID='" + ViewState["CompID"] + "' ");
                if (ddlOrgType.SelectedValue != "") sb.Append("and O1.OrgType='" + ddlOrgType.SelectedValue + "' ");
                FillDDL(sb.ToString(), ddlDeptID);
                save_DeptID(sb.ToString(), "DeptID");
                sb.Reset();
                goto case "OrganID";
            case "OrganID":
                //self = ddlDeptID.SelectedValue;
                sb.Append("Select  OrganID as Code,OrganID+'-'+OrganName as CodeName from " + _eHRMSDB + ".[dbo].[Organization] O1 where OrganID in(" + ViewState["OrgList"].ToString() + ")");
                sb.Append("and O1.CompID='" + ViewState["CompID"] + "' ");
                if (ddlOrgType.SelectedValue != "") sb.Append("and O1.OrgType='" + ddlOrgType.SelectedValue + "' ");
                if (ddlDeptID.SelectedValue != "") sb.Append("and O1.DeptID='" + ddlDeptID.SelectedValue + "' ");
                FillDDL(sb.ToString(), ddlOrganID);
                save_DeptID(sb.ToString(), "DeptID");
                sb.Reset();
                break;
            case "One":
                ViewState["DeptID"] = "";
                //self = ddlOrganID.SelectedValue;
                break;
        }
        self = ddlOrganID.SelectedValue != "" ? ddlOrganID.SelectedValue : ddlDeptID.SelectedValue != "" ? ddlDeptID.SelectedValue : ddlOrgType.SelectedValue;
        if (self != "")
            ViewState["DeptID"] = ViewState["DeptID"].ToString() + "'" + self + "'";
        else
            ViewState["DeptID"] = ViewState["DeptID"].ToString().TrimEnd(',');
        //string test = ViewState["DeptID"].ToString();
    }
    //行政組織下拉
    protected void ddlOrgType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrgType.SelectedValue == "")
            ddlGet("OrgType");
        else
            ddlGet("DeptID");
    }
    protected void ddlDeptID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDeptID.SelectedValue == "")
            ddlGet("DeptID");
        else
            ddlGet("OrganID");
    }
    protected void ddlOrganID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrganID.SelectedValue == "")
            ddlGet("OrganID");
        else
            ddlGet("One");
    }
    //功能組織下拉
    protected void ddlRoleCode40_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRoleCode40.SelectedValue == "")
            ddlGetFlow(40);
        else
            ddlGetFlow(30);
    }
    protected void ddlRoleCode30_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRoleCode30.SelectedValue == "")
            ddlGetFlow(30);
        else
            ddlGetFlow(20);
    }
    protected void ddlRoleCode20_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRoleCode20.SelectedValue == "")
            ddlGetFlow(20);
        else
            ddlGetFlow(10);
    }
    protected void ddlRoleCode10_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRoleCode10.SelectedValue == "")
            ddlGetFlow(10);
        else
            ddlGetFlow(0);
    }
    private void OrgAndFlowChanged(string Type)
    {
        switch (Type)
        {
            case "":
                ddlOrgType.Enabled = false;
                ddlDeptID.Enabled = false;
                ddlOrganID.Enabled = false;
                ddlRoleCode40.Enabled = false;
                ddlRoleCode30.Enabled = false;
                ddlRoleCode20.Enabled = false;
                ddlRoleCode10.Enabled = false;
                ddlGet("OrgType");
                ddlGetFlow(40);
                break;
            case "1":
                ddlOrgType.Enabled = true;
                ddlDeptID.Enabled = true;
                ddlOrganID.Enabled = true;
                ddlRoleCode40.Enabled = false;
                ddlRoleCode30.Enabled = false;
                ddlRoleCode20.Enabled = false;
                ddlRoleCode10.Enabled = false;
                ddlGetFlow(40);
                break;
            case "2":
                ddlOrgType.Enabled = false;
                ddlDeptID.Enabled = false;
                ddlOrganID.Enabled = false;
                ddlRoleCode40.Enabled = true;
                ddlRoleCode30.Enabled = true;
                ddlRoleCode20.Enabled = true;
                ddlRoleCode10.Enabled = true;
                ddlGet("OrgType");
                break;
            case "3":
                ddlOrgType.Enabled = true;
                ddlDeptID.Enabled = true;
                ddlOrganID.Enabled = true;
                ddlRoleCode40.Enabled = true;
                ddlRoleCode30.Enabled = true;
                ddlRoleCode20.Enabled = true;
                ddlRoleCode10.Enabled = true;
                break;
        }
    }
    protected void ddlOrgAndFlow_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrgAndFlowChanged(ddlOrgAndFlow.SelectedValue);
    }
    private string DeleteFromFlowCaseID(string FlowID, List<string> FlowCaseIDList)
    {
        string Msg = "";
        foreach (string FlowCaseID in FlowCaseIDList)
        {
            if (!FlowExpress.IsFlowCaseDeleted(FlowID, FlowCaseID, true))
            {
                Msg = Msg + FlowCaseID + ",";
            }
        }
        if (Msg.Length > 0)
            Msg.Substring(0, Msg.Length - 1);
        return Msg;
    }
}


