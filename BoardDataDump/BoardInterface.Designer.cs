namespace BoardDataDump
{
    partial class BoardInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblHelp = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.rdbFinance = new System.Windows.Forms.RadioButton();
            this.rdbPayroll = new System.Windows.Forms.RadioButton();
            this.rdbOAnalysis = new System.Windows.Forms.RadioButton();
            this.rdbStock = new System.Windows.Forms.RadioButton();
            this.rdbKpi = new System.Windows.Forms.RadioButton();
            this.rdbPurchase = new System.Windows.Forms.RadioButton();
            this.rdbSales = new System.Windows.Forms.RadioButton();
            this.rdbProduction = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblHelp);
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.btnRun);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board Data Transfer";
            // 
            // lblHelp
            // 
            this.lblHelp.AutoSize = true;
            this.lblHelp.Location = new System.Drawing.Point(164, 130);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(143, 39);
            this.lblHelp.TabIndex = 4;
            this.lblHelp.Text = "label3                                    \r\n\r\n                                 ";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(243, 351);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(168, 351);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dateTimePicker2);
            this.groupBox3.Controls.Add(this.dateTimePicker1);
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(320, 55);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(198, 20);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(110, 20);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(43, 20);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(110, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdbAll);
            this.groupBox2.Controls.Add(this.rdbFinance);
            this.groupBox2.Controls.Add(this.rdbPayroll);
            this.groupBox2.Controls.Add(this.rdbOAnalysis);
            this.groupBox2.Controls.Add(this.rdbStock);
            this.groupBox2.Controls.Add(this.rdbKpi);
            this.groupBox2.Controls.Add(this.rdbPurchase);
            this.groupBox2.Controls.Add(this.rdbSales);
            this.groupBox2.Controls.Add(this.rdbProduction);
            this.groupBox2.Location = new System.Drawing.Point(6, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 294);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = true;
            this.rdbAll.Location = new System.Drawing.Point(19, 260);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(36, 17);
            this.rdbAll.TabIndex = 10;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All";
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.rdbAll_CheckedChanged);
            // 
            // rdbFinance
            // 
            this.rdbFinance.AutoSize = true;
            this.rdbFinance.Location = new System.Drawing.Point(19, 230);
            this.rdbFinance.Name = "rdbFinance";
            this.rdbFinance.Size = new System.Drawing.Size(63, 17);
            this.rdbFinance.TabIndex = 9;
            this.rdbFinance.TabStop = true;
            this.rdbFinance.Text = "Finance";
            this.rdbFinance.UseVisualStyleBackColor = true;
            this.rdbFinance.CheckedChanged += new System.EventHandler(this.rdbFinance_CheckedChanged);
            // 
            // rdbPayroll
            // 
            this.rdbPayroll.AutoSize = true;
            this.rdbPayroll.Location = new System.Drawing.Point(19, 200);
            this.rdbPayroll.Name = "rdbPayroll";
            this.rdbPayroll.Size = new System.Drawing.Size(56, 17);
            this.rdbPayroll.TabIndex = 8;
            this.rdbPayroll.TabStop = true;
            this.rdbPayroll.Text = "Payroll";
            this.rdbPayroll.UseVisualStyleBackColor = true;
            this.rdbPayroll.CheckedChanged += new System.EventHandler(this.rdbPayroll_CheckedChanged);
            // 
            // rdbOAnalysis
            // 
            this.rdbOAnalysis.AutoSize = true;
            this.rdbOAnalysis.Location = new System.Drawing.Point(19, 170);
            this.rdbOAnalysis.Name = "rdbOAnalysis";
            this.rdbOAnalysis.Size = new System.Drawing.Size(99, 17);
            this.rdbOAnalysis.TabIndex = 7;
            this.rdbOAnalysis.TabStop = true;
            this.rdbOAnalysis.Text = "Overall Analysis";
            this.rdbOAnalysis.UseVisualStyleBackColor = true;
            this.rdbOAnalysis.CheckedChanged += new System.EventHandler(this.rdbOAnalysis_CheckedChanged);
            // 
            // rdbStock
            // 
            this.rdbStock.AutoSize = true;
            this.rdbStock.Location = new System.Drawing.Point(19, 140);
            this.rdbStock.Name = "rdbStock";
            this.rdbStock.Size = new System.Drawing.Size(117, 17);
            this.rdbStock.TabIndex = 6;
            this.rdbStock.TabStop = true;
            this.rdbStock.Text = "Stock-Consumption";
            this.rdbStock.UseVisualStyleBackColor = true;
            this.rdbStock.CheckedChanged += new System.EventHandler(this.rdbStock_CheckedChanged);
            // 
            // rdbKpi
            // 
            this.rdbKpi.AutoSize = true;
            this.rdbKpi.Location = new System.Drawing.Point(19, 110);
            this.rdbKpi.Name = "rdbKpi";
            this.rdbKpi.Size = new System.Drawing.Size(42, 17);
            this.rdbKpi.TabIndex = 5;
            this.rdbKpi.TabStop = true;
            this.rdbKpi.Text = "KPI";
            this.rdbKpi.UseVisualStyleBackColor = true;
            this.rdbKpi.CheckedChanged += new System.EventHandler(this.rdbKpi_CheckedChanged);
            // 
            // rdbPurchase
            // 
            this.rdbPurchase.AutoSize = true;
            this.rdbPurchase.Location = new System.Drawing.Point(19, 80);
            this.rdbPurchase.Name = "rdbPurchase";
            this.rdbPurchase.Size = new System.Drawing.Size(70, 17);
            this.rdbPurchase.TabIndex = 4;
            this.rdbPurchase.TabStop = true;
            this.rdbPurchase.Text = "Purchase";
            this.rdbPurchase.UseVisualStyleBackColor = true;
            this.rdbPurchase.CheckedChanged += new System.EventHandler(this.rdbPurchase_CheckedChanged);
            // 
            // rdbSales
            // 
            this.rdbSales.AutoSize = true;
            this.rdbSales.Location = new System.Drawing.Point(19, 50);
            this.rdbSales.Name = "rdbSales";
            this.rdbSales.Size = new System.Drawing.Size(51, 17);
            this.rdbSales.TabIndex = 3;
            this.rdbSales.TabStop = true;
            this.rdbSales.Text = "Sales";
            this.rdbSales.UseVisualStyleBackColor = true;
            this.rdbSales.CheckedChanged += new System.EventHandler(this.rdbSales_CheckedChanged);
            // 
            // rdbProduction
            // 
            this.rdbProduction.AutoSize = true;
            this.rdbProduction.Location = new System.Drawing.Point(19, 20);
            this.rdbProduction.Name = "rdbProduction";
            this.rdbProduction.Size = new System.Drawing.Size(76, 17);
            this.rdbProduction.TabIndex = 2;
            this.rdbProduction.TabStop = true;
            this.rdbProduction.Text = "Production";
            this.rdbProduction.UseVisualStyleBackColor = true;
            this.rdbProduction.CheckedChanged += new System.EventHandler(this.rdbProduction_CheckedChanged);
            // 
            // ModulesInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(346, 409);
            this.Controls.Add(this.groupBox1);
            this.Name = "ModulesInterface";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ModulesInterface";
            this.Load += new System.EventHandler(this.BoardInterface_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbPurchase;
        private System.Windows.Forms.RadioButton rdbSales;
        private System.Windows.Forms.RadioButton rdbProduction;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.RadioButton rdbFinance;
        private System.Windows.Forms.RadioButton rdbPayroll;
        private System.Windows.Forms.RadioButton rdbOAnalysis;
        private System.Windows.Forms.RadioButton rdbStock;
        private System.Windows.Forms.RadioButton rdbKpi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblHelp;

    }
}