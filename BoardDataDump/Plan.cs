using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace BoardDataDump
{
    public partial class Plan : Form
    {
        private OleDbConnection OleCon;
   
        public Plan()
        {
            InitializeComponent();
            OleCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\BOARDTRF.mdb;");
        }

        //Plan Data Prepared

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OleCon.Open();
            }
            catch (OleDbException ole)
            {
                MessageBox.Show("Database NOT found.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Database NOT found.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            int b = 0;
            string inpMon = txtDate.Text;
            string sSql = "SELECT DISTINCT FREFER,FCDDOC, FCDCCE, FCLAS1, FCLAS2, FCDCUS FROM PLAN  WHERE (PLAN.FREFER<>'')";
            OleDbCommand command = new OleDbCommand(sSql, OleCon);
            OleDbDataReader rs = command.ExecuteReader();
            //string c = "";
            //if (rs1.GetString(0) != "")
            //{
            //   c = rs1.GetString(0);
            //}

            double UpToDtQty = 0;
            while (rs.Read())
            {
                this.Text = "PLAN DATA IS BEING PREPARED FOR" + rs.GetString(0);
                this.Text = string.Empty;
                //this.ShowInTaskbar = false;
                string c = "";
                b = b + 1;
                //c = rs1.GetValue(rs1.GetOrdinal("FREFER")).ToString();
                c = rs.GetString(0);
                    //.ToString();
                if (c != "")
                {
                    c = rs.GetString(0);

                    //if (rs1.GetString(0) != "")
                    //{
                    //    c = rs1.GetString(0);
                    //}
                    sSql = "SELECT FCDST1, FSSORT,FDTTRA,FREFER, FCDDOC, FCLAS1, FDESC1,FCLAS2,FDESC2,FCDCUS,FRASCL, Sum(FTRSQ1) AS FTRSQ1,FTRAUM,FCDCUR,FCDCCE From PLAN GROUP BY FCDST1, FSSORT, FDTTRA, FREFER, FCDDOC, FCLAS1, FDESC1, FCLAS2, FDESC2, FCDCUS, FRASCL,FTRAUM,FCDCUR,FCDCCE Having (FDESC2 <> '' AND FREFER = '" + rs.GetString(0) + "'  And FCDDOC = '" + rs.GetString(1) + "' and FCDCCE= '" + rs.GetString(2) + "' And FCLAS1 = '" + rs.GetString(3) + "' And FCLAS2 = '" + rs.GetString(4) + "' And FCDCUS = '" + rs.GetString(5) + "') ORDER BY FDTTRA,FCDST1, FSSORT ";
                    //sSql = "SELECT FCDST1, FSSORT,FDTTRA,FREFER, FCDDOC, FCLAS1, FDESC1,FCLAS2,FDESC2,FCDCUS,FRASCL, Sum(FTRSQ1) AS FTRSQ1, FTRAUM,FCDCUR,FCDCCE From PLAN GROUP BY FCDST1, FSSORT, FDTTRA, FREFER, FCDDOC, FCLAS1, FDESC1, FCLAS2, FDESC2, FCDCUS, FRASCL,FTRAUM,FCDCUR,FCDCCE Having (FREFER = '" + rs1.GetString(0) + "') ORDER BY FDTTRA,FCDST1, FSSORT ";
                    //string sSql2 = "SELECT FCDST1, FSSORT,FDTTRA,FREFER, FCDDOC, FCLAS1, FDESC1,FCLAS2,FDESC2,FCDCUS,FRASCL, Sum(FTRSQ1) AS FTRSQ1, FTRAUM,FCDCUR,FCDCCE From PLAN GROUP BY FCDST1, FSSORT, FDTTRA, FREFER, FCDDOC, FCLAS1, FDESC1, FCLAS2, FDESC2, FCDCUS, FRASCL,FTRAUM,FCDCUR,FCDCCE HAVING (FREFER<>'') ORDER BY FDTTRA,FCDST1, FSSORT ";
                    OleDbCommand cmdPlan = new OleDbCommand(sSql, OleCon);
                    OleDbDataReader rsPlan = cmdPlan.ExecuteReader();

                    //string d = "";
                    //d = rs2.GetValue(3).ToString();//rs2.GetOrdinal("FREFER"));
                    //Object d1 = rs2.GetValue(rs2.GetOrdinal("FREFER"));
                    //d=rs2.GetData(3).ToString();

                    //if (d != "")
                    //{
                    int i = 0;
                    while (rsPlan.Read())
                        {
                            double qtyFTRSQ1 = rsPlan.GetDouble(11);
                            //string c2 = (string)rsPlan.GetValue(9);
                            DateTime time = rsPlan.GetDateTime(2);             // Use current time
                            string fmtdate = "yyyyMMdd";
                            string PlanDate = time.ToString(fmtdate);
                            string fmtMon = "yyyyMM";            // Use this format
                            string PlanMon = time.ToString(fmtMon); // W
                            int PlanDateInt;
                            int.TryParse(PlanDate, out PlanDateInt);
                            //for (int i = 0; i < 17; i++ )
                            //{
                               // string c5 = rsPlan.GetString(i);
                            //}
                            if (inpMon == PlanMon)
                            {
                                i = i + 1;
                                UpToDtQty = UpToDtQty + rsPlan.GetDouble(11);

                                //string rsMnpln00f;
                                //sSql="SELECT * FROM MNPLN00F";
                                //OleDbCommand cmdMnpln00f = new OleDbCommand(sSql, OleCon);
                                //OleDbDataReader rsMnpln00f=cmdMnpln00f.ExecuteReader();
                                //rsMnpln00f
                                //rsMnpln00f.GetString(0) = rsPlan.GetString(0);
                                //string st = "  This is an example string. ";

                                // Call Trim instance method.
                                // This returns a new string copy.
                                //st = st.Trim();


                                sSql = "INSERT INTO MNPLN00F(FCDST1, FSSORT,FDTTRA,FREFER, FCDDOC, FCLAS1, FDESC1,FCLAS2,FDESC2,FCDCUS,FRASCL,FTRSQ1,FCTQTY,FTRAUM,FCDCUR,FCDCCE) Values('" + rsPlan.GetString(0) + "','" + rsPlan.GetString(1) + "'," + PlanDateInt + ",'" + rsPlan.GetString(3) + "','" + rsPlan.GetString(4) + "','" + rsPlan.GetString(5) + "','" + rsPlan.GetString(6) + "','" + rsPlan.GetString(7) + "','" + rsPlan.GetString(8) + "','" + rsPlan.GetString(9).Trim() + "','" + rsPlan.GetString(10).Replace("'","") + "'," + qtyFTRSQ1 + "," + UpToDtQty + ",'" + rsPlan.GetString(12) + "','" + rsPlan.GetString(13) + "','" + rsPlan.GetString(14) + "')";

                                OleDbCommand cmdInrt = new OleDbCommand(sSql, OleCon);
                                try
                                {
                                    cmdInrt.ExecuteNonQuery();
                                    label2.Text = "inserted into Mnpln00f";
                                }
                                catch (Exception es)
                                {
                                    MessageBox.Show(es.ToString());
                                }
                            }
                            else
                            {
                                MessageBox.Show(PlanDate + "--" + rsPlan.GetString(3) + "--" + rsPlan.GetString(9) + "--" + "Plan data is not complete"); 
                                //MessageBox.Show(rsPlan.GetString("FDTTRA") - +rsPlan.GetString("FREFER") - +rsPlan.GetString("FCDCUS"));
                            }
                            
                                
                            this.Text = i + "PLAN DATA IS BEING PREPARED FOR";
                            this.Text = string.Empty;
                        }

                   // }
                }
            }
            this.Text = "Plan Data Sucessfully Complete";
            OleCon.Close();
        }

        //Plan Data Upload

        private void button2_Click(object sender, EventArgs e)
        {
            //string a;
            String connectionString = "Dsn=BGD400;uid=ics;pwd=infinite;";
            OdbcConnection OdbcCon = new OdbcConnection(connectionString);
            string queryString,accessString,as400String;
            OdbcCommand cmdAs400;
            OleDbCommand cmdAcess;

            try
            {

                OdbcCon.Open();
            }
            catch (OdbcException odb)
            {
                MessageBox.Show("Database NOT found.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            string sDay = txtDate.Text + "01";
            int sDayInt;
            int.TryParse(sDay, out sDayInt);

            if (txtDate.Text.Length == 6)
            {

            }
            else
            {
                MessageBox.Show("!! Insert Period-  YYYYMM");
            }
            MessageBox.Show(sDay, "Critical Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            //DialogResult result1 = MessageBox.Show("Do You Want to Post Plan with This Period?", "The Question", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
            if (MessageBox.Show("Do You Want to Post Plan with This Period?", "The Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                //Form1.re
                //this.;
                //this.ResetMouseEventArgs();
            }
            //string sDay1 = sDay.Substring(0, 4) + "/" + sDay.Substring(4, 2) + "/" + sDay.Substring(6);
            //string sDay1 = sDay.Substring(6, 2) + "/" + sDay.Substring(4, 2) + "/" + sDay.Substring(0,4);
            //string sday2 = "01/01/2012";
            //DateTime a = Convert.ToDateTime(sDay1); 

            queryString = "Delete From P72IDAT.MNPLN00F where FDTTRA>='" + sDayInt + "' ";
            OdbcCommand command = new OdbcCommand(queryString, OdbcCon);
            command.ExecuteNonQuery();

            OleCon.Open();
            accessString = "SELECT * FROM MNPLN00F";
            cmdAcess = new OleDbCommand(accessString, OleCon);
            OleDbDataReader rs = cmdAcess.ExecuteReader();

            //as400String = "SELECT * FROM P72IDAT.MNPLN00F";
            //cmdAs400 = new OdbcCommand(as400String, OdbcCon);
            //OdbcDataReader rsAs400 = cmdAs400.ExecuteReader();

            //int j = 0;
            while (rs.Read())
            {
                this.Text=rs.GetValue(2)+"--"+rs.GetString(4)+"--"+rs.GetString(24);
                this.Text = string.Empty;
                //Console.WriteLine(rs.GetString(3));
                //a = rs.GetString(2);
                as400String = "Insert into P72IDAT.MNPLN00F(FANNUL,FUSRNM,FDTTRA,FREFER,FCDDOC,FCDDIV,FCDCCE,FCDSTO,FRECTY,FCDKE1,FCDKE2,FCDKE3,FCDKE4,FCDKE5,";
                as400String += " FDESCR,FSUPDS,FCDTRA,FCDVEN,FTRAUM,FTRQTY,FTRSQ1,FHDQTY,FCTQTY,FCDCUS,FRASCL,FCDORD,FLNNUM,FORCAU,";
                as400String += " FCDDIS,FCDDIL,FNRLOT,FCSSTD,FCSAPR,FCSADE,FCDCUR,FDTORA,FTECU1,FTECU2,FTECU3,FTECU4,FTECU5,FTECU6,";
                as400String += " FCOMM0,FSSORT,FCDST1,FCLAS1,FCLAS2,FDESC1,FDESC2)";
                as400String += " Values('" + rs.GetString(0) + "','" + rs.GetString(1) + "'," + rs.GetValue(2) + ",'" + rs.GetString(3) + "','" + rs.GetString(4) + "','" + rs.GetString(5) + "', '" + rs.GetString(6) + "','" + rs.GetString(7) + "','" + rs.GetString(8) + "','" + rs.GetString(9) + "','" + rs.GetString(10) + "',";
                as400String += " '" + rs.GetString(11) + "','" + rs.GetString(12) + "','" + rs.GetString(13) + "','" + rs.GetString(14) + "','" + rs.GetString(15) + "','" + rs.GetString(16) + "', '" + rs.GetString(17) + "','" + rs.GetString(18) + "'," + rs.GetValue(19) + "," + rs.GetValue(20) + "," + rs.GetValue(21) + ",";
                as400String += " " + rs.GetValue(22) + ",'" + rs.GetString(23) + "','" + rs.GetString(24).Replace("'", "") + "','" + rs.GetString(25) + "'," + rs.GetValue(26) + ",'" + rs.GetString(27) + "','" + rs.GetString(28) + "'," + rs.GetValue(29) + ",'" + rs.GetString(30) + "'," + rs.GetValue(31) + "," + rs.GetValue(32) + ", ";
                as400String += " " + rs.GetValue(33) + ",'" + rs.GetString(34) + "'," + rs.GetValue(35) + "," + rs.GetValue(36) + "," + rs.GetValue(37) + "," + rs.GetValue(38) + "," + rs.GetValue(39) + ",'" + rs.GetString(40) + "','" + rs.GetString(41) + "','" + rs.GetString(42) + "','" + rs.GetString(43) + "',";
                as400String += " '" + rs.GetString(44) + "','" + rs.GetString(45) + "','" + rs.GetString(46) + "','" + rs.GetString(47).Trim() + "','" + rs.GetString(48).Trim() + "')";
                
                cmdAs400 = new OdbcCommand(as400String, OdbcCon);
                try
                {
                    cmdAs400.ExecuteNonQuery();
                    label2.Text = "inserted into P72idat.mnpln00f success";
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.ToString());
                }

                //string test = rs.GetString(j);
                //j = j + 1;
            }
            MessageBox.Show("Insert Complete");
            this.Text = "Plan is Uploaded";
            OleCon.Close();
            OdbcCon.Close();
        }
    }
}