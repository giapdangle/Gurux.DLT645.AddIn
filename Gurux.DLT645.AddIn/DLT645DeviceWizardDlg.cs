//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDeviceEditor/Development/AddIns/DLMS/DlmsTypeWizardDlg.cs $
//
// Version:         $Revision: 870 $,
//                  $Date: 2009-09-29 17:21:48 +0300 (ti, 29 syys 2009) $
//                  $Author: airija $
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------


using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Gurux.Device.Editor;
using Gurux.Device;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using Gurux.Common;

namespace Gurux.DLT645.AddIn
{
	/// <summary>
	/// An GXDLT645 specific custom wizard page. The page is used with the GXWizardDlg class.
	/// </summary>
	internal class GXDLT645DeviceWizardDlg : System.Windows.Forms.Form, IGXWizardPage
	{
		GXDLT645Device m_Device = null;
		private System.ComponentModel.Container m_Components = null;
		private TextBox collectorAddressTb;
		private TextBox FrontLeadingBytesTb;
		private TextBox operatorCodeTb;
		private Label operatorCodeLbl;
		private TextBox meterPasswordTb;
		private Label meterPasswordLbl;
		private TextBox relayControlPasswordTb;
        private Label relayControlPasswordLbl;
		private TextBox lowClassPasswordTb;
		private Label lowClassPasswordLbl;
		private CheckBox wakeupCb;
		private CheckBox collectorAddressCb;

		/// <summary>
        /// Initializes a new instance of the GXDLT645DeviceWizardDlg class.
		/// </summary>
		public GXDLT645DeviceWizardDlg()
		{
			InitializeComponent();
		}

