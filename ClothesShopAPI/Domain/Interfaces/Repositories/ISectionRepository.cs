using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ISectionRepository
{
    public Task<Section> GetSectionById(int id);

    public Task<Section> GetSectionByName(string sectionName);

    public Task<IEnumerable<Section>> GetSections();

    public Task<Section> AddSection(Section section);

    public Task UpdateSection(Section section);

    public Task RemoveSection(Section section);

    public Task Save();
}
