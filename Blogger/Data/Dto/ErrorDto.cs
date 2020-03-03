using System;
using System.Collections.Generic;

namespace Blogger.Data.Dto
{
    public class ErrorDto : Dictionary<string, List<string>>
    {
        public ErrorDto()
        {
/*            this = new Dictionary<string, List<string>>();*/
        }

        public ErrorDto Append(string field, string message)
        {
            if(this.ContainsKey(field))
            {
                this[field].Add(message);
            }
            else
            {
                List<string> messages = new List<string>();
                messages.Add(message);
                this.Add(field, messages);
            }
            return this;
        }
    }
}
