using ETradeAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETradeAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandResponse
    {
        public Token Token { get; set; }
    }
}
