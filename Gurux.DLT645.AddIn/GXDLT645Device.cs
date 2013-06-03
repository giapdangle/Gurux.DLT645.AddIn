using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device;
using Gurux.Device.Editor;
using System.ComponentModel;
using System.Runtime.Serialization;
using Gurux.Communication;

namespace Gurux.DLT645.AddIn
{
	[DataContract(Namespace = "http://www.gurux.org")]
    public class GXDLT645Device : GXDevice
	{
        internal GXDLT645 Parser = new GXDLT645();
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// Initialize settings.
        /// </remarks>
        public GXDLT645Device()
        {
            this.WaitTime = 5000;
            this.GXClient.Bop = (byte)0x68;
            this.GXClient.Eop = (byte)0x16;
            this.GXClient.ChecksumSettings.Type = ChecksumType.Sum8Bit;
            this.GXClient.ChecksumSettings.Count = -2;
            this.GXClient.ChecksumSettings.Position = -2;
            this.GXClient.ChecksumSettings.Start = 0;            
            Parser.IgnoreFrame = true;
            this.AllowedMediaTypes.Add(new GXMediaType("Serial", "<Bps>2400</Bps><Parity>Even</Parity>"));
        }

        protected override void OnDeserializing(bool designMode)
        {
            base.OnDeserializing(designMode);
            Parser = new GXDLT645();
            Parser.IgnoreFrame = true;
        }

		[Browsable(true), ReadOnly(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("CollectorAddress")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
        public ulong CollectorAddress
		{
            get
            {
                return Parser.CollectorAddress;
            }
            set
            {
                if (Parser.CollectorAddress != value)
                {
                    Parser.CollectorAddress = value;
                    NotifyChange("CollectorAddress");
                }
            }
		}

        [Browsable(true), ReadOnly(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("DeviceAddress")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
        public ulong DeviceAddress
        {
            get
            {
                return Parser.DeviceAddress;
            }
            set
            {
                if (Parser.DeviceAddress != value)
                {
                    Parser.DeviceAddress = value;
                    NotifyChange("DeviceAddress");
                }
            }
        }	

		[Browsable(true), ReadOnly(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("WakeUp")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        /// <summary>
        /// Receiver awake bytes.
        /// </summary>
        /// <remarks>
        /// If awaker is used 0xFE is usually send one to four times.
        /// </remarks>        
        public byte[] FrontLeadingBytes
        {
            get;
            set;
        }

		[Browsable(true), ReadOnly(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("OperatorCode")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
		public string OperatorCode
		{
            get
            {
                return Parser.OperatorCode;
            }
            set
            {
                if (Parser.OperatorCode != value)
                {
                    Parser.OperatorCode = value;
                    NotifyChange("OperatorCode");
                }
            }
		}

		[Browsable(true), ReadOnly(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("MeterPassword")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
		public string MeterPassword
		{
            get
            {
                return Parser.MeterPassword;
            }
            set
            {
                if (Parser.MeterPassword != value)
                {
                    Parser.MeterPassword = value;
                    NotifyChange("MeterPassword");
                }
            }
		}

		[Browsable(true), ReadOnly(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("RelayControlPassword")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
		public string RelayControlPassword
		{
            get
            {
                return Parser.RelayControlPassword;
            }
            set
            {
                if (Parser.RelayControlPassword != value)
                {
                    Parser.RelayControlPassword = value;
                    NotifyChange("RelayControlPassword");
                }
            }
		}

		[Browsable(true), ReadOnly(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("LowClassPassword")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
		public string LowClassPassword
		{
            get
            {
                return Parser.LowClassPassword;
            }
            set
            {
                if (Parser.LowClassPassword != value)
                {
                    Parser.LowClassPassword = value;
                    NotifyChange("LowClassPassword");
                }
            }
		}

        override public void Validate(bool designMode, GXTaskCollection tasks)
        {
            if (!designMode && DeviceAddress == 0)
            {
                tasks.Add(new GXTask(this, "DeviceAddress", "Invalid device address")); 
            }
        }            
    }
}
