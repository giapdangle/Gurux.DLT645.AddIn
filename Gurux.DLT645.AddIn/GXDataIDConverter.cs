using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Gurux.DLT645.AddIn
{
    class GXDataIDConverter : StringConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {                
                Dictionary<ulong, object> items = GXDLT645Property.ReadDataID();
                ulong tmp = Convert.ToUInt64(value);
                if (items.ContainsKey(tmp))
                {
                    object target = items[tmp];
                    GXDLT645Data d = target as GXDLT645Data;
                    if (d != null)
                    {
                        return string.Format("{0:00000000} {1}", value, d.Name);
                    }
                    GXDLT645TableTemplate t = target as GXDLT645TableTemplate;
                    return string.Format("{0:00000000} {1}", value, t.Name);
                }
                else
                {
                    return string.Format("{0:00000000}", value);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
