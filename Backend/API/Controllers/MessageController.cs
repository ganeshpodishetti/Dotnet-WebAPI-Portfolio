using API.Extensions;
using API.Helpers;
using Application.DTOs.Message;
using Application.Interfaces;
using Domain.Common;
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
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpGet("unreadMessages")]
    public async Task<IActionResult> GetUnReadMessages()
    {
        var result = await messageService.GetNumberOfUnread(AccessToken);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    public async Task<IActionResult> AddMessage([FromBody] MessageRequestDto request,
        IValidator<MessageRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await messageService.AddMessageAsync(request, AccessToken);
        return result.Match(
            success => Ok(new { message = "Message sent successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> MarkAsRead([FromBody] UpdateMessageDto request,
        [FromRoute] Guid id, IValidator<UpdateMessageDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));
        var result = await messageService.UpdateMessageAsync(request, id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Message updated successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid id)
    {
        var result = await messageService.DeleteMessageAsync(id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Message deleted successfully" }),
            error => error.ToActionResult());
    }
}