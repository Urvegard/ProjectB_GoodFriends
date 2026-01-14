using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.DTO;

namespace AppRazor.Pages;

public class EditFriend : PageModel
{
    private readonly IFriendsService _friendsService;

    public EditFriend(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    [BindProperty]
    public FriendCuDto Friend { get; set; } = default!;

    // GET – hämta och fyll formulär
    public async Task<IActionResult> OnGetAsync(Guid friendId)
    {
        var response = await _friendsService.ReadFriendAsync(friendId, flat: false);

        if (response.Item == null)
            return NotFound();

        var f = response.Item;

        // Vän som kommer bli "editerad"
        Friend = new FriendCuDto
        {
            FriendId = f.FriendId,
            FirstName = f.FirstName,
            LastName = f.LastName,
            Email = f.Email,
            Birthday = f.Birthday
        };

        return Page();
    }

    // POST – validera och uppdatera
    public async Task<IActionResult> OnPostAsync()
    {
        // Frontend + server-side validation
        if (!ModelState.IsValid)
            return Page();

        // Uppdaterar befintlig vän
        var response = await _friendsService.UpdateFriendAsync(Friend);

        // Service-/DB-fel → ModelState
        if (response == null || response.Item == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Could not update chosen friend."
            );
            return Page();
        }

        return RedirectToPage(
            "/FriendsInDetail",
            new { friendId = Friend.FriendId }
        );
    }
}
