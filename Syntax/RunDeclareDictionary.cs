﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Parse;

namespace Syntax
{
    class RunDeclareDictionary : RunBase
    {
        public RunDeclareDictionary (Match _match)
            : base (_match)
        { }

        protected override bool Verified()
        {
            return true;
        }

        protected override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
