using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.DLT645.AddIn
{
    /// <inheritdoc cref="IGXPacketParser"/>
	class GXDLT645PacketParser : Gurux.Communication.IGXPacketParser
	{
		#region IGXPacketParser Members

        /// <inheritdoc cref="IGXPacketParser.Load"/>
		public void Load(object sender)
		{		
		}

        public void Connect(object sender)
        {
        }

        public void Disconnect(object sender)
        {

        }

        /// <inheritdoc cref="IGXPacketParser.BeforeSend"/>
		public void BeforeSend(object sender, Gurux.Communication.GXPacket packet)
		{		
		}

        /// <inheritdoc cref="IGXPacketParser.IsReplyPacket"/>
		public void IsReplyPacket(object sender, Gurux.Communication.GXReplyPacketEventArgs e)
		{		
		}

        /// <inheritdoc cref="IGXPacketParser.AcceptNotify"/>
		public void AcceptNotify(object sender, Gurux.Communication.GXReplyPacketEventArgs e)
		{		
		}

        /// <inheritdoc cref="IGXPacketParser.CountChecksum"/>
		public void CountChecksum(object sender, Gurux.Communication.GXChecksumEventArgs e)
		{			
		}

        /// <inheritdoc cref="IGXPacketParser.ReceiveData"/>
		public void ReceiveData(object sender, Gurux.Communication.GXReceiveDataEventArgs e)
		{		
		}

        /// <inheritdoc cref="IGXPacketParser.Received"/>
		public void Received(object sender, Gurux.Communication.GXReceivedPacketEventArgs e)
		{		
		}

        /// <inheritdoc cref="IGXPacketParser.ParsePacketFromData"/>
		public void ParsePacketFromData(object sender, Gurux.Communication.GXParsePacketEventArgs e)
		{		
		}

        /// <inheritdoc cref="IGXPacketParser.Unload"/>
		public void Unload(object sender)
		{			
		}

        /// <inheritdoc cref="IGXPacketParser.VerifyPacket"/>
        public void VerifyPacket(object sender, Gurux.Communication.GXVerifyPacketEventArgs e)
        {

        }

        /// <summary>
        /// Set media Eop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="media"></param>
        public void InitializeMedia(object sender, Gurux.Common.IGXMedia media)
        {
            media.Eop = (byte)0x16;
        }
		#endregion
	}
}
