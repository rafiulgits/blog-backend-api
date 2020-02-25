using System.Collections.Generic;

namespace Blogger.Data
{
    internal interface IRepository<IdType, EntityType>
    {
        EntityType Get(IdType id);
        List<EntityType> GetAll();
        EntityType Add(EntityType entity);
        EntityType Update(EntityType entity);
        EntityType Delete(IdType id);
    }
}