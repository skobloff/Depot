using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    class DepotDataDateTime : DepotData
    {
        DateTime soul;

        internal DepotDataDateTime(DateTime data)
        {
            soul = data;
        }

        internal override DataTypes DataType
        {
            get
            {
                return DataTypes.DateTime;
            }
        }

        internal override int Lenght
        {
            get
            {
                return 8;
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
            return BitConverter.GetBytes(soul.ToBinary());
        }
    }
}
