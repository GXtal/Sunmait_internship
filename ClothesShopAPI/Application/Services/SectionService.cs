using Application.Exceptions.Messages;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class SectionService : ISectionService
{
    private readonly ISectionRepository _sectionRepository;
    private readonly ICategorySectionRepository _categorySectionRepository;

    public SectionService(ISectionRepository sectionRepository, ICategorySectionRepository categorySectionRepository)
    {
        _sectionRepository = sectionRepository;
        _categorySectionRepository = categorySectionRepository;
    }
    public async Task AddSection(string newSectionName)
    {
        var existingSection = await _sectionRepository.GetSectionByName(newSectionName);
        if (existingSection != null)
        {
            throw new BadRequestException(String.Format(SectionExceptionsMessages.SectionNameExists, newSectionName));
        }

        var newSection = new Section { Name = newSectionName };
        await _sectionRepository.AddSection(newSection);
    }

    public async Task<Section> GetSection(int id)
    {
        var section = await _sectionRepository.GetSectionById(id);
        if (section == null)
        {
            throw new NotFoundException(String.Format(SectionExceptionsMessages.SectionNotFound, id));
        }
        return section;
    }

    public async Task<IEnumerable<Section>> GetSections()
    {
        var sections = await _sectionRepository.GetSections();
        return sections;
    }

    public async Task RemoveSection(int id)
    {
        var section = await _sectionRepository.GetSectionById(id);
        if (section == null)
        {
            throw new NotFoundException(String.Format(SectionExceptionsMessages.SectionNotFound, id));
        }

        var categorySections = await _categorySectionRepository.GetCategorySectionsBySection(section);
        if (categorySections.Count() > 0)
        {
            throw new BadRequestException(String.Format(SectionExceptionsMessages.SectionIsUsed, id));
        }

        await _sectionRepository.RemoveSection(section);
    }

    public async Task UpdateSection(int id, string newSectionName)
    {
        var section = await _sectionRepository.GetSectionById(id);
        if (section == null)
        {
            throw new NotFoundException(String.Format(SectionExceptionsMessages.SectionNotFound, id));
        }

        var existingSection = await _sectionRepository.GetSectionByName(newSectionName);
        if (existingSection != null)
        {
            throw new BadRequestException(String.Format(SectionExceptionsMessages.SectionNameExists, newSectionName));
        }

        section.Name = newSectionName;
        await _sectionRepository.UpdateSection(section);
    }
}
