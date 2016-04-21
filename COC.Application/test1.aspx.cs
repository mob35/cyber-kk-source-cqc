using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace COC.Application
{
    public partial class test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string appNo = Request["app_no"];
            string empCode = Request["emp_code"];
            string empTitle = Request["emp_title"];
            string privilegeNCB = Request["privilege_ncb"];
        }
    }
}