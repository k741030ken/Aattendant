using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoPac.WebExpress.Common;

/// <summary>
/// ��ܨt�����Ҥ��ε{��
/// </summary>
public partial class Util_SysInfo : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPageInfo1.ucIsShowApplication = true;
        ucPageInfo1.ucIsShowEnvironmentInfo = true;
        ucPageInfo1.ucIsShowQueryString = false;
        ucPageInfo1.ucIsShowRequestForm = false;
        ucPageInfo1.ucIsShowSession = true;
    }
}