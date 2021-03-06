﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    class DepotDataReal : DepotData
    {
        double soul;

        internal DepotDataReal(double data)
        {
            soul = data;
        }

        internal override DataTypes DataType
        {
            get
            {
                return DataTypes.Real;
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
            return BitConverter.GetBytes(soul);
        }
    }
}
