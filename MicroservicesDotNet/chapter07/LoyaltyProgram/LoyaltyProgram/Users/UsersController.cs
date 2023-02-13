using Microsoft.AspNetCore.Mvc;

namespace LoyaltyProgram.Users;

[Route("/users")]
public class UsersController : ControllerBase
{
    private static readonly Dictionary<int, LoyaltyProgramUser> RegisteredUsers = new();

    // (1) The request must include a LoyaltyProgramUser in the body.
    // (2) Uses the 201 Created status code for the response.
    // (3) Adds a location header to the response because this is expected
    //     by HTTP for 201 Created responses
    // (4) Returns the user in the response for convenience(для удобства).
    [HttpPost("")]
    public ActionResult<LoyaltyProgramUser> CreateUser(
        [FromBody] LoyaltyProgramUser? user)                        // (1)
    {
        if (user == null)
            return BadRequest();

        var newUser = RegisterUser(user);
        return Created(                                             // (2)
            new Uri($"/users/{newUser.Id}", UriKind.Relative),      // (3)
            newUser);                                               // (4)
    }

    // (1) Returns the new user and lets ASP.NET turn it into an HTTP response
    [HttpPut("{userId:int}")]
    public LoyaltyProgramUser UpdateUser(int userId, [FromBody] LoyaltyProgramUser user) =>
        RegisteredUsers[userId] = user;   // (1)

    [HttpGet("{userId:int}")]
    public ActionResult<LoyaltyProgramUser> GetUser(int userId) =>
        RegisteredUsers.ContainsKey(userId)
            ? Ok(RegisteredUsers[userId])
            : NotFound();

    private LoyaltyProgramUser RegisterUser(LoyaltyProgramUser user)
    {
        var userId = RegisteredUsers.Count;
        return RegisteredUsers[userId] = user with { Id = userId };
    }
}
