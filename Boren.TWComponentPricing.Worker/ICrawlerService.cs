using Boren.TWComponentPricing.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boren.TWComponentPricing.Worker
{
    public interface ICrawlerService
    {
        Task<IList<Product>> GetAsync();

        Task SetAsync(IList<Product> products);  
        
        Task GetV2Async();
    }
}
