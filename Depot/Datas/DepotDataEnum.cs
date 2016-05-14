using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    internal class DepotDataEnum : DepotData
    {
        short soul;

        internal DepotDataEnum (short data)
        {
            soul = data;
        }

        internal override DataTypes DataType
        {
            get
            {
                return DataTypes.ENUM;
            }
        }

        internal override int Lenght
        {
            get
            {
                return 2;
            }
        }

        internal override string String
        {
            get
            {
                return soul.ToString();
            }
        }

        internal override byte[] GetBytes()
        {
            return BitConverter.GetBytes(soul);
        }
    }
}
