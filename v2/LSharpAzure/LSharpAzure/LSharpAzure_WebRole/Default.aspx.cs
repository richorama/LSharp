using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.ServiceHosting.ServiceRuntime;
using LSharp;
using System.IO;
using System.Text;

namespace LSharpAzure_WebRole
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["lsharp"] == null)
            {
                
                TextWriter writer = new StringWriter();
                TextWriter errorWriter = new StringWriter();

                Session["lsharp"] = new Runtime(null, writer, errorWriter);

                RoleManager.WriteToLog("Information", "New session Started");
            }

            if (Request.QueryString["q"] != null)
            {
                Runtime runtime = (Runtime)Session["lsharp"];
                string q = Request.QueryString["q"];
                object o = runtime.EvalStrings(q);

                Response.Write(Runtime.PrintToString(o));
            }
        }


        protected void EvalButton_Click(object sender, EventArgs e)
        {
            RoleManager.WriteToLog("Information", ">" + InputTextBox.Text);
            OutputTextBox.Text = ">" + InputTextBox.Text + "\n";

            Runtime runtime = (Runtime)Session["lsharp"];
            object o = null;

            try
            {
                o = runtime.EvalStrings(InputTextBox.Text);
            }
            catch (Exception ex)
            {
                RoleManager.WriteToLog("Information", ex.ToString());
                OutputTextBox.Text = ex + "\n";
            }

            

            OutputTextBox.Text += ((StringWriter)runtime.StdOut()).ToString();

            // Reset the output stream
            StringBuilder sb = ((StringWriter)runtime.StdOut()).GetStringBuilder();
            sb.Remove(0, sb.Length);

            if (o == null)
            {
                OutputTextBox.Text += "null";
                RoleManager.WriteToLog("Information", "null");
            }
            else
            {
                OutputTextBox.Text += Runtime.PrintToString(o);
                RoleManager.WriteToLog("Information", Runtime.PrintToString(o));
            }
            InputTextBox.Text = "";
            
        }
    }
}
