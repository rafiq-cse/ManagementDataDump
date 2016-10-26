using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using cwbx;




namespace BoardDataDump
{
    public partial class BoardInterface : Form
    {
        protected const string _newline = "\r\n";

        string SQL;
        OdbcCommand command = new OdbcCommand();
        OdbcCommand commandMap = new OdbcCommand();
        public OdbcConnection cnMap = new OdbcConnection();

        
        public BoardInterface()
        {
            InitializeComponent();
            
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            
            //this.Close();
            Application.Exit();

        }

        int op;

        private void rdbProduction_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbProduction.TabIndex;
            btnRun.Enabled = true;
        }
        
        private void rdbSales_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbSales.TabIndex;
            btnRun.Enabled = true;
        }
   
        
        private void rdbPurchase_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbPurchase.TabIndex;
            btnRun.Enabled = true;
        }

        private void rdbKpi_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbKpi.TabIndex;
            btnRun.Enabled = true;
        }

        private void rdbStock_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbStock.TabIndex;
            btnRun.Enabled = true;
        }

        private void rdbOAnalysis_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbOAnalysis.TabIndex;
            btnRun.Enabled = true;
        }

        private void rdbPayroll_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbPayroll.TabIndex;
            btnRun.Enabled = true;
        }

        private void rdbFinance_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbFinance.TabIndex;
            btnRun.Enabled = true;
        }

        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            op = rdbAll.TabIndex;
            btnRun.Enabled = true;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        public string inpLastdtMonth()
        {
            DateTime From = dateTimePicker1.Value;
            DateTime To = dateTimePicker2.Value;
            string fmtdate = "yyyyMMdd";
            string dtFrom = From.ToString(fmtdate);
            string dtTo = To.ToString(fmtdate);

            string inpYear = dtFrom.Substring(0, 4);
            string inpMonth = dtFrom.Substring(4, 2);

            int inpYearInt, inpMonthInt;
            int.TryParse(inpYear, out inpYearInt);
            int.TryParse(inpMonth, out inpMonthInt);

            DateTime dateSerial = new DateTime(inpYearInt, inpMonthInt, 1);
            DateTime lstdaySerial = dateSerial.AddMonths(1).AddDays(-1);

            string fmtdate1 = "yyyyMMdd";
            string LastDtMonth = lstdaySerial.ToString(fmtdate1);
            return LastDtMonth;

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //DateTimePicker date1 = dateTimePicker1.Value;
            //int dtFrom, dtTo;

            DateTime From = dateTimePicker1.Value;
            DateTime To = dateTimePicker2.Value;
            string fmtdate = "yyyyMMdd";
            string dtFrom = From.ToString(fmtdate);
            string dtTo = To.ToString(fmtdate);

            //int PlanDateInt;
            //int.TryParse(FromDate, out dtFrom);
            //int.TryParse(ToDate, out dtTo);
            //string dtFrom1 = dtFrom.ToString();
            //string dtTo1 = dtTo.ToString();
            string yrFrom = dtFrom.Substring(0, 4);
            string monFrom = dtFrom.Substring(4, 2);
            string yrTo = dtTo.Substring(0, 4);
            string monTo = dtTo.Substring(4, 2);
            if (Int32.Parse(dtTo) >= Int32.Parse(dtFrom))
            {
                if (yrFrom == yrTo && monFrom == monTo)
                {
                    switch (op)
                    {
                        case 2:
                            Production(dtFrom, dtTo);
                            btnRun.Enabled = false;
                            break;
                        case 3:
                            Sales(dtFrom, dtTo);
                            btnRun.Enabled = false;
                            break;
                        case 4:
                            Purchase(dtFrom, dtTo);
                            btnRun.Enabled = false;
                            break;
                        case 5:
                            Kpi(dtFrom, dtTo);
                            btnRun.Enabled = false;
                            break;
                        case 6:
                            Stock(dtFrom, dtTo);
                            btnRun.Enabled = false;
                            break;
                        case 7:
                            OverallAnalysis(dtFrom, dtTo);
                            btnRun.Enabled = false;
                            break;
                        case 8:
                            Payroll();
                            btnRun.Enabled = false;
                            break;
                        case 9:
                            Finance();
                            btnRun.Enabled = false;
                            break;
                        case 10:
                            All();
                            break;
                        default:
                            MessageBox.Show("Invalid selection");
                            break;
                    }
                }
                else
                    MessageBox.Show("Selected Date is not valid");

            }
            else
                MessageBox.Show("ToDate must be greater than or equal to FromDate");

        }

        public void Production(string dtFrom, string dtTo)
        {
            Login con = new Login();

            con.loginBexprod();

            //con.CN.Open();
            //MessageBox.Show("Production");

            string SQL = "Delete from p72idat.ksttr00f where vusrnm='BOARD' and vdttra=" + dtTo + " ";
            this.Text = "Step 1 Of 13 !Delete from P72IDAT.KSTTR00F For Bexprod";
            OdbcCommand command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            

            AS400System system = new AS400System();
            system.Define("AS400");
            system.UserID = "ICS";
            system.Password = "INFINITE";
            system.IPAddress = "192.168.100.9";
            system.Connect(cwbcoServiceEnum.cwbcoServiceRemoteCmd);

            if (system.IsConnected(cwbcoServiceEnum.cwbcoServiceRemoteCmd) == 1)
            {

                //DENIM CLP CALL to transfer data into D72IDAT.PSTTR00F

                cwbx.Program program = new cwbx.Program();
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DNCHK";
                program.system = system;

                ProgramParameters parameters = new ProgramParameters();
                parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                cwbx.StringConverter stringConverter = new cwbx.StringConverterClass();
                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                this.Text = "Step 2 Of 13 ! CALL PGM(K72ICSOBJ/DNCHK";
                try
                {
                    program.Call(parameters);
                    lblHelp.Refresh();
                    lblHelp.Text = WordWrap("Denim Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //}

                //PTML CLP CALL to trasfer data into P72IDAT.PSTTR00F
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "PNCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                this.Text = "Step 3 Of 13 ! CALL PGM(K72ICSOBJ/PNCHK)";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }


                //BKL CLP CALL to trasfer data into P72IDAT.PSTTR00F
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KNCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));

                this.Text = "Step 4 Of 13 ! CALL PGM(K72ICSOBJ/KNCHK)";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }


                //BTL CLP CALL to trasfer data into F72IDAT.PSTTR00F
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TNCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                this.Text = "Step 5 Of 13 ! CALL PGM(F72ICSOBJ/TNCHK)";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }


                //BGD CLP CALL to trasfer data into G72IDAT.PSTTR00F
                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GNCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));

                this.Text = "Step 6 Of 13 ! CALL PGM(ICSOBJ/GNCHK";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garment Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }
            }
            else
                MessageBox.Show("As400 Connection Error");

            system.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);

            this.Text = "Step 7 Of 13 ! SELECT THE RECORDS FROM P72IDAT.PSTTR00F AND INSERT INTO P72IDAT.KSTTR00F";
            //DENIM
            //BOARD DATA TRANSFER FROM D72IDAT.PSTTR00F TO P72IDAT.KSTTR00F
            SQL = "INSERT INTO P72IDAT.KSTTR00F SELECT * FROM D72IDAT.PSTTR00F";
            this.Text = "Step 8 Of 13 ! INSERT INTO P72IDAT.KSTTR00F SELECT * FROM D72IDAT.PSTTR00F";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //PTML
            //BOARD DATA TRANSFER FROM P72IDAT.PSTTR00F TO P72IDAT.KSTTR00F
            SQL = "INSERT INTO P72IDAT.KSTTR00F SELECT * FROM P72IDAT.PSTTR00F";
            this.Text = "Step 9 Of 13 ! INSERT INTO P72IDAT.KSTTR00F SELECT * FROM P72IDAT.PSTTR00F";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            //BKL
            //BOARD DATA TRANSFER FROM K72IDAT.PSTTR00F TO P72IDAT.KSTTR00F
            SQL = "INSERT INTO P72IDAT.KSTTR00F SELECT * FROM K72IDAT.PSTTR00F";
            this.Text = "Step 10 Of 13 ! INSERT INTO P72IDAT.KSTTR00F SELECT * FROM K72IDAT.PSTTR00F";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //BTL
            //BOARD DATA TRANSFER FROM F72IDAT.PSTTR00F TO P72IDAT.KSTTR00F
            SQL = "INSERT INTO P72IDAT.KSTTR00F SELECT * FROM F72IDAT.PSTTR00F";
            this.Text = "Step 11 Of 13 ! INSERT INTO P72IDAT.KSTTR00F SELECT * FROM P72IDAT.PSTTR00F";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //BGD

            //Update G72IDAT.PSTTR00F
            SQL = "Update G72IDAT.PSTTR00F set VCLAS1 = 'KNIT', VDESC1 = 'KNIT' where VCLAS2 = 'KNIT' and VUSRNM = 'BOARD'";
            this.Text = "Step 12 Of 13 ! INSERT INTO P72IDAT.KSTTR00F SELECT * FROM G72IDAT.PSTTR00F";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //BOARD DATA TRANSFER FROM G72IDAT.PSTTR00F TO P72IDAT.KSTTR00F
            SQL = "INSERT INTO P72IDAT.KSTTR00F SELECT * FROM G72IDAT.PSTTR00F";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //PLAN
            //PLAN DATA UPLOAD INTO P72IDAT.KSTTR00F
            SQL = "INSERT INTO P72IDAT.KSTTR00F SELECT * FROM P72IDAT.MNPLN00F where fusrnm='BOARD' and fDTTRA=" + dtTo + "  ";
            this.Text = "Step 13 Of 13 ! INSERT INTO P72IDAT.KSTTR00F SELECT * FROM P72IDAT.MNPLN00F";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //UPDATE COMMAND ACCORDING TO ANIL MIS......
            this.Text = "Step:14-Update P72IDAT/KSTTR00F Accorded to AnilM..... !!!";
            lblHelp.Text = "Update P72IDAT/KSTTR00F SET VCLAS1='TBK',VCLAS2='TBK' ,VDESC1='TOP/BOTOM/KNIT',VDESC2='TOP/BOTOM/KNIT'!";

            SQL = "UPDATE P72IDAT.KSTTR00F SET VCLAS1='TBK',VCLAS2='TBK' ,VDESC1='TOP/BOTOM/KNIT',VDESC2='TOP/BOTOM/KNIT' WHERE VREFER='GARMENTS' AND VDTTRA= " + dtTo + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "UPDATE P72IDAT.KSTTR00F SET VREFER='KNIT-SEW' WHERE VREFER='GARMENTS' AND VDTTRA=" + dtTo + " AND VCDDOC IN('IKAL-KNIT','ESSES-KNIT') ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //CHANGE BY VIPIN 20110401
            SQL = "UPDATE P72IDAT.KSTTR00F SET VREFER='KNIT-FIN' WHERE LTrim(VREFER)='GMT-FINISH' AND VDTTRA=" + dtTo + " AND VCDDOC IN('IKAL-KNIT','ESSES-KNIT') ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "UPDATE P72IDAT.KSTTR00F SET VREFER='SUB CON' WHERE VREFER='GARMENTS' AND VDTTRA=" + dtTo + " AND VCDDOC IN ('SUB CON','SUBCON-KNT','SUBCON-WVN')";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //CHANGE BY VIPIN 20110501
            SQL = "UPDATE P72IDAT.KSTTR00F SET VCDDOC='SUBCON-WVN' WHERE LTrim(VREFER)='SUB CON' AND VDTTRA=" + dtTo + " AND VORCAU IN('D1','W1') ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "UPDATE P72IDAT.KSTTR00F SET VCDDOC='SUBCON-KNT' WHERE LTrim(VREFER)='SUB CON' AND VDTTRA=" + dtTo + " AND VORCAU='K1' ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "UPDATE P72IDAT.KSTTR00F SET VCDCCE='PK' WHERE VDTTRA=" + dtTo + " AND substr(VCDORD,1,2)='PK'";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //lblHelp.Text = "BOARD DATA TRANSFER FOR PRODUCTION IS COMPLETE SUCCESSFULLY !!";
            lblHelp.Text = WordWrap("BOARD DATA TRANSFER FOR PRODUCTION IS COMPLETE SUCCESSFULLY !!",30);

            con.CN.Close();
        }
        public void Sales(string dtFrom, string dtTo)
        {
            Login con = new Login();
            con.loginBexprod();

            //MessageBox.Show("Sales");
            //string dtFr = dateTimePicker1.Value.ToString();
            //int dtFromint = (int)dtFrom;
            //string a=dtFr.ToString();

            //string[] b;
            //b = dtFr.Split();
            //string[] c;
            //string c1 = b[0].Substring(6, 4);
            //string c2 = b[0].Substring(3, 2);
            //string inpYear = dtFrom.Substring(0, 4);
            //string inpMonth = dtFrom.Substring(4, 2);
            //int d1, d2;
            //int.TryParse(b[0].Substring(6, 4), out d1);
            //int.TryParse(b[0].Substring(3, 2), out d2);

            //string inpYear = dtFrom.Substring(0, 4);
            //string inpMonth = dtFrom.Substring(4, 2);

            //int inpYearInt, inpMonthInt;
            //int.TryParse(inpYear, out inpYearInt);
            //int.TryParse(inpMonth, out inpMonthInt);

            //DateTime dateSerial = new DateTime(inpYearInt, inpMonthInt, 1);
            //DateTime lstdaySerial = dateSerial.AddMonths(1).AddDays(-1);

            //string fmtdate = "yyyyMMdd";
            //string LastDtMonth = lstdaySerial.ToString(fmtdate);

            string LastDtMonth = inpLastdtMonth();

            
            
            lblHelp.Text = WordWrap("Board Data Transfer for Sales is in Process !!",30);
            lblHelp.Refresh();
            
            string SQL;
            OdbcCommand command = new OdbcCommand();

            this.Text = "Step 1 Of 19 !BTL400! Delete from P72idat.BPMIS00F";
            SQL = "Delete from P72idat.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','CUTSHIP') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "  ";
            command=new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 2 Of 19 !BTL400! Delete from P72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY')";
            SQL = "Delete from P72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY','FABDELAY')";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 3 Of 19 !BTL400! Delete from D72idat.BPMIS00F";
            SQL = "Delete from D72idat.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','CUTSHIP') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "  ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 4 Of 19 !BTL400! Delete from D72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY')";
            SQL = "Delete from D72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY','FABDELAY')";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 5 Of 19 !BTL400! Delete from K72idat.BPMIS00F";
            SQL = "Delete from K72idat.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','CUTSHIP') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "  ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 6 Of 19 !BTL400! Delete from K72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY')";
            SQL = "Delete from K72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY','FABDELAY')";
            command = new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 7 Of 19 !IS400! Delete from F72idat.BPMIS00F";
            SQL = "Delete from F72idat.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','CUTSHIP') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 8 Of 19 !IS400! Delete from F72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY')";
            SQL = "Delete from F72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY','FABDELAY')";
            command = new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 9 Of 19 !BGD400! Delete from G72IDAT.BPMIS00F";
            SQL = "Delete from G72IDAT.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','CUTSHIP') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 10 Of 19 !BGD400! Delete from G72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY')";
            SQL = "Delete from G72idat.BPMIS00F where BUSRNM IN ('OPENORDER','CUTDELAY','FABDELAY')";
            command = new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

            //Call CLP 
            this.Text = "Step 11 Of 19! CALL PGM(D72IOBJ/DSCHK)";

            AS400System system = new AS400System();
            system.Define("AS400");
            system.UserID = "ICS";
            system.Password = "INFINITE";
            system.IPAddress = "192.168.100.9";
            system.Connect(cwbcoServiceEnum.cwbcoServiceRemoteCmd);

            if (system.IsConnected(cwbcoServiceEnum.cwbcoServiceRemoteCmd) == 1)
            {

                //DENIM CLP CALL 

                cwbx.Program program = new cwbx.Program();
                program.LibraryName = "D72IOBJ";
                program.ProgramName = "DSCHK";
                program.system = system;

                ProgramParameters parameters = new ProgramParameters();
                parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                cwbx.StringConverter stringConverter = new cwbx.StringConverterClass();
                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //For Fabric delay

                program.LibraryName = "D72IOBJ";
                program.ProgramName = "DSCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("6".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //PTML CLP call

                this.Text = "Step 12 Of 19! CALL PGM(P72IOBJ/PSCHK)";

                program.LibraryName = "P72IOBJ";
                program.ProgramName = "PSCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Knitting CLP call

                this.Text = "Step 13 Of 19! CALL PGM(K72IOBJ/KSCHK)";

                program.LibraryName = "K72IOBJ";
                program.ProgramName = "KSCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                program.LibraryName = "K72IOBJ";
                program.ProgramName = "KSCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("6".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Textile CLP call

                this.Text = "Step 14 Of 19 !CALL PGM(F72ICSOBJ/TSCHK)";

                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TSCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Fabric Delay

                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TSCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("6".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Garments CLP call

                this.Text = "Step 15 Of 19 !CALL PGM(ICSOBJ/GSCHK)";

                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GSCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garment Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }


            }
            else
                MessageBox.Show("As400 Connection Error");

            system.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);


            this.Text= "Step 16 Of 19! INSERT INTO P72IDAT.BPMIS00F SELECT * from D72idat.BPMIS00F";
            SQL= "INSERT INTO P72IDAT.BPMIS00F SELECT * from D72idat.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','FABDELAY','CUTDELAY') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 17 Of 19 !BTL400!INSERT INTO P72IDAT.BPMIS00F SELECT * from K72idat.BPMIS00F";
            SQL = "INSERT INTO P72IDAT.BPMIS00F SELECT * from K72idat.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','FABDELAY','CUTDELAY') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 18 Of 19 !Select * from F72IDAT.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','FABDELAY','CUTDELAY') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            SQL = "Insert Into P72IDAT.BPMIS00F Select * from F72IDAT.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','FABDELAY','CUTDELAY') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 19 Of 19 !Select * from G72IDAT.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','CUTDELAY','FABDELAY') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            SQL = "Insert Into P72IDAT.BPMIS00F Select * from G72IDAT.BPMIS00F where BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','CUTDELAY','FABDELAY') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Remove the Data from from Board Input file of various order types

            SQL = "DELETE FROM P72IDAT.BPMIS00F WHERE BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','CUTDELAY','FABDELAY') AND BREFER='GARMENT' AND BORCAU NOT IN ('W1','D1','K1','Y1') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            SQL = "DELETE FROM P72IDAT.BPMIS00F WHERE BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','CUTDELAY','FABDELAY') AND BCDDOC='BTL' AND BORCAU NOT IN ('01','30','31','DE','Y1') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            SQL = "DELETE FROM P72IDAT.BPMIS00F WHERE BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','CUTDELAY','FABDELAY') AND BCDDOC='BKL' AND BORCAU NOT IN ('KN','30','Y1') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            SQL = "DELETE FROM P72IDAT.BPMIS00F WHERE BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','CUTDELAY','FABDELAY') AND BCDDOC='BDL' AND BORCAU NOT IN ('01','DE','30','Y1') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            SQL = "DELETE FROM P72IDAT.BPMIS00F WHERE BUSRNM IN ('SALES','ORDERBOOK','OPENORDER','CUTSHIP','CUTDELAY','FABDELAY') AND BREFER='YARN' AND BORCAU NOT IN ('DE','DM','01','02','03','04','Y1') AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            lblHelp.Text = WordWrap("BOARD DATA TRANSFER FOR SALES IS COMPLETED SUCCESSFULLY !!", 25);
            lblHelp.Refresh();

            con.CN.Close();

        }
        public void Purchase(string dtFrom, string dtTo)
        {
            //MessageBox.Show("Purchase");
            Login con = new Login();
            con.loginBexprod();

            string LastDtMonth = inpLastdtMonth();

            lblHelp.Text = "Board Data Transfer for Purchase is in Process !";
            //string SQL;
            //OdbcCommand command;

            SQL = "Delete from P72IDAT.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from D72IDAT.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from K72IDAT.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            SQL = "Delete from F72IDAT.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            SQL = "Delete from G72IDAT.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();


            AS400System system = new AS400System();
            system.Define("AS400");
            system.UserID = "ICS";
            system.Password = "INFINITE";
            system.IPAddress = "192.168.100.9";
            system.Connect(cwbcoServiceEnum.cwbcoServiceRemoteCmd);

            if (system.IsConnected(cwbcoServiceEnum.cwbcoServiceRemoteCmd) == 1)
            {

                //DENIM CLP CALL 

                this.Text = "Step 1 of 9 PGM(K72ICSOBJ/DPCHK) PARM('" + dtFrom + "' + 'To' + '" + dtTo + "' '3')";
                this.Refresh();

                cwbx.Program program = new cwbx.Program();
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DPCHK";
                program.system = system;

                ProgramParameters parameters = new ProgramParameters();
                parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                cwbx.StringConverter stringConverter = new cwbx.StringConverterClass();
                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Purchase Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //PTML CLP CALL
                this.Text = "Step 2 of 9 PGM(K72ICSOBJ/PPCHK) PARM('" + dtFrom + "' + 'To' + '" + dtTo + "' '1')";
                this.Refresh();

                
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "PPCHK";
                program.system = system;
              
                //parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Purchase Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }


                // Knitting CLP Call
                this.Text = "Step 3 of 9 PGM(K72ICSOBJ/KPCHK) PARM('" + dtFrom + "' + 'To' + '" + dtTo + "' '4')";
                this.Refresh();
              
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KPCHK";
                program.system = system;

                //parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Purchase Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Textile CLP Call

                this.Text = "Step 4 of 9 PGM(F72ICSOBJ/TPCHK) PARM('" + dtFrom + "' + 'To' + '" + dtTo + "' '2')";
                this.Refresh();

                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TPCHK";
                program.system = system;

                //parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Purchase Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch 
                {
                    MessageBox.Show("Textile CLP is not running");
                    //if (system.Errors.Count > 0)
                    //{
                    //    foreach (cwbx.Error error in system.Errors)
                    //    {
                    //        Console.WriteLine(error.Text);
                    //    }
                    //}

                    //if (program.Errors.Count > 0)
                    //{
                    //    foreach (cwbx.Error error in program.Errors)
                    //    {
                    //        Console.WriteLine(error.Text);
                    //    }
                    //}
                }

                //Garments CLP Call

                this.Text = "Step 5 of 9 PGM(ICSOBJ/GPCHK) PARM('" + dtFrom + "' + 'To' + '" + dtTo + "' '5')";
                this.Refresh();

                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GPCHK";
                program.system = system;

                //parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garments Purchase Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

            }

            else
                MessageBox.Show("As400 connection Error");

            system.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);


            this.Text = "Step 6 of 9 D72IDAT.BPMIS00F To P72IDAT.BPMIS00F";
            this.Refresh();

            SQL = "INSERT INTO P72IDAT.BPMIS00F SELECT * from D72idat.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 7 of 9 K72IDAT.BPMIS00F To P72IDAT.BPMIS00F";
            this.Refresh();

            SQL = "INSERT INTO P72IDAT.BPMIS00F SELECT * from K72idat.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 8 of 9 F72IDAT.BPMIS00F To P72IDAT.BPMIS00F";
            this.Refresh();

            SQL = "Insert Into P72IDAT.BPMIS00F Select * from F72IDAT.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            this.Text = "Step 9 of 9 G72IDAT.BPMIS00F To P72IDAT.BPMIS00F";
            this.Refresh();

            SQL = "Insert Into P72IDAT.BPMIS00F  Select * from G72IDAT.BPMIS00F where BUSRNM ='PURCHASE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            lblHelp.Text = WordWrap("BOARD DATA TRANSFER FOR PURCHASE IS COMPLETED SUCCESSFULLY !!", 30);
            lblHelp.Refresh();

            con.CN.Close();

        }
        public void Kpi(string dtFrom, string dtTo)
        {
            //MessageBox.Show("Kpi");
            Login con = new Login();
            con.loginBexprod();
            string LastDtMonth = inpLastdtMonth();

            lblHelp.Text = WordWrap("Board Data Transfer for KPI is in Process !!", 30);
            lblHelp.Refresh();
            this.Text = "Clear Records From P72idat.XXXXXXXX";
            this.Refresh();

            //string SQL;
            //Date 2008/02/26  Decession By Rafat/Sabbir
            SQL = "Delete from P72idat.BPSTP00F where  BREFER='GARMENTS' and  BUSRNM='STOPPAGE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /******************************************************************************************/
            //PTML data delete
            SQL = "Delete from P72idat.BPSTP00F where BUSRNM='STOPPAGE' AND BDTTRA>=" + dtTo + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from P72idat.BPEFF00F where BUSRNM='EFFICIENCY' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from P72idat.BPWST00F where VUSRNM='WASTAGE' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from P72idat.BPDGR00F where VUSRNM='DWNGRDTION' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from P72idat.BPSTK00F where VUSRNM='ESTOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /******************************************************************************************/
            //Denim data delete
            this.Text = "Clear Records From D72idat.XXXXXXXX";
            this.Refresh();

            SQL = "Delete from D72idat.BPSTP00F where BUSRNM='STOPPAGE' AND BDTTRA>=" + dtTo + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from D72idat.BPEFF00F where BUSRNM='EFFICIENCY' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from D72idat.BPWST00F where VUSRNM='WASTAGE' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from D72idat.BPDGR00F where VUSRNM='DWNGRDTION' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from D72idat.BPSTK00F where VUSRNM='ESTOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /******************************************************************************************/
            //Knitting data delete
            this.Text = "Clear Records From K72idat.XXXXXXXX";
            this.Refresh();

            SQL = "Delete from K72idat.BPSTP00F where BUSRNM='STOPPAGE' AND BDTTRA>=" + dtTo + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from K72idat.BPEFF00F where BUSRNM='EFFICIENCY' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from K72idat.BPWST00F where VUSRNM='WASTAGE' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from K72idat.BPDGR00F where VUSRNM='DWNGRDTION' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from K72idat.BPSTK00F where VUSRNM='ESTOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /******************************************************************************************/
            //Textile data delete
            this.Text = "Clear Records From F72idat.XXXXXXXX";
            this.Refresh();

            SQL = "Delete from F72idat.BPSTP00F where BUSRNM='STOPPAGE' AND BDTTRA>=" + dtTo + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from F72idat.BPEFF00F where BUSRNM='EFFICIENCY' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from F72idat.BPWST00F where VUSRNM='WASTAGE' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from F72IDAT.BPDGR00F where VUSRNM='DWNGRDTION' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from F72idat.BPSTK00F where VUSRNM='ESTOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /******************************************************************************************/
            //Garments data delete
            this.Text = "Clear Records From G72idat.XXXXXXXX";
            this.Refresh();

            SQL = "Delete from G72idat.BPSTP00F where BUSRNM='STOPPAGE' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from G72idat.BPEFF00F where BUSRNM='EFFICIENCY' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from G72idat.BPWST00F where VUSRNM='WASTAGE' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from G72idat.BPDGR00F where VUSRNM='DWNGRDTION' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from G72idat.BPSTK00F where VUSRNM='ESTOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();


            //AS400 Connection to call CLP
            AS400System system = new AS400System();
            system.Define("AS400");
            system.UserID = "ICS";
            system.Password = "INFINITE";
            system.IPAddress = "192.168.100.9";
            system.Connect(cwbcoServiceEnum.cwbcoServiceRemoteCmd);

            if (system.IsConnected(cwbcoServiceEnum.cwbcoServiceRemoteCmd) == 1)
            {

                //PTML CLP CALL

                //PTML STOPPAGE
                this.Text = "Call K72ICSOBJ/P(T-L)CHK";
                this.Refresh();

                cwbx.Program program = new cwbx.Program();
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "PTCHK";
                program.system = system;

                ProgramParameters parameters = new ProgramParameters();
                parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                cwbx.StringConverter stringConverter = new cwbx.StringConverterClass();
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Stoppage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch 
                {
                    MessageBox.Show("PTML Stoppage CLP is not called successfully");
                    
                }

                //PTML Efficiency
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "PECHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Efficiency Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("PTML Efficiency CLP is not called successfully");
                }

                //PTML Wastage
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "PWCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Efficiency Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("PTML Efficiency CLP is not called successfully");
                }

                //PTML Downgrade/Rejection
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "PDCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Efficiency Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("PTML Efficiency CLP is not called successfully");
                }

                ////PTML Excess/Left Over Stock 
                //program.LibraryName = "K72ICSOBJ";
                //program.ProgramName = "PLCHK";
                //program.system = system;

                //stringConverter.Length = 8;
                //parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                //stringConverter.Length = 8;
                //parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                //stringConverter.Length = 1;
                //parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                //try
                //{
                //    program.Call(parameters);
                //    lblHelp.Text = WordWrap("PTML Efficiency Data Transfer is Over !", 30);
                //    lblHelp.Refresh();
                //}
                //catch
                //{
                //    MessageBox.Show("PTML Efficiency CLP is not called successfully");
                //}

                /**************************************************************************************/
            
                //Denim CLP call

                //Denim Stoppage
                this.Text = "Call K72ICSOBJ/D(T-L)CHK";
                this.Refresh();

                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DTCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Stoppage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Denim Stoppage CLP is not called successfully");
                }

                //Denim Efficiency
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DECHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Efficiency Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Denim Efficiency CLP is not called successfully");
                }

                //Denim Wastage
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DWCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Wastage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Denim Wastage CLP is not called successfully");
                }

                //Denim Downgrade/Rejection
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DDCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Downgrade Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Denim Downgrade CLP is not called successfully");
                }

                //Denim Excess Stock
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DLCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Excess Stock Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Denim Excess Stock CLP is not called successfully");
                }

                /**************************************************************************************/
                //Knitting CLP call
                
                //Knitting Stoppage
                this.Text = "Call K72ICSOBJ/K(T-L)CHK";
                this.Refresh();

                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KTCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Stoppage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Knitting Stoppage CLP is not called successfully");
                }

                //Knitting Efficiency
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KECHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Efficiency Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Knitting Efficiency CLP is not called successfully");
                }

                //Knitting Wastage
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KWCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Wastage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Knitting Wastage CLP is not called successfully");
                }

                //Knitting Downgrade/Rejection
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KDCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Wastage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Knitting Wastage CLP is not called successfully");
                }

                //Knitting Excess Stock
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KLCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("4".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Knitting Wastage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Knitting Wastage CLP is not called successfully");
                }

                /*************************************************************************************/
                //Textile CLP call

                //Textile Stoppage
                this.Text = "Call F72ICSOBJ/T(T-L)CHK";
                this.Refresh();

                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TTCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Stoppage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Textile Stoppage CLP is not called successfully");
                }

                //Textile Efficiency
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TECHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Efficiency Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Textile Efficiency CLP is not called successfully");
                }

                //Textile Wastage
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TWCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Wastage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Textile Wastage CLP is not called successfully");
                }

                //Textile Downgrade/Rejection
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TDCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Downgrade Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Textile Downgrade CLP is not called successfully");
                }

                //Textile Excess Stock
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TLCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Excess Stock Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Textile Excess Stock CLP is not called successfully");
                }

                /*************************************************************************************/
                //Garments CLP call

                //Garments Stoppage
                this.Text = "Call ICSOBJ/G(T-L)CHK";
                this.Refresh();

                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GTCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garments Stoppage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Garments Stoppage CLP is not called successfully");
                }

                //Garments Efficiency
                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GECHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garments Efficiency Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Garments Efficiency CLP is not called successfully");
                }

                //Garments Wastage
                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GWCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garments Wastage Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Garments Wastage CLP is not called successfully");
                }

                //Garments Downgrade/Rejection
                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GDCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garments Downgrade Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Garments Downgrade CLP is not called successfully");
                }

                //Garments Excess Stock
                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GLCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("5".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Garments Excess Stock Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("Garments Excess Stock CLP is not called successfully");
                }
            }

            else
                MessageBox.Show("As400 Connection Error");

            system.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);

            //----------------------------------------------------------------------------------------//
            //Insert into Specific File

            this.Text = "INSERT INTO P72IDAT.BPxxxxxx SELECT * from D72idat.BPEFF00F";
            this.Refresh();

            //Insert into PTML from Denim
            //Stoppage
            SQL = "INSERT INTO P72IDAT.BPSTP00F SELECT * from D72idat.BPSTP00F WHERE BDTTRA>=" + dtTo + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Efficiency
            SQL = "INSERT INTO P72IDAT.BPEFF00F SELECT * from D72idat.BPEFF00F WHERE BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Wastage
            SQL = "INSERT INTO P72IDAT.BPWST00F SELECT * from D72idat.BPWST00F WHERE VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Downgrade
            SQL = "INSERT INTO P72IDAT.BPDGR00F SELECT * from D72idat.BPDGR00F WHERE VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Excess Stock
            SQL = "INSERT INTO P72IDAT.BPSTK00F SELECT * from D72idat.BPSTK00F WHERE VUSRNM='ESTOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();
            
            /******************************************************************************************/
            //Insert into PTML from Knitting
            //Stoppage
            SQL = "INSERT INTO P72IDAT.BPSTP00F SELECT * from K72idat.BPSTP00F WHERE BDTTRA>=" + dtTo + " and BDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Efficiency
            SQL = "INSERT INTO P72IDAT.BPEFF00F SELECT * from K72idat.BPEFF00F WHERE BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Wastage
            SQL = "INSERT INTO P72IDAT.BPWST00F SELECT * from K72idat.BPWST00F WHERE VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Downgrade
            SQL = "INSERT INTO P72IDAT.BPDGR00F SELECT * from K72idat.BPDGR00F WHERE VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Excess Stock
            SQL = "INSERT INTO P72IDAT.BPSTK00F SELECT * from K72idat.BPSTK00F WHERE VUSRNM='ESTOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /******************************************************************************************/
            //Insert into PTML from Textile
            //Stoppage
            SQL = "Insert Into P72IDAT.BPSTP00F Select * from F72IDAT.BPSTP00F where BDTTRA>=" + dtTo + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Efficiency
            SQL = "Insert Into P72IDAT.BPEFF00F Select * from F72IDAT.BPEFF00F where BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Wastage
            SQL = "Insert Into P72IDAT.BPWST00F Select * from F72IDAT.BPWST00F where VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Downgrade
            SQL = "Insert Into P72IDAT.BPDGR00F Select * from F72IDAT.BPDGR00F where VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Excess Stock
            SQL = "Insert Into P72IDAT.BPSTK00F Select * from F72IDAT.BPSTK00F where vusrnm='ESTOCK' and VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /******************************************************************************************/
            //Insert into PTML from Garments
            //Stoppage
            SQL = "Insert Into P72IDAT.BPSTP00F Select * from G72IDAT.BPSTP00F where BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Efficiency
            SQL = "Insert Into P72IDAT.BPEFF00F Select * from G72IDAT.BPEFF00F where BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Wastage
            SQL = "Insert Into P72IDAT.BPWST00F Select * from G72IDAT.BPWST00F where VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Downgarde
            SQL = "Insert Into P72IDAT.BPDGR00F Select * from G72IDAT.BPDGR00F where VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            //Excess Stock
            SQL = "Insert Into P72IDAT.BPSTK00F Select * from G72IDAT.BPSTK00F where vusrnm='ESTOCK' and VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            /*****************************************************************************************/

            SQL = "update P72idat.BPDGR00F set vquaca=vcdqua where VUSRNM='DWNGRDTION' AND vcddoc='BDL' and VCDQUA='3' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "update P72idat.BPDGR00F set vquads=vcdqds where VUSRNM='DWNGRDTION' AND vcddoc='BDL' and VCDQUA='3' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "delete from P72idat.BPDGR00F  where VUSRNM='DWNGRDTION' AND VCDQUA IN ('2','4','5') AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            lblHelp.Text = WordWrap("Reprocess For F72IDAT.BPREP00F is in Process", 30);
            lblHelp.Refresh();

            Reprocess(dtFrom,dtTo);

            lblHelp.Text = WordWrap("Board Data Transfer For KPI is Complete Successfully !!", 30);
            lblHelp.Refresh();

            con.CN.Close();

        }

        public void Reprocess(string dtFrom, string dtTo)
        {
            Login con = new Login();
            con.loginBexprod();
            string LastDtMonth = inpLastdtMonth();

            SQL = "Delete from P72IDAT.BPREP00F where BUSRNM='REPROCESS' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            
            //As400 Connection
            AS400System system = new AS400System();
            system.Define("AS400");
            system.UserID = "ICS";
            system.Password = "INFINITE";
            system.IPAddress = "192.168.100.9";
            system.Connect(cwbcoServiceEnum.cwbcoServiceRemoteCmd);

            if (system.IsConnected(cwbcoServiceEnum.cwbcoServiceRemoteCmd) == 1)
            {

                //CLP CALL 

                this.Text = "CALL PGM(F72ICSOBJ/TCRPR";
                this.Refresh();

                cwbx.Program program = new cwbx.Program();
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TCRPR";
                program.system = system;

                ProgramParameters parameters = new ProgramParameters();
                parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                cwbx.StringConverter stringConverter = new cwbx.StringConverterClass();
                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Reprocess data transfer is over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("CLP is not call Successfully");
                }

                /**************************************************************************************/
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TCRPR";
                program.system = system;
              
                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("2".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Reprocess data transfer is over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("CLP is not call Successfully");
                }

                /**************************************************************************************/
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TCRPR";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                stringConverter.Length = 1;
                parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Textile Reprocess data transfer is over !", 30);
                    lblHelp.Refresh();
                }
                catch
                {
                    MessageBox.Show("CLP is not call Successfully");
                }

                //system.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);
            }
            else
                MessageBox.Show("As400 Connection Error");

            system.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);
            
            this.Text = "Insert Into P72IDAT.BPREP00F From F72IDAT.BPREP00F";
            this.Refresh();

            SQL = "Insert Into P72IDAT.BPREP00F Select * from F72IDAT.BPREP00F where BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL,con.CN);
            command.ExecuteNonQuery();

        }

        public void Stock(string dtFrom, string dtTo)
        {
            //MessageBox.Show("Stock");
            Login con = new Login();
            con.loginBexprod();
            string LastDtMonth = inpLastdtMonth();


            lblHelp.Text = WordWrap("Board Data Transfer for Stock and Consumption is in Process !!", 30);
            lblHelp.Refresh();

            SQL = "Delete from P72idat.BPSTF00F where VUSRNM='STOCK' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + " ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from P72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from D72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from K72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from F72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Delete from G72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();


            AS400System system = new AS400System();
            system.Define("AS400");
            system.UserID = "ICS";
            system.Password = "INFINITE";
            system.IPAddress = "192.168.100.9";
            system.Connect(cwbcoServiceEnum.cwbcoServiceRemoteCmd);

            if (system.IsConnected(cwbcoServiceEnum.cwbcoServiceRemoteCmd) == 1)
            {

                //DENIM CLP CALL 
                this.Text = "CALL PGM(K72ICSOBJ/DCCHK)" + dtFrom + "  to " + dtTo + " ";
                this.Refresh();

                cwbx.Program program = new cwbx.Program();
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "DCCHK";
                program.system = system;

                ProgramParameters parameters = new ProgramParameters();
                parameters.Append("@dtFrom", cwbrcParameterTypeEnum.cwbrcInout, 8);
                parameters.Append("@dtTo", cwbrcParameterTypeEnum.cwbrcInout, 8);
                //parameters.Append("Unit", cwbrcParameterTypeEnum.cwbrcInout, 1);

                cwbx.StringConverter stringConverter = new cwbx.StringConverterClass();
                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                //stringConverter.Length = 1;
                //parameters["Unit"].Value = stringConverter.ToBytes("3".PadRight(1, ' '));
                // parameters["@dtFrom"] = stringConverter.ToBytes(dtFrom.PadRight(paramLength, ' '));

                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("Denim Sales Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //PTML CLP Call
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "PCCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                //stringConverter.Length = 1;
                //parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                this.Text = "Step 3 Of 13 ! CALL PGM(K72ICSOBJ/PCCHK)";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Knitting CLP call
                program.LibraryName = "K72ICSOBJ";
                program.ProgramName = "KCCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                //stringConverter.Length = 1;
                //parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                this.Text = "Step 3 Of 13 ! CALL PGM(K72ICSOBJ/KCCHK)";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Textile CLP call
                program.LibraryName = "F72ICSOBJ";
                program.ProgramName = "TCCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                //stringConverter.Length = 1;
                //parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                this.Text = "Step 3 Of 13 ! CALL PGM(F72ICSOBJ/TCCHK)";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }

                //Garments CLP call
                program.LibraryName = "ICSOBJ";
                program.ProgramName = "GCCHK";
                program.system = system;

                stringConverter.Length = 8;
                parameters["@dtFrom"].Value = stringConverter.ToBytes(dtFrom.PadRight(8, ' '));
                stringConverter.Length = 8;
                parameters["@dtTo"].Value = stringConverter.ToBytes(dtTo.PadRight(8, ' '));
                //stringConverter.Length = 1;
                //parameters["Unit"].Value = stringConverter.ToBytes("1".PadRight(1, ' '));

                this.Text = "Step 3 Of 13 ! CALL PGM(ICSOBJ/GCCHK)";
                try
                {
                    program.Call(parameters);
                    lblHelp.Text = WordWrap("PTML Production Data Transfer is Over !", 30);
                    lblHelp.Refresh();
                }
                catch (Exception ex)
                {
                    if (system.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in system.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }

                    if (program.Errors.Count > 0)
                    {
                        foreach (cwbx.Error error in program.Errors)
                        {
                            Console.WriteLine(error.Text);
                        }
                    }
                }


            }

            else
                MessageBox.Show("As400 Connection Error");

            system.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);

            SQL = "INSERT INTO P72IDAT.MWAVL00B  SELECT * from D72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "INSERT INTO P72IDAT.MWAVL00B  SELECT * from K72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Insert Into P72IDAT.MWAVL00B Select * from F72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "Insert Into P72IDAT.MWAVL00B Select * from G72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = " INSERT INTO P72IDAT.BPSTF00F (VUSRNM,VDTTRA,VREFER,VCDDOC,VCDSTO,VRECTY,VCDKE1,VCDKE2,VCDKE3,VCDKE4,VCDKE5,VDESCR,VSUPDS,VTRAUM,";
            SQL += " VCOMM0, VCLAS1,VCLAS2,VDESC1,VDESC2,VOPQTY,VOPVAL,VRCQTY,VRCVAL,VISQTY,VISVAL,VCLQTY,VCLVAL,VTECU5,VTECU6) (SELECT 'STOCK',PGTDAT,NRFERN,NCDOCN,";
            SQL += " CDSTOD,RECTYD,CODE1,CODE2,CODE3,CODE4,CODE5,SHTDES,DESCRP,CDUMSP,(TRIM(CHAR(PGTDAT)) CONCAT TRIM(CHAR(PGRTIM))),ZCLA11,ZCLA22,ZCL1DS,ZCL2DS,OPENP,OPENVL,(RECVP+RCVTR+RCVOTH),RCVVL,";
            SQL += " (ISSPRD+ISSOTH),(ISSPVL+ISSOVL),(TOTALP-ISSPRD-ISSOTH),BLNVL,SUBSTR(RECDSC,1,10),SUBSTR(WHNMP,1,10)  FROM P72IDAT.MWAVL00B where PGTDAT>=" + dtFrom + " and PGTDAT<=" + LastDtMonth + "  ) ";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            lblHelp.Text = WordWrap("BOARD DATA TRANSFER FOR Stock IS COMPLETED SUCCESSFULLY !!", 30);
            lblHelp.Refresh();

            con.CN.Close();

        }
        public string sqlBOALL00F;
        public void OverallAnalysis(string dtFrom, string dtTo)
        {
            Login con = new Login();
            con.loginBexprod();
            string LastDtMonth = inpLastdtMonth();
            //MessageBox.Show("OverallAnalysis");
            //MessageBox.Show("Connection State=" + con.CN.State);
            lblHelp.Text = WordWrap("Board Data Transfer for Overall Analysis is in Process !!", 30);
            lblHelp.Refresh();

            SQL = "Delete from P72IDAT.BOALL00F where HDTTRA>=" + dtFrom + " and HDTTRA<=" + LastDtMonth + "";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "INSERT INTO P72IDAT.BOALL00F (HANNUL,HDTTRA,HUSRNM,HESERC,HCDPER,HREFER,HCDDOC,HCLAS1,HDESC1,HCLAS2,HDESC2,HCDCUR,";
            SQL += " HTRAUM,HTOTLA,HTOTLB,HTOTLC,HTOTLD )";
            SQL += " SELECT BANNUL,BDTTRA,'PRODUCT GROUP WISE SALES QTY',BESERC,BCDPER,BREFER,BCDDOC,BCLAS1,BDESC1,BCLAS2,BDESC2,BCDCUR,";
            SQL += " BTRAUM,SUM(BTRQTY),SUM(BTRSQ1),SUM(BHDQTY),SUM(BCTQTY) from P72IDAT.BPMIS00F where BUSRNM ='SALES' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "  ";
            SQL += " GROUP BY BANNUL,BDTTRA,BESERC,BCDPER,BREFER,BCDDOC,BCLAS1,BDESC1,BCLAS2,BDESC2,BCDCUR,BTRAUM";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "INSERT INTO P72IDAT.BOALL00F (HANNUL,HDTTRA,HUSRNM,HESERC,HCDPER,HREFER,HCDDOC,HCLAS1,HDESC1,HCLAS2,HDESC2,HCDCUR,";
            SQL += " HTRAUM,HTOTLA,HTOTLB,HTOTLC,HTOTLD )";
            SQL += " SELECT BANNUL,BDTTRA,'PRODUCT GROUP WISE SALES VALUE',BESERC,BCDPER,BREFER,BCDDOC,BCLAS1,BDESC1,BCLAS2,BDESC2,BCDCUR,";
            SQL += " BTRAUM,SUM(BTOTLA),SUM(BTOTLB),SUM(BTOTLC),SUM(BTOTLD) from P72IDAT.BPMIS00F where BUSRNM ='SALES' AND BDTTRA>=" + dtFrom + " and BDTTRA<=" + LastDtMonth + "  ";
            SQL += " GROUP BY BANNUL,BDTTRA,BESERC,BCDPER,BREFER,BCDDOC,BCLAS1,BDESC1,BCLAS2,BDESC2,BCDCUR,BTRAUM";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            SQL = "INSERT INTO P72IDAT.BOALL00F (HANNUL,HDTTRA,HUSRNM,HESERC,HCDPER,HREFER,HCDDOC,HCLAS1,HDESC1,HCLAS2,HDESC2,HCDCUR,";
            SQL += " HTRAUM,HTOTLA,HTOTLB,HTOTLC,HTOTLD )";
            SQL += " SELECT VANNUL,VDTTRA,'PRODUCT GROUP WISE PRODUCTION QTY',SUBSTR(DIGITS(VDTTRA),2,3),SUBSTR(DIGITS(VDTTRA),5,2),VREFER,VCDDOC,VCLAS1,VDESC1,VCLAS2,VDESC2,VCDCUR,";
            SQL += " VTRAUM,SUM(VTRQTY),SUM(VTRSQ1),SUM(VHDQTY),SUM(VCTQTY) from P72IDAT.KSTTR00F where VUSRNM ='BOARD' AND VDTTRA>=" + dtFrom + " and VDTTRA<=" + LastDtMonth + "  ";
            SQL += " GROUP BY VANNUL,VDTTRA,VREFER,VCDDOC,VCLAS1,VDESC1,VCLAS2,VDESC2,VCDCUR,VTRAUM";
            command = new OdbcCommand(SQL, con.CN);
            command.ExecuteNonQuery();

            bool CMapics = false;

            try
            {
                
                //ADODB.Connection cnAdoMap = new ADODB.Connection();
                
                String connectionString = "Dsn=map;uid=ics; pwd=infinite;";
                cnMap = new OdbcConnection(connectionString);
                cnMap.Open();
                CMapics = true;
            }
            catch 
            {
                CMapics = false;
            }
            //MessageBox.Show("" + cnMap.State);
            //ADODB.Recordset rsMap = new ADODB.Recordset();

            SQL = "Select HANNUL,HUSRNM,HDTTRA,substr(HESERC,2,3),HCDPER,HREFER,HCDDSC,HPLHCD,HPLHDS,";
            SQL += " HCDCUR,SUM(HTOTLA) AS HTOTLA,SUM(HTOTLB) AS HTOTLB,SUM(HTOTLC) AS HTOTLC,SUM(HTOTLD) AS HTOTLD  from BTDIFM1.BMPLS00F";
            SQL += " where HUSRNM ='OUTPAYABLE' AND HPLHCD='3' AND HDTTRA>=" + dtFrom + " and HDTTRA<=" + LastDtMonth + "  ";
            SQL += " GROUP BY HANNUL,HUSRNM,HDTTRA,HESERC,HCDPER,HREFER,HCDDSC,HPLHCD,HPLHDS,HCDCUR";

            //SQL = "Select COUNT(*) ";
            //SQL += " from BTDIFM1.BMPLS00F";
            //SQL += " where HDTTRA>=" + dtFrom + " and HDTTRA<=" + LastDtMonth + "  ";

            //SQL += " GROUP BY HANNUL,HUSRNM,HDTTRA,HESERC,HCDPER,HREFER,HCDDSC,HPLHCD,HPLHDS,HCDCUR";
            



            commandMap = new OdbcCommand(SQL, cnMap);
            
            //int cnt = (int)commandMap.ExecuteScalar();

            //IAsyncResult result = commandMap.BeginExecuteNonQuery();

            //int count = 0;
            //while (!result.IsCompleted)
            //{
            //    Console.WriteLine("Waiting ({0})", count++);
            //    System.Threading.Thread.Sleep(1000);
            //}
            //Console.WriteLine("Command complete. Affected {0} rows.",
            //commandMap.EndExecuteNonQuery(result));




            //command.ExecuteNonQuery();
            OdbcDataReader rsBMPLS00F = commandMap.ExecuteReader();

            while(rsBMPLS00F.Read())
            {
                //string sqlBOALL00F;
                sqlBOALL00F = "Insert Into P72IDAT.BOALL00F (HANNUL,HUSRNM,HDTTRA,HESERC,HCDPER,HREFER,HCDDOC,HCLAS1,HDESC1,";
                sqlBOALL00F += "HCDCUR,HTOTLA,HTOTLB,HTOTLC,HTOTLD) Values(";
                sqlBOALL00F += " '" + rsBMPLS00F.GetString(0) + "', 'OUTSTANDING PAYABLES', " + (int)rsBMPLS00F.GetValue(2) + ", '" + rsBMPLS00F.GetString(3) + "', '" + rsBMPLS00F.GetString(4) + "', '" + rsBMPLS00F.GetString(5) + "',";
                sqlBOALL00F += " '" + rsBMPLS00F.GetString(6) + "', '" + rsBMPLS00F.GetString(7) + "', '" + rsBMPLS00F.GetString(8) + "', '" + rsBMPLS00F.GetString(9) + "', " + (int)rsBMPLS00F.GetValue(10) + ", " + (int)rsBMPLS00F.GetValue(11) + ", " + (int)rsBMPLS00F.GetValue(12) + ",";
                sqlBOALL00F += " " + (int)rsBMPLS00F.GetValue(13) + ")";
                command = new OdbcCommand(sqlBOALL00F, con.CN);
                command.ExecuteNonQuery();
            
            }

            SQL = "Select HANNUL,HUSRNM,HDTTRA,substr(HESERC,2,3),HCDPER,HREFER,HCDDSC,HPLHCD,HPLHDS,";
            SQL += " HCDCUR,SUM(HTOTLA) AS HTOTLA,SUM(HTOTLB) AS HTOTLB,SUM(HTOTLC) AS HTOTLC,SUM(HTOTLD) AS HTOTLD from BTDIFM1.BMPLS00F";
            SQL += " where HUSRNM ='OUTPAYABLE' AND HPLHCD='4' AND HDTTRA>=" + dtFrom + " and HDTTRA<=" + LastDtMonth + "  ";
            SQL += " GROUP BY HANNUL,HUSRNM,HDTTRA,HESERC,HCDPER,HREFER,HCDDSC,HPLHCD,HPLHDS,HCDCUR";

            commandMap = new OdbcCommand(SQL, cnMap);
            rsBMPLS00F = commandMap.ExecuteReader();

            while(rsBMPLS00F.Read())
            {
                sqlBOALL00F = "Insert Into P72IDAT.BOALL00F (HANNUL,HUSRNM,HDTTRA,HESERC,HCDPER,HREFER,HCDDOC,HCLAS1,HDESC1,";
                sqlBOALL00F += "HCDCUR,HTOTLA,HTOTLB,HTOTLC,HTOTLD) Values(";
                sqlBOALL00F += " '" + rsBMPLS00F.GetString(0) + "', 'OUTSTANDING RECEIVABLES', " + (int)rsBMPLS00F.GetValue(2) + ", '" + rsBMPLS00F.GetString(3) + "', '" + rsBMPLS00F.GetString(4) + "', '" + rsBMPLS00F.GetString(5) + "',";
                sqlBOALL00F += " '" + rsBMPLS00F.GetString(6) + "', '" + rsBMPLS00F.GetString(7) + "', '" + rsBMPLS00F.GetString(8) + "', '" + rsBMPLS00F.GetString(9) + "', " + (int)rsBMPLS00F.GetValue(10) + ", " + (int)rsBMPLS00F.GetValue(11) + ", " + (int)rsBMPLS00F.GetValue(12) + ",";
                sqlBOALL00F += " " + (int)rsBMPLS00F.GetValue(13) + ")";
                command = new OdbcCommand(sqlBOALL00F, con.CN);
                command.ExecuteNonQuery();

            }

            lblHelp.Text = WordWrap("Board Data Transfer for Overall Analysis is Complete !!", 30);
            lblHelp.Refresh();

            rsBMPLS00F.Close();
            cnMap.Close();
            con.CN.Close();


        }
        
        public void Payroll()
        {
            MessageBox.Show("Payroll");
        }
        public void Finance()
        {
            MessageBox.Show("Finance");
        }
        public void All()
        {
            MessageBox.Show("All");
        }

        public void BoardInterface_Load(object sender, EventArgs e)
        {
            DateTime dt = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1);
            dateTimePicker1.Text = dt.ToShortDateString();

            //DateTime From = dateTimePicker1.Value;
            //DateTime To = dateTimePicker2.Value;
            //string fmtdate = "yyyyMMdd";
            //string dtFrom = From.ToString(fmtdate);
            //string dtTo = To.ToString(fmtdate);

            //string inpYear = dtFrom.Substring(0, 4);
            //string inpMonth = dtFrom.Substring(4, 2);

            //int inpYearInt, inpMonthInt;
            //int.TryParse(inpYear, out inpYearInt);
            //int.TryParse(inpMonth, out inpMonthInt);

            //DateTime dateSerial = new DateTime(inpYearInt, inpMonthInt, 1);
            //DateTime lstdaySerial = dateSerial.AddMonths(1).AddDays(-1);

            //string fmtdate1 = "yyyyMMdd";
            //string LastDtMonth = lstdaySerial.ToString(fmtdate1);
        
    //       preInterface.Caption = "!! BOARD DATA TRANSFER !!"
    //Dim dtBeginOfMonth As String
    //Dim dtCurrent As String
    
    //dtBeginOfMonth = "1" & " /" & Month(Date) & " / " & Year(Date)
    //DTPicker1.Value = CDate(dtBeginOfMonth)
     
    //dtCurrent = CStr((Day(Date))) & " /" & Month(Date) & " / " & Year(Date)
    //DTPicker2.Value = CDate(dtCurr

            //label3.Text="BOARD DATA TRANSFER FOR PRODUCTION IS COMPLETE SUCCESSFULLY !!";
            //lblHelp.Text = WordWrap("BOARD DATA TRANSFER FOR PRODUCTION IS COMPLETE SUCCESSFULLY !!", 30);
         
            
        }

       
        public static string WordWrap(string text, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();
            // Lucidity check

            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(_newline, pos);

                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + _newline.Length;
                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);

                        sb.Append(text, pos, len);
                        sb.Append(_newline);
                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(_newline); // Empty line
            }
            return sb.ToString();
        }

        public static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            if (i < 0)
                return max; // No whitespace found; break at maximum length
            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
        
    }
    
}
