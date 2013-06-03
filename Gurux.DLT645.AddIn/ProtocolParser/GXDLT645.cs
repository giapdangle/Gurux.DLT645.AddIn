using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Gurux.DLT645
{
    public class GXDLT645
    {
        string m_OperatorCode, m_MeterPassword, m_RelayControlPassword, m_LowClassPassword;
        enum Function
        {
            Read = 1,
            ReadSubsequent = 2,
            ReRead = 3,
            Write = 4,
            CorrectTimeByBroadcast = 8,
            WriteDeviceAddress = 10,
            ChangeCommunicationSpeed = 12,
            ChangePassword = 15,
            ClearMaximumDemand = 16
        }

        /// <summary>
        /// Make control code.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="send"></param>
        /// <param name="subSequent"></param>
        /// <returns></returns>
        byte MakeControlCode(Function func, bool send, bool subSequent)
        {
            byte value = (byte)((byte)func & 0x1F);
            if (!send)
            {
                value += 0x80;
            }

            if (subSequent)
            {
                value += 0x20;
            }
            return value;
        }

        /// <summary>
        /// Device OperatorCode.
        /// </summary>
        /// <remarks>
        /// Operator Code maximum length is four charachters.
        /// </remarks>
        public string OperatorCode
        {
            get
            {
                return m_OperatorCode;
            }
            set
            {
                if (m_OperatorCode != null && m_OperatorCode.Length > 4)
                {
                    throw new ArgumentException("Operator Code is maximum four charachters.");
                }
                m_OperatorCode = value;
            }
        }

        /// <summary>
        /// Collector address.
        /// </summary>
        public ulong CollectorAddress
        {
            get;
            set;
        }

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

        /// <summary>
        /// Password of the meter.
        /// </summary>
        /// <remarks>
        /// Maimum size is eight charachters.
        /// </remarks>
        public string MeterPassword
        {
            get
            {
                return m_MeterPassword;
            }
            set
            {
                if (value != null && value.Length > 8)
                {
                    throw new ArgumentException("Length of the meter password is maximum eight charachters.");
                }
                m_MeterPassword = value;
            }
        }

        /// <summary>
        /// Password of the relay control.
        /// </summary>
        /// <remarks>
        /// Maimum size is eight charachters.
        /// </remarks>
        public string RelayControlPassword
        {
            get
            {
                return m_RelayControlPassword;
            }
            set
            {
                if (value != null && value.Length > 8)
                {
                    throw new ArgumentException("Length of the relay control password is maximum eight charachters.");
                }
                m_RelayControlPassword = value;
            }
        }

        /// <summary>
        /// Password of the low class.
        /// </summary>
        /// <remarks>
        /// Maimum size is eight charachters.
        /// </remarks>
        public string LowClassPassword
        {
            get
            {
                return m_LowClassPassword;
            }
            set
            {
                if (value != null && value.Length > 8)
                {
                    throw new ArgumentException("Length of the low class password is maximum eight charachters.");
                }
                m_LowClassPassword = value;
            }
        }

        const byte Bop = 0x68;
        const byte Eop = 0x16;

        /// <summary>
        /// Are BOP, EOP and Checksum added to data.
        /// </summary>
        [DefaultValue(false)]
        public bool IgnoreFrame
        {
            get;
            set;
        }

        public ulong DeviceAddress
        {
            get;
            set;
        }

        void AddFrame(List<byte> data)
        {
            if (!IgnoreFrame)
            {
                int pos = 0;
                if (FrontLeadingBytes != null)
                {
                    pos = FrontLeadingBytes.Length;
                }
                data.Insert(pos, Bop);
                byte crc = 0;
                for (int a = pos; a != data.Count; ++a)
                {
                    crc = (byte)((crc + data[a]) & 0xFF);
                }
                data.Add(crc);
                data.Add(Eop);
            }
        }

        void AddDeviceAddress(List<byte> data)
        {
            int add = 0;
            if (CollectorAddress != 0)
            {
                add = 0x33;
            }
            string str = string.Format("{0:000000000000}", DeviceAddress);
            for (int pos = str.Length; pos > 0; pos -= 2)
            {
                string tmp = str.Substring(pos - 2, 2);
                data.Add((byte)(Convert.ToInt32(tmp, 16) + add));
            }
        }

        void AddDataID(List<byte> data, ulong dataID)
        {           
            string str = string.Format("{0:x8}", dataID);
            for (int pos = str.Length; pos > 0; pos -= 2)
            {
                string tmp = str.Substring(pos - 2, 2);
                data.Add((byte)(Convert.ToInt32(tmp, 16) + 0x33));
            }
        }


        byte[] MakePacket(Function func, ulong dataID, object value)
        {
            List<byte> data = new List<byte>(11);
            if (FrontLeadingBytes != null)
            {
                data.AddRange(FrontLeadingBytes);
            }
            int LenPos = 0;
            if (CollectorAddress != 0)
            {
                data.Add((byte)(CollectorAddress & 0xff));
                data.Add((byte)((CollectorAddress >> 8) & 0xff));
                data.Add((byte)((CollectorAddress >> 16) & 0xff));
                data.Add((byte)((CollectorAddress >> 24) & 0xff));
                data.Add((byte)((CollectorAddress >> 32) & 0xff));
                data.Add((byte)((CollectorAddress >> 40) & 0xff));
                data.Add(Bop);
                // Add Control Code.
                data.Add(MakeControlCode(func, true, false));
                LenPos = data.Count;
                AddDataID(data, dataID);
                AddDeviceAddress(data);
            }
            else
            {
                AddDeviceAddress(data);
                data.Add(Bop);
                // Add Control Code.
                data.Add(MakeControlCode(func, true, false));
                LenPos = data.Count;
                AddDataID(data, dataID);
            }
            if ((func & Function.Write) != 0)
            {
                //Password is always eight digits.
                string tmp = "";
                for (int pos = MeterPassword.Length; pos != 8; ++pos)
                {
                    tmp += "0" + tmp;
                }
                tmp += MeterPassword;
                for (int pos = tmp.Length; pos > 0; pos -= 2)
                {
                    string tmp2 = tmp.Substring(pos - 2, 2);
                    data.Add((byte)(Convert.ToInt32(tmp2, 16) + 0x33));
                }
                //Operator code is always found digits.
                tmp = "";
                for (int a = 0; a != 4 - OperatorCode.Length; ++a)
                {
                    tmp += "0";
                }
                tmp += OperatorCode;
                char[] items = tmp.ToCharArray();
                Array.Reverse(items);
                foreach (byte it in items)
                {
                    data.Add((byte)(it + 0x33));
                }
            }

            if (value != null)
            {
                if (value is DateTime)
                {
                    DateTime dt = (DateTime)value;
                    data.Add((byte)((int)dt.DayOfWeek + 0x33));
                    data.Add((byte)(dt.Day + 0x33));
                    data.Add((byte)(dt.Month + 0x33));
                    data.Add((byte)((dt.Year - 2000 + 6 + 0x33)));
                }
            }
            data.Insert(LenPos, (byte)(data.Count - LenPos));
            AddFrame(data);
            return data.ToArray();
        }

        /// <summary>
        /// Generates read message.
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        /// <seealso cref="GetValue"/>
        public byte[] ReadValue(ulong dataID)
        {
            return MakePacket(Function.Read | Function.ClearMaximumDemand, dataID, null);
        }

        /// <summary>
        /// Generates Write message.
        /// </summary>
        /// <param name="dataID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] WriteValue(ulong dataID, object value)
        {
            return MakePacket(Function.Write | Function.ClearMaximumDemand, dataID, value);
        }

        /// <summary>
        /// Check is there more data available.
        /// </summary>
        /// <param name="dataID"></param>
        /// <param name="reply"></param>
        /// <returns></returns>
        public bool IsMoreDataAvailable(ulong dataID, byte[] reply)
        {
            int index = -1;
            int StartIndex = -1;
            int cc = GetControlCode(reply, ref index, out StartIndex);
            if ((cc & 0x80) != 0x80)
            {
                throw new Exception("Parse Failed. Not a reply packet.");
            }
            return (cc & 0x20) == 0x20;
        }

        /// <summary>
        /// Generates an acknowledgment message, with which the server is informed to 
        /// send next packets.
        /// </summary>
        /// <returns></returns>
        public byte[] ReceiverReady()
        {
            throw new NotImplementedException();
        }

        byte GetControlCode(byte[] reply, ref int index, out int StartIndex)
        {
            if (IgnoreFrame)
            {
                index = StartIndex = 0;
            }
            else
            {
                StartIndex = -1;
                //Find Bop.
                for (int pos = 0; pos != reply.Length; ++pos)
                {
                    if (reply[pos] == Bop)
                    {
                        StartIndex = pos;
                        index = pos + 1;
                        break;
                    }
                }
            }
            if (StartIndex == -1)
            {
                throw new OutOfMemoryException("BOP Not found.");
            }
            //Check that device addresses matchs.
            ulong address = 0;
            for (int pos = 0; pos != 6; ++pos)
            {
                byte val = (byte)reply[pos + index];
                int tmp = (val >> 4) * 10 | val & 0xF;
                address += (ulong)(tmp * Math.Pow(100, pos));
            }
            index += 6;
            if (address != DeviceAddress)
            {
                throw new Exception("Parse Failed. Invalid Address.");
            }
            if (reply[index++] != 0x68)
            {
                throw new Exception("Parse Failed. Invalid data.");
            }
            //Get Control Code.
            return reply[index++];
        }

        public bool IsPacketComplete(ulong dataID, byte[] reply)
        {
            try
            {
                if (reply == null)
                {
                    return false;
                }
                GetValue(dataID, reply, typeof(byte[]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsError(byte[] reply)
        {
            int index = -1;
            int StartIndex = -1;
            int cc = GetControlCode(reply, ref index, out StartIndex);
            return (cc & 0x40) == 0x40;
        }

        /// <summary>
        /// Removes the DL/T 645 header from the packet, and returns payload data only.
        /// </summary>
        /// <param name="packet">The received packet, from the device, as byte array.</param>
        /// <param name="data">The exported data.</param>
        /// <returns>Received Data</returns>
        public bool GetDataFromPacket(ulong dataID, byte[] reply, ref byte[] allData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parse value.
        /// </summary>
        /// <param name="dataID"></param>
        /// <param name="reply"></param>
        /// <returns></returns>
        /// <seealso cref="ReadValue"/>
        public object GetValue(ulong dataID, byte[] reply, Type type)
        {
            int index = -1;
            int StartIndex = -1;
            int cc = GetControlCode(reply, ref index, out StartIndex);
            if ((cc & 0x80) != 0x80)
            {
                throw new Exception("Parse Failed. Not a reply packet.");
            }
            int len = reply[index++];
            if (reply.Length < index + len + (int)(IgnoreFrame ? 0 : 1))
            {
                throw new OutOfMemoryException();
            }
            int crcPos = index + len;
            if (!IgnoreFrame)
            {
                //Check CRC.                
                byte readCrc = reply[crcPos];
                byte countCrc = 0;
                for (int pos = StartIndex; pos != index + len; ++pos)
                {
                    countCrc = (byte)((countCrc + reply[pos]) & 0xFF);
                }
                if (countCrc != readCrc)
                {
                    throw new Exception("Parse Failed. CRC do not match.");
                }
                if (reply[index + len + 1] != 0x16)
                {
                    throw new Exception("Parse Failed. EOP not found.");
                }
            }
            if (index + 2 < reply.Length)
            {
                ulong address = 0;
                for (int pos = 0; pos != 4; ++pos)
                {
                    byte val = (byte)(reply[index++] - 0x33);
                    address += (ulong)(val << (8 * pos));
                }
                if (dataID != address)
                {
                    throw new Exception("Parse Failed. data identification do not match.");
                }
                byte[] buff = new byte[crcPos - index];
                int a = 0;
                while (index != crcPos)
                {
                    buff[a++] = (byte)(reply[index++] - 0x33);
                }
                int cnt;                 
                return Gurux.Shared.GXCommon.ByteArrayToObject(buff, type, out cnt);
            }
            return null;
        }
    }
}