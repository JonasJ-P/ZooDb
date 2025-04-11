using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("animal")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly AnimalService _animalService;
    public AnimalController(AnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAnimals()
    {
        var animals = await _animalService.GetAllAnimals();
        return Ok(animals); 
    }
}