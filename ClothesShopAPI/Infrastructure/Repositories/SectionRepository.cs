using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SectionRepository : ISectionRepository
{
    private readonly ShopDbContext _dbContext;

    public SectionRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Section> AddSection(Section section)
    {
        _dbContext.Add(section);
        await Save();
        return section;
    }

    public async Task<Section> GetSectionById(int id)
    {
        var section = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == id);
        return section;
    }

    public async Task<Section> GetSectionByName(string sectionName)
    {
        var section = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Name == sectionName);
        return section;
    }

    public async Task<IEnumerable<Section>> GetSections()
    {
        var allSections = await _dbContext.Sections.ToListAsync();
        return allSections;
    }

    public async Task RemoveSection(Section section)
    {
        _dbContext.Sections.Remove(section);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateSection(Section section)
    {
        _dbContext.Entry(section).State = EntityState.Modified;
        await Save();
    }
}
