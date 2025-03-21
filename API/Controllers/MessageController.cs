using API.Helpers;
using Application.DTOs.Message;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessageController(
    IMessageService messageService,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation)
    : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetMessages()
    {
        var result = await messageService.GetMessagesByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpGet("unreadMessages")]
    public async Task<IActionResult> GetUnReadMessages()
    {
        var result = await messageService.GetNumberOfUnread(AccessToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddMessage([FromBody] MessageRequestDto request,
        IValidator<MessageRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await messageService.AddMessageAsync(request, AccessToken);
        return Ok("Message sent successfully.");
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> MarkAsRead([FromBody] UpdateMessageDto request,
        [FromRoute] Guid id, IValidator<UpdateMessageDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));
        var result = await messageService.UpdateMessageAsync(request, id, AccessToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid id)
    {
        var result = await messageService.DeleteMessageAsync(id, AccessToken);
        return Ok("Message deleted successfully.");
    }
}