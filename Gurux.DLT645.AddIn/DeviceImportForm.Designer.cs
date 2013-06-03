namespace Gurux.DLT645.AddIn
{
    partial class DeviceImportForm
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
            this.MeterAddressTB = new System.Windows.Forms.TextBox();
            this.MeterAddressLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MeterAddressTB
            // 
            this.MeterAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MeterAddressTB.Location = new System.Drawing.Point(133, 12);
            this.MeterAddressTB.MaxLength = 16;
            this.MeterAddressTB.Name = "MeterAddressTB";
            this.MeterAddressTB.Size = new System.Drawing.Size(218, 20);
            this.MeterAddressTB.TabIndex = 0;
            // 
            // MeterAddressLbl
            // 
            this.MeterAddressLbl.AutoSize = true;
            this.MeterAddressLbl.Location = new System.Drawing.Point(7, 15);
            this.MeterAddressLbl.Name = "MeterAddressLbl";
            this.MeterAddressLbl.Size = new System.Drawing.Size(86, 13);
            this.MeterAddressLbl.TabIndex = 6;
            this.MeterAddressLbl.Text = "MeterAddressLbl";
            // 
            // DeviceImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 93);
            this.Controls.Add(this.MeterAddressTB);
            this.Controls.Add(this.MeterAddressLbl);
            this.Name = "DeviceImportForm";
            this.Text = "DeviceImportForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MeterAddressTB;
        private System.Windows.Forms.Label MeterAddressLbl;

    }
}