using API.Handlers;
using API.Helpers;
using Application.DTOs.Message;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize(Roles = "Admin")]
public class MessageController(
    IMessageService messageService,
    IAccessTokenHelper accessTokenHelper,
    ILogger<MessageController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetMessages()
    {
        logger.LogInformation("Retrieving messages for user");
        var result = await messageService.GetMessagesByUserIdAsync(AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully retrieved {Count} messages", result.Value.Count());
                return Ok(result.Value);
            },
            error =>
            {
                logger.LogError("Failed to retrieve messages: {Error}", error.Description);
                return error.ToActionResult();
            });
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
    public async Task<IActionResult> AddMessage([FromBody] MessageRequestDto request)
    {
        logger.LogInformation("Adding new message from: {Email}", request.Email);
        var result = await messageService.AddMessageAsync(request, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully added message from: {Email}", request.Email);
                return Ok(new { message = "Message sent successfully" });
            },
            error =>
            {
                logger.LogError("Failed to add message: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> MarkAsRead([FromBody] UpdateMessageDto request,
        [FromRoute] Guid id)
    {
        var result = await messageService.UpdateMessageAsync(request, id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Message updated successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting message from: {id}", id);
        var result = await messageService.DeleteMessageAsync(id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully deleted message from: {id}", id);
                return Ok(new { message = "Message deleted successfully" });
            },
            error =>
            {
                logger.LogError("Failed to delete message: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}