		private void UpdateResources()
		{
			collectorAddressCb.Text = Properties.Resources.CollectorAddressTxt;
			wakeupCb.Text = Properties.Resources.FrontLeadingBytesTxt;			
			operatorCodeLbl.Text = Properties.Resources.OperatorCodeTxt;
			meterPasswordLbl.Text = Properties.Resources.MeterPasswordTxt;
			relayControlPasswordLbl.Text = Properties.Resources.RelayControlPasswordTxt;
			lowClassPasswordLbl.Text = Properties.Resources.LowClassPasswordTxt;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (m_Components != null)
				{
					m_Components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.collectorAddressTb = new System.Windows.Forms.TextBox();
            this.FrontLeadingBytesTb = new System.Windows.Forms.TextBox();
            this.operatorCodeTb = new System.Windows.Forms.TextBox();
            this.operatorCodeLbl = new System.Windows.Forms.Label();
            this.meterPasswordTb = new System.Windows.Forms.TextBox();
            this.meterPasswordLbl = new System.Windows.Forms.Label();
            this.relayControlPasswordTb = new System.Windows.Forms.TextBox();
            this.relayControlPasswordLbl = new System.Windows.Forms.Label();
            this.lowClassPasswordTb = new System.Windows.Forms.TextBox();
            this.lowClassPasswordLbl = new System.Windows.Forms.Label();
            this.wakeupCb = new System.Windows.Forms.CheckBox();
            this.collectorAddressCb = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // collectorAddressTb
            // 
            this.collectorAddressTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.collectorAddressTb.Location = new System.Drawing.Point(138, 12);
            this.collectorAddressTb.Name = "collectorAddressTb";
            this.collectorAddressTb.ReadOnly = true;
            this.collectorAddressTb.Size = new System.Drawing.Size(183, 20);
            this.collectorAddressTb.TabIndex = 1;
            // 
            // FrontLeadingBytesTb
            // 
            this.FrontLeadingBytesTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FrontLeadingBytesTb.Location = new System.Drawing.Point(138, 38);
            this.FrontLeadingBytesTb.Name = "FrontLeadingBytesTb";
            this.FrontLeadingBytesTb.ReadOnly = true;
            this.FrontLeadingBytesTb.Size = new System.Drawing.Size(178, 20);
            this.FrontLeadingBytesTb.TabIndex = 3;
            // 
            // operatorCodeTb
            // 
            this.operatorCodeTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.operatorCodeTb.Location = new System.Drawing.Point(138, 64);
            this.operatorCodeTb.MaxLength = 4;
            this.operatorCodeTb.Name = "operatorCodeTb";
            this.operatorCodeTb.Size = new System.Drawing.Size(183, 20);
            this.operatorCodeTb.TabIndex = 5;
            // 
            // operatorCodeLbl
            // 
            this.operatorCodeLbl.AutoSize = true;
            this.operatorCodeLbl.Location = new System.Drawing.Point(12, 67);
            this.operatorCodeLbl.Name = "operatorCodeLbl";
            this.operatorCodeLbl.Size = new System.Drawing.Size(85, 13);
            this.operatorCodeLbl.TabIndex = 4;
            this.operatorCodeLbl.Text = "operatorCodeLbl";
            // 
            // meterPasswordTb
            // 
            this.meterPasswordTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.meterPasswordTb.Location = new System.Drawing.Point(138, 90);
            this.meterPasswordTb.MaxLength = 8;
            this.meterPasswordTb.Name = "meterPasswordTb";
            this.meterPasswordTb.Size = new System.Drawing.Size(183, 20);
            this.meterPasswordTb.TabIndex = 7;
            // 
            // meterPasswordLbl
            // 
            this.meterPasswordLbl.AutoSize = true;
            this.meterPasswordLbl.Location = new System.Drawing.Point(12, 93);
            this.meterPasswordLbl.Name = "meterPasswordLbl";
            this.meterPasswordLbl.Size = new System.Drawing.Size(93, 13);
            this.meterPasswordLbl.TabIndex = 6;
            this.meterPasswordLbl.Text = "meterPasswordLbl";
            // 
            // relayControlPasswordTb
            // 
            this.relayControlPasswordTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.relayControlPasswordTb.Location = new System.Drawing.Point(138, 116);
            this.relayControlPasswordTb.MaxLength = 8;
            this.relayControlPasswordTb.Name = "relayControlPasswordTb";
            this.relayControlPasswordTb.Size = new System.Drawing.Size(183, 20);
            this.relayControlPasswordTb.TabIndex = 9;
            // 
            // relayControlPasswordLbl
            // 
            this.relayControlPasswordLbl.AutoSize = true;
            this.relayControlPasswordLbl.Location = new System.Drawing.Point(12, 119);
            this.relayControlPasswordLbl.Name = "relayControlPasswordLbl";
            this.relayControlPasswordLbl.Size = new System.Drawing.Size(122, 13);
            this.relayControlPasswordLbl.TabIndex = 8;
            this.relayControlPasswordLbl.Text = "relayControlPasswordLbl";
            // 
            // lowClassPasswordTb
            // 
            this.lowClassPasswordTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lowClassPasswordTb.Location = new System.Drawing.Point(138, 142);
            this.lowClassPasswordTb.MaxLength = 8;
            this.lowClassPasswordTb.Name = "lowClassPasswordTb";
            this.lowClassPasswordTb.Size = new System.Drawing.Size(186, 20);
            this.lowClassPasswordTb.TabIndex = 13;
            // 
            // lowClassPasswordLbl
            // 
            this.lowClassPasswordLbl.AutoSize = true;
            this.lowClassPasswordLbl.Location = new System.Drawing.Point(12, 145);
            this.lowClassPasswordLbl.Name = "lowClassPasswordLbl";
            this.lowClassPasswordLbl.Size = new System.Drawing.Size(108, 13);
            this.lowClassPasswordLbl.TabIndex = 12;
            this.lowClassPasswordLbl.Text = "lowClassPasswordLbl";
            // 
            // wakeupCb
            // 
            this.wakeupCb.AutoSize = true;
            this.wakeupCb.Location = new System.Drawing.Point(15, 40);
            this.wakeupCb.Name = "wakeupCb";
            this.wakeupCb.Size = new System.Drawing.Size(127, 17);
            this.wakeupCb.TabIndex = 14;
            this.wakeupCb.Text = "FrontLeadingBytesCb";
            this.wakeupCb.UseVisualStyleBackColor = true;
            this.wakeupCb.CheckedChanged += new System.EventHandler(this.wakeupCb_CheckedChanged);
            // 
            // collectorAddressCb
            // 
            this.collectorAddressCb.AutoSize = true;
            this.collectorAddressCb.Location = new System.Drawing.Point(15, 14);
            this.collectorAddressCb.Name = "collectorAddressCb";
            this.collectorAddressCb.Size = new System.Drawing.Size(117, 17);
            this.collectorAddressCb.TabIndex = 15;
            this.collectorAddressCb.Text = "collectorAddressCb";
            this.collectorAddressCb.UseVisualStyleBackColor = true;
            this.collectorAddressCb.CheckedChanged += new System.EventHandler(this.collectorAddressCb_CheckedChanged);
            // 
            // GXDLT645DeviceWizardDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 193);
            this.Controls.Add(this.collectorAddressCb);
            this.Controls.Add(this.wakeupCb);
            this.Controls.Add(this.lowClassPasswordTb);
            this.Controls.Add(this.lowClassPasswordLbl);
            this.Controls.Add(this.relayControlPasswordTb);
            this.Controls.Add(this.relayControlPasswordLbl);
            this.Controls.Add(this.meterPasswordTb);
            this.Controls.Add(this.meterPasswordLbl);
            this.Controls.Add(this.operatorCodeTb);
            this.Controls.Add(this.operatorCodeLbl);
            this.Controls.Add(this.FrontLeadingBytesTb);
            this.Controls.Add(this.collectorAddressTb);
            this.Name = "GXDLT645DeviceWizardDlg";
            this.Text = "DlmsTypeWizardDlg";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        #region IGXWizardPage Members

        public void Back()
		{
		}

		public void Next()
		{
            if (string.IsNullOrEmpty(this.operatorCodeTb.Text))
            {
                throw new Exception("Invalid Operator code.");
            }
            if (collectorAddressCb.Checked)
            {
                try
                {
                    Convert.ToUInt64(collectorAddressTb.Text);
                }
                catch
                {
                    throw new Exception("Collector Address is in wrong format.");
                }
            }
            if (wakeupCb.Checked)
            {
                try
                {
                    ASCIIEncoding.ASCII.GetBytes(FrontLeadingBytesTb.Text);
                }
                catch
                {
                    throw new Exception("Front leading bytes are in wrong format.");
                }                
            }
		}

		/// <summary>
		/// Gets a string that describes the wizard page object.
		/// </summary>
		public string Description
		{
			get
			{
				return "Description"; //TODO add proper description
			}
		}

		public string Caption
		{
			get
			{
				return "Caption"; //TODO add proper description
			}
		}

		public GXWizardButtons EnabledButtons
		{
			get
			{
				return GXWizardButtons.All;
			}
		}

		public void Finish()
		{
            m_Device.CollectorAddress = collectorAddressCb.Checked ? Convert.ToUInt64(collectorAddressTb.Text) : 0;
            m_Device.FrontLeadingBytes = wakeupCb.Checked ? ASCIIEncoding.ASCII.GetBytes(FrontLeadingBytesTb.Text) : null;
			m_Device.OperatorCode = operatorCodeTb.Text;
			m_Device.MeterPassword = meterPasswordTb.Text;
			m_Device.RelayControlPassword = relayControlPasswordTb.Text;
			m_Device.LowClassPassword = lowClassPasswordTb.Text;
		}

        public void Initialize()
		{
            m_Device = Target as GXDLT645Device;
            collectorAddressTb.Text = m_Device.CollectorAddress.ToString();
            collectorAddressCb.Checked = m_Device.CollectorAddress != 0;
            if (m_Device.FrontLeadingBytes != null)
            {
                FrontLeadingBytesTb.Text = BitConverter.ToString(m_Device.FrontLeadingBytes).Replace('-', ' ');
            }
            wakeupCb.Checked = m_Device.FrontLeadingBytes != null;
            operatorCodeTb.Text = m_Device.OperatorCode;
            meterPasswordTb.Text = m_Device.MeterPassword;
            relayControlPasswordTb.Text = m_Device.RelayControlPassword;
            lowClassPasswordTb.Text = m_Device.LowClassPassword;
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            UpdateResources();
        }

		public void Cancel()
		{
		}

		public bool IsShown()
		{
			return true;
		}

        public object Target
        {
            get;
            set;
        }

		#endregion

		private void collectorAddressCb_CheckedChanged(object sender, EventArgs e)
		{
			collectorAddressTb.ReadOnly = !collectorAddressCb.Checked;
		}

		private void wakeupCb_CheckedChanged(object sender, EventArgs e)
		{
            FrontLeadingBytesTb.ReadOnly = !wakeupCb.Checked;
		}
	}
}
