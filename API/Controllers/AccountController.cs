using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context) : BaseApiController
    {
        [HttpPost("register")] //  account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {

            if (await UserExist(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                Password = registerDto.Password
             //   PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //    PasswordSalt = hmac.Key

            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        private async Task<bool> UserExist(string username)
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

        [HttpPost("login")] //  account/login

        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower() && x.Password.ToLower() == loginDto.Password);

            if(user == null) return Unauthorized("Invalid username");


            // using var hmac = new HMACSHA512(user.PasswordSalt);

            // var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // for (int i = 0; i < computedhash.Length; i++)
            // {
            //     if(computedhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            // }

            return user;
        }

    }
}