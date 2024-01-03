using ETradeAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETradeAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandResponse
    {
       
    }
    public class LoginUserSuccessCommandResponse : LoginUserCommandResponse
    {
        public Token Token { get; set; }
    }
    public class LoginUserErrorCommandResponse : LoginUserCommandResponse
    {
        public string Message { get; set; }
    }
}
