using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    class DepotDataGuid : DepotData
    {
        Guid soul;

        internal DepotDataGuid(Guid data)
        {
            soul = data;
        }

        internal override DataTypes DataType
        {
            get
            {
                return DataTypes.GUID;
            }
        }

        internal override int Lenght
        {
            get
            {
                return 16;
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
            return soul.ToByteArray();
        }
    }
}
