//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDeviceEditor/Development/AddIns/DLMS/AddressDlg.cs $
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Xml;
using Gurux.Device.Editor;
using Gurux.Device;
using System.Collections.Generic;
using Gurux.Common;

namespace Gurux.DLT645.AddIn
{
	/// <summary>
	/// A DLT645 specific custom wizard page. The page is used with the GXWizardDlg class.
	/// </summary>
	internal class AddressDlg : System.Windows.Forms.Form, IGXWizardPage
    {
		GXDLT645Table m_Table = null;
		GXDLT645Property m_Property = null;
		private System.ComponentModel.Container m_Components = null;
		private ComboBox DataIDCB;
		private Label DataIDLbl;
        private ComboBox typeCb;
        private Label NameLbl;
        private TextBox NameTB;
        private Label typeLbl;

        public AddressDlg()
        {
            InitializeComponent();
            typeCb.Items.AddRange(Enum.GetNames(typeof(DataType)));
            UpdateResources();
        }

		private void UpdateResources()
		{
			DataIDLbl.Text = Properties.Resources.DataIDTxt;
            typeLbl.Text = Properties.Resources.TypeTxt;
		}

        private void FillTables()
        {
            Dictionary<ulong, object> items = GXDLT645Property.ReadDataID();
            foreach (var it in items)
            {
                string tmp = string.Format("{0:x8}", it.Key);
                if (it.Value is GXDLT645TableTemplate)
                {
                    DataIDCB.Items.Add(tmp + " " + ((GXDLT645TableTemplate)it.Value).Name);
                }
            }            
        }


