using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZooDb.Exceptions;

public class SectionService
{
    private readonly AppDbContext _context;

    public SectionService(AppDbContext context)
    {
        _context = context;
    }

    //Get all sections
    public async Task<List<SectionDTO>> GetAllSections()
    {
        var sections = await _context.Sections.ToListAsync();
        if(sections == null || sections.Count == 0)
        {
            throw new NotFoundException("Could not find any sections");
        }

        var sectionDTOs = sections.Select(s=> new SectionDTO
        {
            Name=s.Name
        }).ToList();

        return sectionDTOs;
    }

    //Get one section with animals that belong to it
    public async Task<SectionDTO> GetSectionById(int id)
    {
        var section = await _context.Sections.FindAsync(id) ?? throw new NotFoundException("Could not find the section");
        var sectionDTO = new SectionDTO
        {
            Name = section.Name,
            Animals = section.Animals.Select(a=>new AnimalDTO
            {
                Name=a.Name,
                Type=a.Type,
                Age = a.Age
            }).ToList()
        };

        return sectionDTO;
    }

    //Create a section
    public async Task PostSection(SectionDTO sectionDTO)
    {
        if(sectionDTO.Name == "" || sectionDTO.Name == null)
        {
            throw new InvalidInputException("The section has to have a name");
        }
        var section = new Section()
        {
            Name = sectionDTO.Name
        };
        _context.Sections.Add(section);
        await _context.SaveChangesAsync();
    }

    //Add animals to a section
    public async Task AddAnimalToSection(int id, AnimalDTO animalDTO)
    {
        var section = await _context.Sections.FindAsync(id) ?? throw new NotFoundException("Could not find the section");
        if(animalDTO.Name == "" || animalDTO == null)
        {
            throw new InvalidInputException("Animal needs a name");
        }
        var animal = new Animal()
        {
            Name = animalDTO.Name,
            Type = animalDTO.Type,
            Age = animalDTO.Age
        };

        section.Animals.Add(animal);
        await _context.SaveChangesAsync();
    }


    //Delete animal from a section
    public async Task RemoveAnimalSection(int secId, int animalId)
    {
        var section = await _context.Sections.FindAsync(secId) ?? throw new NotFoundException("Could not find the section");
        var animal = await _context.Animals.FindAsync(animalId) ?? throw new NotFoundException("Could not find the animal");
        section.Animals.Remove(animal);
        await _context.SaveChangesAsync();
    }

    //Uppdate details of the section
    public async Task UpdateSection(int id, SectionDTO sectionDTO)
    {
        var section = await _context.Sections.FindAsync(id) ?? throw new NotFoundException("Could not find the section");
        section.Name = sectionDTO.Name;
        await _context.SaveChangesAsync();
    }

    //Delete section
    public async Task DeleteSection(int id)
    {
        var section = await _context.Sections.FindAsync(id) ?? throw new NotFoundException("Could not find the section");
        _context.Sections.Remove(section);
        await _context.SaveChangesAsync();
    }
}