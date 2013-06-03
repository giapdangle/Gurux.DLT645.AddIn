using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device;

namespace Gurux.DLT645.AddIn
{
	class GXDLT645PacketHandler : Gurux.Device.IGXPacketHandler
	{
        ulong RowCount = 0;
        ulong RowIndex = 0;
        Dictionary<ulong, object> m_Items;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLT645PacketHandler()
        {            
        }

		#region IGXPacketHandler Members

        /// <inheritdoc cref="IGXPacketHandler.Parent"/>
        public object Parent
        {
            get;
            set;
        }

        /// <inheritdoc cref="IGXPacketHandler.Connect"/>
        public void Connect(object sender)
        {
        }

        /// <inheritdoc cref="IGXPacketHandler.Disconnect"/>
        public void Disconnect(object sender)
        {

        }

        GXDLT645Device Device
        {
            get
            {
                return Parent as GXDLT645Device;
            }
        }

        /// <inheritdoc cref="IGXPacketHandler.ExecuteSendCommand"/>
        public void ExecuteSendCommand(object sender, string command, Gurux.Communication.GXPacket packet)
        {
            ulong id = 0;
            if (command == "ReadData")
            {
                GXDLT645Property prop = sender as GXDLT645Property;
                id = prop.DataID;                
            }
            else if (command == "ReadTableRowsCount")
            {				
                if (m_Items == null)
                {
                    m_Items = GXDLT645Property.ReadDataID();
                }
                RowIndex = RowCount = 0;                
                GXDLT645Table table = sender as GXDLT645Table;
				table.ClearRows();
                id = table.DataID;
            }
            else if (command == "ReadTable" || command == "ReadTableNext")
            {                
                GXDLT645Table table = sender as GXDLT645Table;
                ++RowIndex;
                id = table.DataID + RowIndex;
            }
            else
            {
                throw new Exception("ExecuteCommand failed. Unknown command: " + command);
            }
            packet.AppendData(Device.Parser.ReadValue(id));
        }

        /// <inheritdoc cref="IGXPacketHandler.ExecuteParseCommand"/>
		public void ExecuteParseCommand(object sender, string command, Gurux.Communication.GXPacket[] packets)
		{            
            if (command == "ReadDataReply")
            {
                GXDLT645Property prop = sender as GXDLT645Property;
                byte[] data = (byte[])packets[0].ExtractData(typeof(byte[]), 0, -1);
                prop.SetValue(Device.Parser.GetValue(prop.DataID, data, typeof(byte[])), false, PropertyStates.ValueChangedByDevice);                
            }
            else if (command == "ReadTableRowsCountReply")
            {
                GXDLT645Table table = sender as GXDLT645Table;
                byte[] data = (byte[])packets[0].ExtractData(typeof(byte[]), 0, -1);
                data = (byte[])Device.Parser.GetValue(table.DataID, data, typeof(byte[]));
                for (int pos = 0; pos != data.Length; ++pos)
                {
                    byte val = (byte)data[pos];
                    RowCount += (ulong)(val << (8 * pos));                    
                }
            }
            else if (command == "UpdateTable")
            {
                //Rows are already updated.                
            }                
            else
            {
                throw new Exception("ExecuteParseCommand failed. Unknown command: " + command);
            }			
		}

        /// <inheritdoc cref="IGXPacketHandler.IsTransactionComplete"/>
		public bool IsTransactionComplete(object sender, string command, Gurux.Communication.GXPacket packet)
		{
            if (command == "IsAllTableDataReceived")
            {
                GXDLT645Table table = sender as GXDLT645Table;
                List<object[]> rows = new List<object[]>(1);
                byte[] data = (byte[])packet.ExtractData(typeof(byte[]), 0, -1);
                if (Device.Parser.IsError(data))
                {
                    return true;
                }
                List<byte> reply = new List<byte>();
	            reply.AddRange((byte[])Device.Parser.GetValue(table.DataID + RowIndex, data, typeof(byte[])));
                List<object> values = new List<object>();
                int size;
                GXDLT645TableTemplate t = m_Items[table.DataID] as GXDLT645TableTemplate;
                for (int pos = 0; pos != table.Columns.Count; ++pos)
                {
                    object value = GetValue(reply.ToArray(), t.Columns[pos].Type, out size);
                    values.Add(value);
                    reply.RemoveRange(0, size);
                }
                if (values.Count != table.Columns.Count)
                {
                    throw new Exception("Table read failed. Columns count do not match.");
                }
                rows.Add(values.ToArray());
                table.AddRows((int)(RowIndex - 1), rows, true);
                return RowIndex == RowCount;
            }                
			throw new Exception("IsTransactionComplete failed. Unknown command: " + command);
		}

