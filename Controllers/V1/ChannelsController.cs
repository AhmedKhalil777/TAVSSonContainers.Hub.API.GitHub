using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hub.API.Contracts.V1;
using Hub.API.Contracts.V1.Requests;
using Hub.API.Domain;
using Hub.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hub.API.V1.Controllers
{

    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelsService _ChannelService;
        
        private static object successfull = new { Status = 1, Message = "Successful Transaction" };
        private static object failed = new { Status = 0, Message = "Failed Transaction" };
        public ChannelsController(IChannelsService channelsService)
        {
            _ChannelService = channelsService;
        }
        [HttpPut(ApiRoutes.Chat.AddUserToChannel)]
        public async Task<IActionResult> AddUserToChannel([FromRoute] string CID , [FromRoute] string UID)
        {
            var res = await _ChannelService.AddUserToChannel(CID, UID);
            if (res)
            {
                return Ok(successfull);
            }
            return BadRequest();
        }

        [HttpGet(ApiRoutes.Chat.GetUserChannels)]
        public async Task<IActionResult> GetUserChannels([FromRoute] string UID) => Ok(await _ChannelService.GetUserChannels(UID));
        [HttpPost(ApiRoutes.Chat.CreateChannel)]
        public async Task<IActionResult> CreateChannel([FromBody] CreateChannelViewModel model)
        {
            var channel = new Channel() { Caption = model.Caption, Name = model.Name, AdminId = model.AdminId };
            var result = await _ChannelService.CreateChannel(channel);
            if (result)
                return Ok(successfull);
            return BadRequest(failed);
        }
        [HttpGet(ApiRoutes.Chat.SearchUser)]
        public async Task<IActionResult> SearchUser([FromBody] string filter) => Ok(await _ChannelService.SearchUser(filter));

        [HttpGet(ApiRoutes.Chat.GetUser)]
        public async Task<IActionResult> GetUser([FromRoute] string UID) => Ok(await _ChannelService.GetUser(UID));

        [HttpGet(ApiRoutes.Chat.GetUsers)]
        public async Task<IActionResult> GetUsers() => Ok( await _ChannelService.GetUsers());

        [HttpPost(ApiRoutes.Chat.CreateUser)]
        public async Task<IActionResult> CreateUser([FromForm] string name)
        {
            if (await _ChannelService.CreateUser(new User() { Name = name , ImgPath = ""}))
                return Ok(successfull);
            return BadRequest(failed);
        }
        [HttpGet(ApiRoutes.Chat.GetChannel)]
        //CID = ChannelId
        public async Task<IActionResult> GetChannel([FromRoute] string CID) => Ok(await _ChannelService.GetChannel(CID));

        [HttpPut(ApiRoutes.Chat.SendMessage)]
        public async Task<IActionResult> SendMessage([FromBody] MessageContainerViewModel model)
        {
            var message = new Message()
            {
                Body = model.Body,
                Date = DateTime.Now,
                UserId = model.UID
            };

            if (await _ChannelService.SendMessage(model.CID, model.UID, message))
                return Ok(successfull);

            return BadRequest(failed);

        }

        [HttpDelete(ApiRoutes.Chat.DeleteChannel)]
        public async Task<IActionResult> DeleteChannel([FromRoute] string CID)
        {
            if (await _ChannelService.DeleteChannel(CID))
                return Ok(successfull);
            return BadRequest(failed);
        }

        [HttpPost(ApiRoutes.Chat.InsertImgtoChannel)]
        public async Task<IActionResult> InsertImgtoChannel([FromRoute] string CID ,[FromForm] IFormFile file)
        {
            var result = await _ChannelService.InsertImgtoChannel(CID, file);
            if (result)
            {
                return Ok(successfull);
            }
            return BadRequest(failed);

        }

        [HttpPut(ApiRoutes.Chat.InsertImgtoUser)]
        public async Task<IActionResult> InsertImgtoUser([FromRoute] string UID, IFormFile file)
        {
            var result = await _ChannelService.InsertImgtoUser(UID, file);
            if (result)
            {
                return Ok(successfull);
            }
            return BadRequest(failed);

        }


    }
}