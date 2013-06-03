using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gurux.Device.Editor;
using Gurux.Common;

namespace Gurux.DLT645.AddIn
{
    public partial class DeviceImportForm : Form, Gurux.Common.IGXWizardPage
    {
        GXDLT645Device Device;

        public DeviceImportForm()
        {            
            InitializeComponent();
            this.MeterAddressLbl.Text = Gurux.DLT645.AddIn.Properties.Resources.MeterAddressTxt;
        }

        #region IGXWizardPage Members

        /// <inheritdoc cref="IGXWizardPage.IsShown"/>
        public bool IsShown()
        {
            return true;
        }

        /// <inheritdoc cref="IGXWizardPage.OnNext"/>
        /// <remarks>
        /// Checks that meter address is given.
        /// </remarks>
        public void Next()
        {
            if (string.IsNullOrEmpty(MeterAddressTB.Text))
            {
                throw new Exception("Invalid device address.");
            }
            Device.DeviceAddress = Convert.ToUInt64(MeterAddressTB.Text);
        }

        /// <inheritdoc cref="IGXWizardPage.OnBack"/>
        public void Back()
        {            
        }

        /// <inheritdoc cref="IGXWizardPage.OnFinish"/>
        public void Finish()
        {         
        }

        /// <inheritdoc cref="IGXWizardPage.OnCancel"/>
        public void Cancel()
        {            
        }

        /// <inheritdoc cref="IGXWizardPage.Initialize"/>
        public void Initialize()
        {
            Device = Target as GXDLT645Device;
            MeterAddressTB.Text = Device.DeviceAddress.ToString();
        }

        /// <inheritdoc cref="IGXWizardPage.EnabledButtons"/>
        public GXWizardButtons EnabledButtons
        {
            get
            {
                return GXWizardButtons.All;
            }
        }

        /// <inheritdoc cref="IGXWizardPage.Caption"/>
        public string Caption
        {
            get
            {
                return Gurux.DLT645.AddIn.Properties.Resources.MeterInformationTxt;
            }
        }

        /// <inheritdoc cref="IGXWizardPage.Description"/>
        public string Description
        {
            get
            {
                return Gurux.DLT645.AddIn.Properties.Resources.UpdateMeterAddressTxt;                
            }
        }

        public object Target
        {
            get;
            set;
        }

        #endregion
    }
}
