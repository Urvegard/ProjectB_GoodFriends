using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AppRazor.Pages
{
    public class EditFriendAddress : PageModel
    {
        private readonly IFriendsService _friendsService;
        private readonly IAddressesService _addressesService;

        public EditFriendAddress(IFriendsService friendsService, IAddressesService addressesService)
        {
            _friendsService = friendsService;
            _addressesService = addressesService;
        }

        // FriendId skickas via route
        [BindProperty(SupportsGet = true)]
        public Guid FriendId { get; set; }

        // FriendName används i rubrik
        public string FriendName { get; set; } = "";

        // Adress som ska bindas till formulär
        [BindProperty]
        public AddressCuDto Address { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // Hämta vän
            var friendResponse = await _friendsService.ReadFriendAsync(FriendId, flat: false);
            if (friendResponse.Item == null)
                return NotFound();

            // Spara vänens namn för rubrik
            FriendName = $"{friendResponse.Item.FirstName} {friendResponse.Item.LastName}";

            // Om adress saknas → skapa tom AddressCuDto (AddressId null)
            if (friendResponse.Item.Address == null)
            {
                Address = new AddressCuDto
                {
                    AddressId = null,
                    StreetAddress = "",
                    ZipCode = 0,
                    City = "",
                    Country = "",
                    FriendsId = new List<Guid> { FriendId } // koppla adress till vän
                };
            }
            else
            {
                var f = friendResponse.Item.Address;
                Address = new AddressCuDto
                {
                    AddressId = f.AddressId,
                    StreetAddress = f.StreetAddress,
                    ZipCode = f.ZipCode,
                    City = f.City,
                    Country = f.Country,
                    FriendsId = new List<Guid> { FriendId } // koppla adress till vän
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Skapa ny adress om AddressId saknas
            if (Address.AddressId == null || Address.AddressId == Guid.Empty)
            {
                await _addressesService.CreateAddressAsync(Address);
            }
            else
            {
                // Annars uppdatera befintlig adress
                await _addressesService.UpdateAddressAsync(Address);
            }

            // Redirect tillbaka till friend-details
            return RedirectToPage(
                "/FriendsInDetail",
                new { friendId = FriendId }
            );
        }
    }
}
