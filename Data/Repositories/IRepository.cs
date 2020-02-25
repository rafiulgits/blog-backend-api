using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data
{
    internal interface IRepository<IdType, EntityType>
    {
        Task<EntityType> Get(IdType id);
        Task<EntityType> Add(EntityType entity);
        Task<EntityType> Update(EntityType entity);
        Task<EntityType> Delete(EntityType entity);
        IQueryable<EntityType> GetQueryableHandler();
    }
}