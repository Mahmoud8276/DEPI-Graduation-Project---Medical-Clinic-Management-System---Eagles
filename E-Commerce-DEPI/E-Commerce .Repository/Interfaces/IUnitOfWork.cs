using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_.Repository.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IProductRepository ProductRepository { get; }
        Task<int> CompleteAsync();
    }
}
