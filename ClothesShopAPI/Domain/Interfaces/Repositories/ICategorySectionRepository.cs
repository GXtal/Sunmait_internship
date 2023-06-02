using Domain.Entities;

namespace Domain.Interfaces.Repositories;
public interface ICategorySectionRepository
{
    public Task<IEnumerable<CategorySection>> GetCategorySectionsBySection(Section section);
}
