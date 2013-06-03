using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device;
using System.Runtime.Serialization;
using System.ComponentModel;
using Gurux.Device.Editor;

namespace Gurux.DLT645.AddIn
{
	[DataContract(Namespace = "http://www.gurux.org")]
    [Gurux.Device.Editor.GXReadMessage("ReadTableRowsCount", "ReadTableRowsCountReply", Index=1)]
    //[Gurux.Device.Editor.GXReadMessage("ReadTable", "ReadTableReply", Index=2)]
    [GXReadMessage("ReadTable", "UpdateTable", "IsAllTableDataReceived", "ReadTableNext", Index = 2)]
	class GXDLT645Table : GXTable
	{
        [DataMember(Name = "DataID", IsRequired = false, EmitDefaultValue = false)]
        ulong m_DataID;

        [Browsable(true), ReadOnly(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Data ID")]        
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [TypeConverter(typeof(GXDataIDConverter))]
        public ulong DataID
        {
            get
            {
                return m_DataID;
            }
            set
            {
                if (m_DataID != value)
                {
                    m_DataID = value;
                    NotifyChange("DataID");
                }
            }
        }
	}
}
