using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WarrAttachmentManagementService.Application.Interfaces.Persistence
{
    public interface ILineItemAttachmentTypeRepository
        : IRepositoryBase<Domain.Entities.LineItemAttachmentType>
    {
        Task<ICollection<Domain.Entities.LineItemAttachmentType>> GetAllTypesAsync(
            CancellationToken cancellationToken);

        IQueryable<Domain.Entities.LineItemAttachmentType> QueryResultAsync(
            Expression<Func<Domain.Entities.LineItemAttachmentType, bool>> expression,
            bool ignoreQueryFilters = false);
    }
}
