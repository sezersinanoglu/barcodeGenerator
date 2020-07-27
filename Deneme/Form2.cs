using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deneme
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private async void iTalk_Button_21_Click(object sender, EventArgs e)
        {
            Helper h = new Helper();
            String resultString = await h.login(login.Text.Trim());
            var result = JObject.Parse(resultString);
            if ((String)result["status"] == "success")
            {
                Form1 frm = new Form1();
                this.Hide();
                frm.Show();
            }
            else
            {
                MessageBox.Show((String)result["message"]);
            }
        }

        private void iTalk_Button_22_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
