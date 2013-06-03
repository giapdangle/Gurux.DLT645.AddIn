using System;
using System.Collections.Generic;
using System.Text;
using Gurux.Device;
using System.Runtime.Serialization;
using System.ComponentModel;
using Gurux.Device.Editor;
using System.IO;
using System.Xml;

namespace Gurux.DLT645.AddIn
{
    class GXDLT645Data
    {
        public GXDLT645Data(string name)
        {
            Access = AccessMode.Read;
            Name = name;
        }
        public string Name
        {
            get;
            set;
        }

        public DataType Type
        {
            get;
            set;
        }

        public AccessMode Access
        {
            get;
            set;
        }

        public ulong DataID
        {
            get;
            set;
        }
    }

    class GXDLT645TableTemplate
    {
        public GXDLT645TableTemplate(string name)
        {
            Name = name;
            Columns = new List<GXDLT645Data>();
        }
        public string Name
        {
            get;
            set;
        }

        public List<GXDLT645Data> Columns
        {
            get;
            private set;
        }
    }   

	[Gurux.Device.Editor.GXReadMessage("ReadData", "ReadDataReply")]
    [Gurux.Device.Editor.GXWriteMessage("WriteData", "WriteDataReply")]
	[DataContract(Namespace = "http://www.gurux.org")]
    class GXDLT645Property : GXProperty
	{
        ulong m_DataID;
        Dictionary<ulong, object> m_Items;

        /// <summary>
        /// Constructor.
        /// </summary>
		public GXDLT645Property()
		{
            this.AccessMode = AccessMode.Read;
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLT645Property(ulong dataID, string name, DataType type, AccessMode access) : 
            base(name)
        {
            this.DataID = dataID;
            Type = type;
            this.AccessMode = access;
        }

        internal Dictionary<ulong, object> GetDataIDCollection()
        {
            if (m_Items == null)
            {
                m_Items = ReadDataID();
            }
            return m_Items;
        }        

        static internal Dictionary<ulong, object> ReadDataID()
        {
            string filePath = Path.Combine(Path.GetDirectoryName(typeof(GXDLT645Property).Assembly.Location), "DLTProperties.xml");            
            XmlDataDocument myXmlDocument = new XmlDataDocument();            
            if (!File.Exists(filePath))
            {
                myXmlDocument.LoadXml(Gurux.DLT645.AddIn.Properties.Resources.DLTProperties);
				try
				{
					using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.Default))
					{
						myXmlDocument.Save(writer);
						writer.Close();
					}
				}
				catch (System.IO.IOException)
				{
					//This is probably due to insufficient file access rights.
				}
            }
            else
            {                
                myXmlDocument.Load(filePath);
            }
            Dictionary<ulong, object> items = new Dictionary<ulong, object>();
            foreach (XmlNode parentNode in myXmlDocument.DocumentElement.ChildNodes)
            {
                if (parentNode.Name == "Properties")
                {
                    GetProperties(items, parentNode);
                }
                else if (parentNode.Name == "Tables")
                {
                    foreach (XmlNode table in parentNode.ChildNodes)
                    {
                        GXDLT645TableTemplate item = new GXDLT645TableTemplate(table.Attributes["Name"].Value);
                        items.Add(Convert.ToUInt64(table.Attributes["ID"].Value, 16), item);
                        foreach (XmlNode prop in parentNode.ChildNodes)
                        {                            
                            GetColumns(item, prop);
                        }
                    }
                }
            }
            return items;
        }

        private static void GetColumns(GXDLT645TableTemplate table, XmlNode parentNode)
        {
            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {                    
                    GXDLT645Data item = new GXDLT645Data(childNode.Attributes["Name"].Value);
                    if (childNode.Attributes["Type"] != null)
                    {
                        item.Type = (DataType)Enum.Parse(typeof(DataType), childNode.Attributes["Type"].Value);
                    }
                    table.Columns.Add(item);
                }
            }
        }

        private static void GetProperties(Dictionary<ulong, object> items, XmlNode parentNode)
        {
            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    GXDLT645Data item = new GXDLT645Data(childNode.Attributes["Name"].Value);
                    item.DataID = Convert.ToUInt64(childNode.Attributes["ID"].Value, 16);
                    if (childNode.Attributes["Type"] != null)
                    {
                        item.Type = (DataType)Enum.Parse(typeof(DataType), childNode.Attributes["Type"].Value);
                    }
                    if (childNode.Attributes["Access"] != null)
                    {
                        item.Access = (AccessMode)Enum.Parse(typeof(AccessMode), childNode.Attributes["Access"].Value);
                    }
                    items.Add(item.DataID, item);
                }
            }
        }

        [Browsable(true), ReadOnly(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Data type")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        public DataType Type
        {
            get;
            set;
        }

        [Browsable(true), ReadOnly(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Data ID")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
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
