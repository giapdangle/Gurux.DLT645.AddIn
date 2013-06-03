using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Gurux.Shared
{    
    class GXCommon
    {
        /// <summary>
        /// Convert object to byte array.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetAsByteArray(object value)
        {            
            if (value == null)
            {
                return new byte[0];
            }
            if (value is string)
            {
                return Encoding.UTF8.GetBytes((string)value);
            }
            int rawsize = 0;
            byte[] rawdata = null;
            GCHandle handle;
            if (value is Array)
            {
                Array arr = value as Array;
                if (arr.Length != 0)
                {
                    int valueSize = Marshal.SizeOf(arr.GetType().GetElementType());
                    rawsize = valueSize * arr.Length;
                    rawdata = new byte[rawsize];
                    handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);                    
                    long pos = handle.AddrOfPinnedObject().ToInt64();
                    foreach (object it in arr)
                    {
                        Marshal.StructureToPtr(it, new IntPtr(pos), false);
                        pos += valueSize;
                    }
                    handle.Free();
                    return rawdata;
                }
                return new byte[0];
            }

            rawsize = Marshal.SizeOf(value);
            rawdata = new byte[rawsize];
            handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
            handle.Free();
            return rawdata;            
        }

        /// <summary>
        /// Convert received byte stream to wanted object.
        /// </summary>
        /// <param name="byteArray">Bytes to parse.</param>
        /// <param name="type">Object type.</param>
        /// <param name="readBytes">Read byte count.</param>
        /// <returns></returns>
        public static object ByteArrayToObject(byte[] byteArray, Type type, out int readBytes)
        {
            readBytes = 0;
            object value = null;
            if (type == typeof(string))
            {
                return Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);                
            }
            if (type == null)
            {
                type = typeof(byte[]);
            }
            GCHandle handle;
            if (type.IsArray)
            {
                Type valueType = type.GetElementType();
                int valueSize = Marshal.SizeOf(valueType);
                int cnt = byteArray.Length / Marshal.SizeOf(valueType);
                Array arr = (Array)Activator.CreateInstance(type, cnt);
                handle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
                long start = handle.AddrOfPinnedObject().ToInt64();
                for (int pos = 0; pos != cnt; ++pos)
                {
                    arr.SetValue(Marshal.PtrToStructure(new IntPtr(start), valueType), pos);
                    start += valueSize;
                    readBytes += valueSize;
                }
                handle.Free();
                return arr;
            }
            handle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            value = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
            readBytes = Marshal.SizeOf(type);
            handle.Free();
            return value; 
        }
    }
}