		private void FillProperties()
		{
            Dictionary<ulong, object> items = GXDLT645Property.ReadDataID();
            foreach (var it in items)
            {
                string tmp = string.Format("{0:x8}", it.Key);
                if (it.Value is GXDLT645Data)
                {
                    DataIDCB.Items.Add(tmp + " " + ((GXDLT645Data)it.Value).Name);
                }
            }
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
            this.DataIDCB = new System.Windows.Forms.ComboBox();
            this.DataIDLbl = new System.Windows.Forms.Label();
            this.typeCb = new System.Windows.Forms.ComboBox();
            this.typeLbl = new System.Windows.Forms.Label();
            this.NameLbl = new System.Windows.Forms.Label();
            this.NameTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DataIDCB
            // 
            this.DataIDCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DataIDCB.FormattingEnabled = true;
            this.DataIDCB.Location = new System.Drawing.Point(70, 9);
            this.DataIDCB.Name = "DataIDCB";
            this.DataIDCB.Size = new System.Drawing.Size(240, 21);
            this.DataIDCB.TabIndex = 0;
            this.DataIDCB.SelectedIndexChanged += new System.EventHandler(this.DataIDCB_SelectedIndexChanged);
            // 
            // DataIDLbl
            // 
            this.DataIDLbl.Location = new System.Drawing.Point(4, 12);
            this.DataIDLbl.Name = "DataIDLbl";
            this.DataIDLbl.Size = new System.Drawing.Size(90, 16);
            this.DataIDLbl.TabIndex = 13;
            this.DataIDLbl.Text = "addressLbl";
            // 
            // typeCb
            // 
            this.typeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeCb.Location = new System.Drawing.Point(70, 61);
            this.typeCb.Name = "typeCb";
            this.typeCb.Size = new System.Drawing.Size(240, 21);
            this.typeCb.TabIndex = 2;
            // 
            // typeLbl
            // 
            this.typeLbl.Location = new System.Drawing.Point(5, 63);
            this.typeLbl.Name = "typeLbl";
            this.typeLbl.Size = new System.Drawing.Size(55, 16);
            this.typeLbl.TabIndex = 16;
            this.typeLbl.Text = "TypeLbl";
            // 
            // NameLbl
            // 
            this.NameLbl.Location = new System.Drawing.Point(5, 38);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(55, 16);
            this.NameLbl.TabIndex = 17;
            this.NameLbl.Text = "NameLbl";
            // 
            // NameTB
            // 
            this.NameTB.Location = new System.Drawing.Point(70, 35);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(240, 20);
            this.NameTB.TabIndex = 1;
            // 
            // AddressDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(322, 208);
            this.Controls.Add(this.NameTB);
            this.Controls.Add(this.NameLbl);
            this.Controls.Add(this.typeCb);
            this.Controls.Add(this.typeLbl);
            this.Controls.Add(this.DataIDCB);
            this.Controls.Add(this.DataIDLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddressDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddressDlg";
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
            if (string.IsNullOrEmpty(NameTB.Text))
            {
                throw new Exception("Invalid Name.");
            }            
            if (string.IsNullOrEmpty(this.DataIDCB.Text))
            {
                throw new Exception("Invalid Data ID code.");
            }
            //Type is not used for tables.
            if (typeCb.Visible && typeCb.SelectedIndex == -1)
            {
                throw new Exception("Invalid Data type.");
            }
		}

		/// <summary>
		/// Gets a string that describes the address dialog.
		/// </summary>
		public string Description
		{
			get
			{
				return string.Empty;
			}
		}

		public string Caption
		{
			get
			{
                return "DL/T 645-2007";
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
            if (m_Property != null && typeCb.SelectedItem != null)
            {
                m_Property.Type = (DataType)Enum.Parse(typeof(DataType), typeCb.SelectedItem.ToString());                
            }
            string address = DataIDCB.Text;
            if (address.Contains(" "))
            {
                address = address.Split(' ')[0];
            }
            if (m_Property != null)
            {
                m_Property.Name = NameTB.Text;
                m_Property.DataID = Convert.ToUInt64(address, 16);
            }
            else if (m_Table != null)
            {
                m_Table.Name = NameTB.Text;
                m_Table.DataID = Convert.ToUInt64(address, 16);
                Dictionary<ulong, object> items = GXDLT645Property.ReadDataID();
                GXDLT645TableTemplate t = items[m_Table.DataID] as GXDLT645TableTemplate;
                foreach (GXDLT645Data it in t.Columns)
                {
                    m_Table.Columns.Add(new GXDLT645Property(it.DataID, it.Name, it.Type, it.Access));
                }
            }
		}

		public void Initialize()
		{
            if (Target is GXDLT645Property)
            {
                m_Property = Target as GXDLT645Property;
                NameTB.Text = m_Property.Name;
                DataIDCB.DropDownStyle = ComboBoxStyle.DropDown;
                FillProperties();
            }
            else if (Target is GXDLT645Table)
            {
                m_Table = Target as GXDLT645Table;
                NameTB.Text = m_Table.Name;
                DataIDCB.DropDownStyle = ComboBoxStyle.DropDownList;
                FillTables();
            }
            if (Target is GXDLT645Property)
            {
                string tmp = string.Format("{0:x8}", m_Property.DataID);
                DataIDCB.SelectedIndex = DataIDCB.FindString(tmp);
            }
            else if (Target is GXDLT645Table)
            {
                string tmp = string.Format("{0:x8}", m_Table.DataID);
                DataIDCB.SelectedIndex = DataIDCB.FindString(tmp);
            }
            if (m_Property == null) //If table.
            {
                typeLbl.Visible = typeCb.Visible = false;
            }
            else //If property.
            {
                if (m_Property.Type != DataType.None)
                {
                    typeCb.SelectedItem = m_Property.Type.ToString();
                }
                else
                {
                    typeCb.SelectedIndex = -1;
                }
            }
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

        private void DataIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<ulong, object> items = GXDLT645Property.ReadDataID();
            string address = DataIDCB.Text;
            if (address.Contains(" "))
            {
                NameTB.Text = address.Substring(address.IndexOf(' '));
                address = address.Split(' ')[0];
                ulong id = Convert.ToUInt64(address, 16);
                if (items.ContainsKey(id))
                {
                    GXDLT645Data d = items[id] as GXDLT645Data;
                    if (d != null)
                    {
                        typeCb.SelectedItem = d.Type.ToString();
                    }
                }
            }
            else
            {
                typeCb.SelectedIndex = -1;
            }
        }      
	}
}
