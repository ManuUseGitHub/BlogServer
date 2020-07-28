using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRecordKind
    {
        public enum RecordKind
        {
            DEFINED,
            OLDEST,
            UPTODATE
        }
    }
}
