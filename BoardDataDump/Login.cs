using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;

namespace BoardDataDump
{
    public partial class Login : Form
    {
        public bool Cbexprod = false;
        public OdbcConnection CN = new OdbcConnection();
  
        public Login()
        {
            InitializeComponent();
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            loginBexprod();

            if (Cbexprod == true)
            {
                this.Hide();
                BoardInterface modshow = new BoardInterface();
                modshow.Show();
            }
            else if (Cbexprod == false)
            {
                MessageBox.Show("User Name or Password wrong");
            }
            CN.Close();  
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void loginBexprod()
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;
            try
            {
                
                String connectionString = "Dsn=bex400;uid=" + username + ";pwd=" + password + ";";
                CN = new OdbcConnection(connectionString);
                //CN.ConnectionString =
                //     "Dsn=bex00;" +
                //     "Uid=" + username + ";" +
                //     "Pwd=" + password + ";";
                CN.Open();
                //Bexprod();
                Cbexprod = true;
            }
            catch(Exception ex)
            {
                
                Cbexprod = false;

            }
            
        }


        //public void Bexprod()
        //{
        //    CN.ConnectionString =
        //                 "Dsn=BGD400;" +
        //                 "Uid=ics;" +
        //                 "Pwd=infinite;";
        //    CN.Open();
        //}
        //public void BGD()
        //{
        //    CNBGD.ConnectionString =
        //             "Dsn=BGD400;" +
        //             "Uid=ICS;" +
        //             "Pwd=INFINITE;";
        //    CNBGD.Open();
        //}
    

    }
}