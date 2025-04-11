using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZooDb.Exceptions;

public class AnimalService
{
    private readonly AppDbContext _context;

    public AnimalService(AppDbContext context)
    {
        _context = context;
    }

    //Get all animals
    public async Task<List<AnimalDTO>> GetAllAnimals()
    {
        var animals = await _context.Animals.ToListAsync();

        if (animals == null || animals.Count == 0)
        {
            throw new NotFoundException("Could not find any animals");
        }
        

        var animalDTOs = animals.Select(a=> new AnimalDTO
        {
            Name=a.Name,
            Type=a.Type,
            Age = a.Age
        }).ToList();
        return animalDTOs;
    }

    //Post an animal
    public async Task<AnimalDTO> PostAnimal(AnimalDTO animalDTO)
    {
        if(animalDTO.Name == "" || animalDTO.Name == null)
        {
            throw new InvalidInputException("Animal needs a name");
        }

        var animal = new Animal
        {
            Name=animalDTO.Name,
            Type=animalDTO.Type,
            Age=animalDTO.Age
        };

        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        return animalDTO;

    }
    //Update an exsiting animal

    public async Task<AnimalDTO> UpdateAnimal(int id, AnimalDTO animalDTO)
    {
        var animal = await _context.Animals.FindAsync(id) ?? throw new NotFoundException("Could not find the animal");
        animal.Name = animalDTO.Name;
        animal.Type = animalDTO.Type;
        animal.Age = animalDTO.Age;
        await _context.SaveChangesAsync();
        return animalDTO;
    }
    //Delete an animal

    public async Task DeleteAnimal(int id)
    {
        var animal = await _context.Animals.FindAsync(id) ?? throw new NotFoundException("Could not find the animal");


        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync();
    }
}