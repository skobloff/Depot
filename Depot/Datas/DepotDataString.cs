using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    class DepotDataString : DepotData
    {
        string soul;
        Encoding u16LE = Encoding.Unicode;

        internal DepotDataString(string data)
        {
            soul = data;
        }

        internal override DataTypes DataType
        {
            get
            {
                return DataTypes.String;
            }
        }

        internal override int Lenght
        {
            get
            {
                return u16LE.GetByteCount(soul) + 2;
            }
        }

        internal override string String
        {
            get
            {
                return soul;
            }
        }

        internal override byte[] GetBytes()
        {
            byte[] ret = new byte[soul.Length + 2];
            int i = 0;
            foreach (byte b in u16LE.GetBytes(soul))
            {
                ret[i] = b;
                i++;
            }
            return ret;
        }
    }

    internal class DepotDataMemo : DepotDataString
    {
        internal DepotDataMemo(string data) : base(data) { }

        internal override Boolean IsIndexsable
        {
            get
            {
                return false;
            }
        }
    }
}
