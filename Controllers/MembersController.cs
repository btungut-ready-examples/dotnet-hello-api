using Microsoft.AspNetCore.Mvc;

namespace HelloAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MembersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Member>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Member>>> GetList(int? filterAgeAbove = null)
    {
        List<Member> members;

        if (filterAgeAbove == null)
            members = await Member.GetAsync();
        else
            members = await Member.GetAsync(m => m.Age >= filterAgeAbove);

        return Ok(members);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Member), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Member>> GetById(int id)
    {
        var member = await Member.GetByIdAsync(id);

        if (member == null)
            return NotFound();

        return Ok(member);
    }
}
