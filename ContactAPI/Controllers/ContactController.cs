using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly ContactAPIDbContext _context;
        public ContactController(ContactAPIDbContext contact)
        {
            _context = contact;
        }
        [HttpGet]
        public async Task<IActionResult> getContact()
        {
            var contact= await _context.contacts.ToListAsync(); ;
            return Ok(contact);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> getbyID([FromRoute] Guid id)
        {
            var ID=await _context.contacts.FindAsync(id);
            if (ID == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var  con = new Contact()
            {
                Id=new Guid(),
                Name=addContactRequest.Name,
                Email=addContactRequest.Email,
                Phone=addContactRequest.Phone,
                Address=addContactRequest.Address
            };
            _context.contacts.Add(con);
            await _context.SaveChangesAsync();
            return Ok(con);
        }
        [HttpPut]
        //[HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> updateCotact([FromRoute]Guid id,AddContactRequest addContactRequest)
        {
            var contactId=await _context.contacts.FindAsync(id);
            if(contactId != null)
            {
                contactId.Name=addContactRequest.Name;
                contactId.Email=addContactRequest.Email;
                await _context.SaveChangesAsync();
                return Ok(contactId);
            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var ID = await _context.contacts.FindAsync(id);
            if (ID != null)
            {
                _context.contacts.Remove(ID);
                await _context.SaveChangesAsync();
                return Ok(ID);
            }
            return NotFound();
        }
    }
}
