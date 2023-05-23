using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminArchive.Model
{
    partial class StorageUnit
    {
        public string FullNumber
        {
            get
            {
                return Number + "" + Literal;
            }
        }
    }
}
