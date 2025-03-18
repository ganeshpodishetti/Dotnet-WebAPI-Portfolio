using API.Helpers;
using Application.DTOs.Message;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessageController(
    IMessageService messageService,
    IAccessTokenHelper accessTokenHelper)
    : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet("getMessages")]
    public async Task<IActionResult> GetMessages()
    {
        var result = await messageService.GetMessagesByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUnReadMessages()
    {
        var result = await messageService.GetNumberOfUnread(AccessToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddMessage([FromBody] MessageRequestDto messageRequestDto)
    {
        var result = await messageService.AddMessageAsync(messageRequestDto, AccessToken);
        return Ok("Message sent successfully.");
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> MarkAsRead([FromBody] MessageRequestDto messageRequestDto,
        [FromRoute] Guid id)
    {
        var result = await messageService.UpdateMessageAsync(messageRequestDto, id, AccessToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid id)
    {
        var result = await messageService.DeleteMessageAsync(id, AccessToken);
        return Ok("Message deleted successfully.");
    }
}