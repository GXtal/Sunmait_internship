using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ISectionService
{
    public Task<Section> GetSection(int id);

    public Task<IEnumerable<Section>> GetSections();

    public Task AddSection(string newSectionName);

    public Task RenameSection(int id, string newSectionName);

    public Task RemoveSection(int id);
}
