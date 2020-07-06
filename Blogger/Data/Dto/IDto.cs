using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data.Dto
{
    public interface IDto<ModelType>
    {
        ModelType GetPersistentObject();
    }
}
