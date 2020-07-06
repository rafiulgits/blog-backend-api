using Blogger.Util;

namespace Blogger.Services
{
    public class DataDependentService<DataType>
    {
        public DataDependentResult<DataType> Result { set; get; }

        public DataDependentService()
        {
            Result = new DataDependentResult<DataType>();
            Result.IsValid = false;
            Result.Error = new Data.Dto.ErrorDto();
        }
    }
}
