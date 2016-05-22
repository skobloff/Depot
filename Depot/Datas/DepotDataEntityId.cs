using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    class DepotDataEntityId : DepotData
    {
        EntityId soul;

        internal DepotDataEntityId(EntityId data)
        {
            soul = data;
        }

        internal override DataTypes DataType
        {
            get
            {
                return DataTypes.EntityId;
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
                return soul.data.ToString();
            }
        }

        internal override byte[] GetBytes()
        {
            return BitConverter.GetBytes(soul.data);
        }
    }
}
