
namespace webCrawling
{
    partial class frmMTMProgram
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.closeBtn = new System.Windows.Forms.Button();
            this.insertMTMValue = new System.Windows.Forms.Button();
            this.searchMTMData = new System.Windows.Forms.Button();
            this.standardTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grd_AVG_TNRVAL = new DevExpress.XtraGrid.GridControl();
            this.grdv_AVG_TNRVAL = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.OracleCommand1 = new Oracle.ManagedDataAccess.Client.OracleCommand();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd_AVG_TNRVAL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdv_AVG_TNRVAL)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.8927F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.1073F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 628F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(944, 692);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.closeBtn);
            this.panel1.Controls.Add(this.insertMTMValue);
            this.panel1.Controls.Add(this.searchMTMData);
            this.panel1.Controls.Add(this.standardTime);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(926, 50);
            this.panel1.TabIndex = 0;
            // 
            // closeBtn
            // 
            this.closeBtn.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.closeBtn.Location = new System.Drawing.Point(838, 10);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 30);
            this.closeBtn.TabIndex = 6;
            this.closeBtn.Text = "종료";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // insertMTMValue
            // 
            this.insertMTMValue.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.insertMTMValue.Location = new System.Drawing.Point(757, 10);
            this.insertMTMValue.Name = "insertMTMValue";
            this.insertMTMValue.Size = new System.Drawing.Size(75, 30);
            this.insertMTMValue.TabIndex = 5;
            this.insertMTMValue.Text = "입력";
            this.insertMTMValue.UseVisualStyleBackColor = true;
            this.insertMTMValue.Click += new System.EventHandler(this.insertMTMValue_Click);
            // 
            // searchMTMData
            // 
            this.searchMTMData.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.searchMTMData.Location = new System.Drawing.Point(676, 10);
            this.searchMTMData.Name = "searchMTMData";
            this.searchMTMData.Size = new System.Drawing.Size(75, 30);
            this.searchMTMData.TabIndex = 4;
            this.searchMTMData.Text = "조회";
            this.searchMTMData.UseVisualStyleBackColor = true;
            this.searchMTMData.Click += new System.EventHandler(this.searchMTMData_Click);
            // 
            // standardTime
            // 
            this.standardTime.Location = new System.Drawing.Point(92, 15);
            this.standardTime.Name = "standardTime";
            this.standardTime.Size = new System.Drawing.Size(169, 21);
            this.standardTime.TabIndex = 3;
            this.standardTime.Value = new System.DateTime(2023, 1, 12, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "기준일자 : ";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grd_AVG_TNRVAL);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(938, 622);
            this.panel2.TabIndex = 2;
            // 
            // grd_AVG_TNRVAL
            // 
            this.grd_AVG_TNRVAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd_AVG_TNRVAL.Location = new System.Drawing.Point(0, 0);
            this.grd_AVG_TNRVAL.MainView = this.grdv_AVG_TNRVAL;
            this.grd_AVG_TNRVAL.Name = "grd_AVG_TNRVAL";
            this.grd_AVG_TNRVAL.Size = new System.Drawing.Size(938, 622);
            this.grd_AVG_TNRVAL.TabIndex = 0;
            this.grd_AVG_TNRVAL.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdv_AVG_TNRVAL});
            // 
            // grdv_AVG_TNRVAL
            // 
            this.grdv_AVG_TNRVAL.GridControl = this.grd_AVG_TNRVAL;
            this.grdv_AVG_TNRVAL.Name = "grdv_AVG_TNRVAL";
            // 
            // OracleCommand1
            // 
            this.OracleCommand1.Transaction = null;
            // 
            // frmMTMProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 692);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmMTMProgram";
            this.Text = "민평평균 RPA 프로그램";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd_AVG_TNRVAL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdv_AVG_TNRVAL)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker standardTime;
        private System.Windows.Forms.Button searchMTMData;
        private System.Windows.Forms.Button insertMTMValue;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Panel panel2;
        internal Oracle.ManagedDataAccess.Client.OracleCommand OracleCommand1;
        private DevExpress.XtraGrid.GridControl grd_AVG_TNRVAL;
        private DevExpress.XtraGrid.Views.Grid.GridView grdv_AVG_TNRVAL;
    }
}

