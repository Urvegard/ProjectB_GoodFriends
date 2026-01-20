using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Models.Interfaces;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AppRazor.Pages;

public class EditFriendAddress : PageModel
{
    private readonly IAddressesService _addressesService;
    private readonly IFriendsService _friendsService;

    public EditFriendAddress(IAddressesService addressesService, IFriendsService friendsService)
    {
        _addressesService = addressesService;
        _friendsService = friendsService;
    }

    [BindProperty(SupportsGet = true)]
    public Guid FriendId { get; set; }

    [BindProperty]
    public AddressCuDto Address { get; set; } = new AddressCuDto();

    public string FriendName { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        // Hämta vännen för att fylla i eventuell befintlig adress
        var response = await _friendsService.ReadFriendAsync(FriendId, flat: false);
        if (response.Item == null)
            return NotFound();

        FriendName = $"{response.Item.FirstName} {response.Item.LastName}";

        if (response.Item.Address != null)
        {
            // Kopiera befintlig adress till DTO för formulärbindning
            Address = new AddressCuDto(response.Item.Address);
        }
        else
        {
            // Ny adress, se till att FriendsId är satt
            Address.FriendsId = new List<Guid> { FriendId };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        // Koppla alltid adress till vännen
        if (Address.FriendsId == null)
            Address.FriendsId = new List<Guid> { FriendId };
        else if (!Address.FriendsId.Contains(FriendId))
            Address.FriendsId.Add(FriendId);

        // Skapa ny adress eller uppdatera befintlig
        if (Address.AddressId == null || Address.AddressId == Guid.Empty)
        {
            await _addressesService.CreateAddressAsync(Address);
        }
        else
        {
            await _addressesService.UpdateAddressAsync(Address);
        }

        // Redirect till FriendsInDetail med flat:false för att visa uppdaterad info
        return RedirectToPage(
            "/FriendsInDetail",
            new { friendId = FriendId }
        );
    }
}

