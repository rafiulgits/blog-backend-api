using Blogger.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Util
{
    public class DataDependentResult<DataType>
    {
        public ErrorDto Error { set; get; }
        public bool IsValid { set; get; }
        public DataType Data { set; get; }
    }
}