        /// <inheritdoc cref="IGXPacketHandler.UIValueToDeviceValue"/>
		public object UIValueToDeviceValue(Gurux.Device.GXProperty sender, object value)
		{
            return value;			
		}

        object GetValue(byte[] data, DataType type, out int size)
        {
            Array.Reverse(data);
            string value = Gurux.Communication.Common.GXConverter.FromBCD(data);            
            string str;
            int year, month, day, hour, minute, second;
            switch (type)
            {
                case DataType.UInt8:
                    size = 1;
                    return Convert.ToByte(value);
                case DataType.UInt16:
                    size = 2;
                    return Convert.ToUInt16(value);
                case DataType.UInt24:
                    size = 3;
                    return Convert.ToUInt32(value);
                case DataType.UInt32:
                    size = 4;
                    return Convert.ToUInt32(value);
                case DataType.UInt48:
                    size = 6;
                    return Convert.ToUInt64(value);
                case DataType.UInt64:
                    size = 8;
                    return Convert.ToUInt64(value);
                case DataType.Int8:
                    size = 1;
                    return Convert.ToSByte(value);
                case DataType.Int16:
                    size = 2;
                    return Convert.ToInt16(value);
                case DataType.Int32:
                    size = 4;
                    return Convert.ToInt32(value);
                case DataType.Int64:
                    size = 8;
                    return Convert.ToInt64(value);
                case DataType.String:
                case DataType.BCD:
                    size = data.Length;
                    return value;
                case DataType.Time:
                    size = 3;
                    str = value.ToString();
                    if (str.Length < 6)
                    {
                        throw new ArgumentException("Invalid time string");
                    }
                    hour = Convert.ToInt16(str.Substring(0, 2));
                    minute = Convert.ToInt16(str.Substring(2, 2));
                    second = Convert.ToInt16(str.Substring(4, 2));
                    return new DateTime(2000, 1, 1, hour, minute, second).ToLongTimeString();
                case DataType.Date:
                    str = value.ToString();
                    size = 4;
                    if (str.Length < 8)
                    {
                        throw new ArgumentException("Invalid date string");
                    }
                    year = 2000 + Convert.ToInt16(str.Substring(0, 2));
                    month = Convert.ToInt16(str.Substring(2, 2));
                    day = Convert.ToInt16(str.Substring(4, 2));
                    return new DateTime(year, month, day);
                case DataType.DateTime:
                    str = value.ToString();
                    size = 6;
                    if (str.Length < 12)
                    {
                        throw new ArgumentException("Invalid date string");
                    }
                    year = 2000 + Convert.ToInt16(str.Substring(0, 2));
                    month = Convert.ToInt16(str.Substring(2, 2));
                    day = Convert.ToInt16(str.Substring(4, 2));
                    hour = Convert.ToInt16(str.Substring(6, 2));
                    minute = Convert.ToInt16(str.Substring(8, 2));
                    second = Convert.ToInt16(str.Substring(10, 2));
                    return new DateTime(year, month, day, hour, minute, second);
            }
            throw new ArgumentOutOfRangeException("type");
        }

        /// <inheritdoc cref="IGXPacketHandler.DeviceValueToUIValue"/>
		public object DeviceValueToUIValue(Gurux.Device.GXProperty sender, object value)
		{
            GXDLT645Property prop = sender as GXDLT645Property;
            int size;
            value = GetValue((byte[])value, prop.Type, out size);
            return value;
		}
		#endregion
	}
}
