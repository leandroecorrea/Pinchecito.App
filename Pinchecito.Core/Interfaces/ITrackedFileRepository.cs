using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinchecito.Core.Interfaces
{
    public interface ITrackedFileRepository
    {
        public Task<Result<TrackedFile>> GetTrackedFile(int id);
    }
}
