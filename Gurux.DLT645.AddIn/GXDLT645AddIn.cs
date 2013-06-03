using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device.Editor;
using Gurux.Device;
using System.Windows.Forms;
using Gurux.Common;

namespace Gurux.DLT645.AddIn
{
	[GXCommunicationAttribute(typeof(GXDLT645PacketHandler), typeof(GXDLT645PacketParser))]
	public class GXDLT645AddIn : GXProtocolAddIn
    {
        public GXDLT645AddIn()
			: base("DLT 645-2007", false, true, false)
		{
			base.WizardAvailable = VisibilityItems.None;
		}

		public override VisibilityItems ItemVisibility
		{
			get
			{
				return VisibilityItems.Categories | VisibilityItems.Tables;
			}
		}

		public override Functionalities GetFunctionalities(object target)
		{
			if (target is GXCategoryCollection)
			{
				return Functionalities.Add;
			}
			else if (target is GXTableCollection)
			{
				return Functionalities.Add;
			}
			else if (target is GXCategory)
			{
				return Functionalities.Add;
			}
			else
			{
				return Functionalities.Remove | Functionalities.Edit;
			}
		}

		public override Type[] GetTableTypes(object parent)
		{
			return new Type[] { typeof(GXDLT645Table) };
		}

		public override Type GetDeviceType()
		{
			return typeof(GXDLT645Device);
		}

		public override Type[] GetPropertyTypes(object parent)
		{
			return new Type[] { typeof(GXDLT645Property) };
		}

        public override void ModifyWizardPages(object source, GXPropertyPageType type, System.Collections.Generic.List<Control> pages)
        {
            if (type == GXPropertyPageType.Device)
            {
                pages.Insert(1, new GXDLT645DeviceWizardDlg());
            }
            else if (type == GXPropertyPageType.Property ||
                type == GXPropertyPageType.Table)
            {
                //Remove default pages.
                pages.Clear();
                pages.Add(new AddressDlg());
            }
            else if (type == GXPropertyPageType.Import)
            {
                pages.Insert(1, new DeviceImportForm());
            }
        }

        public override void ImportFromDevice(Control[] addinPages, GXDevice device, Gurux.Common.IGXMedia media)
        {            			
			media.Eop = device.GXClient.Eop;			
            GXDLT645Device dev = device as GXDLT645Device;
            dev.Parser.IgnoreFrame = false;
            Dictionary<ulong, object> items = GXDLT645Property.ReadDataID();
            GXCategory cat = device.Categories.Find("Default");
            if (cat == null)
            {
                cat = new GXCategory("Default");
                device.Categories.Add(cat);
            }
            media.Open();
            int count = 0;
            foreach (var it in items)
            {
                Progress(++count, items.Count);
                byte[] data = dev.Parser.ReadValue(it.Key);
                lock (media.Synchronous)
                {					
                    media.Send(data, null);
                    ReceiveParameters<byte[]> p = new ReceiveParameters<byte[]>()
                    {
                        Eop = media.Eop,
                        WaitTime = device.WaitTime
                    };
                    bool compleate = false;
                    try
                    {
                        while (!(compleate = dev.Parser.IsPacketComplete(it.Key, p.Reply)))
                        {
                            if (!media.Receive<byte[]>(p))
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace(ex.Message + Environment.NewLine);
                    }
                    // If data is not received or error has occurred.
                    if (!compleate || dev.Parser.IsError(p.Reply))
                    {
                        continue;
                    }
                    GXDLT645Data d = it.Value as GXDLT645Data;
                    if (d != null)
                    {
                        Trace(it.Key + " " + d.Name + Environment.NewLine);
                        cat.Properties.Add(new GXDLT645Property(it.Key, d.Name, d.Type, d.Access));
                    }
                    else
                    {
                        GXDLT645TableTemplate t = it.Value as GXDLT645TableTemplate;
                        Trace(it.Key + " " + t.Name + Environment.NewLine);
                        GXDLT645Table table = new GXDLT645Table();
                        table.Name = t.Name;
                        table.DataID = it.Key;
                        foreach (GXDLT645Data col in t.Columns)
                        {
                            table.Columns.Add(new GXDLT645Property(it.Key, col.Name, col.Type, col.Access));
                        }
                        device.Tables.Add(table);
                    }
                }
            }            
        }       
	}
}
