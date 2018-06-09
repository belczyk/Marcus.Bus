using System;
using System.Threading.Tasks;

namespace Marcus.Bus
{
    public interface IDenormalizer : IHandler
    {
        Task ClaerView(Guid tenantId);
    }
}