using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("section")]
[ApiController]
public class SectionController : ControllerBase
{
    private readonly SectionService _sectionService;

    public SectionController(SectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpGet]
    public async Task<ActionResult<Section>> GetAllSections()
    {

    }

    [HttpPost]
    public async Task<ActionResult<Section>> PostSection([FromBody] Section section)
    {

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSection(int id, [FromBody] Section section)
    {

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSection(int id)
    {

    }
}