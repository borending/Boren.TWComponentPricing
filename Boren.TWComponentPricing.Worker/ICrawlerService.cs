using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boren.TWComponentPricing.Worker
{
    public interface ICrawlerService
    {
        Task<IList<object>> GetAsync();
    }
}
