using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MotorX.Api.DTOs.Requests;
using MotorX.Auth.Configuration;
using MotorX.Auth.DTO;
using MotorX.Auth.DTO.Requests;
using MotorX.Auth.DTO.Responses;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        public AccountsController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters) : base(unitOfWork)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            try
            {
                return Ok(new { message = "SuccessFull EndPoint" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {

            //return await _user.Users.ToListAsync();
            var Users = await _unitOfWork.User.GetAllAsync();

            var UserDto = Users.Select(x => new UserResponse
            {
                Id = Guid.Parse(x.Id),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Password = x.PasswordHash,
                PhoneNumber = x.PhoneNumber,
                LockoutEnabled = x.LockoutEnabled
            }).ToList();

            return Ok(UserDto);
        }

        [HttpGet]
        [Route("GetUser", Name = "GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] string userid)
        {
            //return Ok(await _user.FindByIdAsync(userid));
            var IsValidFormat = Guid.TryParse(userid, out var userId);

            if (!IsValidFormat) return BadRequest("Invalid Format");

            var UserExist = await _unitOfWork.User.GetAsync(userId);

            if (UserExist is null) return NotFound("User Not Found");



            return Ok(new UserResponse
            {
                Id = Guid.Parse(UserExist.Id),
                FirstName = UserExist.FirstName!,
                LastName = UserExist.LastName!,
                Email = UserExist.Email,
                Password = UserExist.PasswordHash,
                PhoneNumber = UserExist.PhoneNumber,
                LockoutEnabled = UserExist.LockoutEnabled
            });
        }

        [HttpGet]
        [Route("GetUserProfile", Name = "GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId != null)
            {
                var IsValidFormat = Guid.TryParse(UserId, out var userId);

                if (!IsValidFormat) return BadRequest("Invalid Format");

                var UserExist = await _unitOfWork.User.GetAsync(userId);

                if (UserExist is null) return NotFound("User Not Found");



                return Ok(new UserResponse
                {
                    Id = Guid.Parse(UserExist.Id),
                    FirstName = UserExist.FirstName!,
                    LastName = UserExist.LastName!,
                    Email = UserExist.Email,
                    Password = UserExist.PasswordHash,
                    PhoneNumber = UserExist.PhoneNumber,
                    LockoutEnabled = UserExist.LockoutEnabled
                });
            }

            return BadRequest();
            
        }
        [HttpPut]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateRequest userUpdate)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //return Ok(await _user.FindByIdAsync(userid));
            var IsValidFormat = Guid.TryParse(UserId, out var userId);

            if (!IsValidFormat) return BadRequest("Invalid Format");

            var UserExist = await _unitOfWork.User.GetAsync(userId);

            if (UserExist is null) return NotFound("User Not Found");




            if (!string.IsNullOrEmpty(userUpdate.Password))
            {
                var x = await _userManager.ChangePasswordAsync(UserExist, userUpdate.CurrentPassword, userUpdate.Password);
                if (!x.Succeeded)
                {
                    var errors = x.Errors.ToList().Select(x => x.Description);
                    return BadRequest(new UserRegistrationResponse
                    {
                        Success = false,
                        Errors = errors.ToList(),
                    });
                }
            }

            if (!string.IsNullOrEmpty(userUpdate.FirstName))
                UserExist.FirstName = userUpdate.FirstName;

            if (!string.IsNullOrEmpty(userUpdate.LastName))
                UserExist.LastName = userUpdate.LastName;

            if (!string.IsNullOrEmpty(userUpdate.Email))
                UserExist.Email = userUpdate.Email;

            if (!string.IsNullOrEmpty(userUpdate.PhoneNumber))
                UserExist.PhoneNumber = userUpdate.PhoneNumber;
            var IsUpdated = await _userManager.UpdateAsync(UserExist);

            if (!IsUpdated.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }



            return Ok(new UserResponse
            {
                Id = Guid.Parse(UserExist.Id),
                FirstName = UserExist.FirstName!,
                LastName = UserExist.LastName!,
                Email = UserExist.Email,
                Password = UserExist.PasswordHash,
                PhoneNumber = UserExist.PhoneNumber,
                LockoutEnabled = UserExist.LockoutEnabled
            });
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest userUpdate)
        {
            //return Ok(await _user.FindByIdAsync(userid));
            var IsValidFormat = Guid.TryParse(userUpdate.UserId, out var userId);

            if (!IsValidFormat) return BadRequest("Invalid Format");

            var UserExist = await _unitOfWork.User.GetAsync(userId);

            if (UserExist is null) return NotFound("User Not Found");




            if (!string.IsNullOrEmpty(userUpdate.Password))
            {
                // var x = await _userManager.ChangePasswordAsync(UserExist, userUpdate.CurrentPassword, userUpdate.Password);
                var hashedPassword = _passwordHasher.HashPassword(UserExist, userUpdate.Password);
                if (!string.IsNullOrEmpty(hashedPassword))
                {
                    UserExist.PasswordHash = hashedPassword;
                }
            }

            if (!string.IsNullOrEmpty(userUpdate.FirstName))
                UserExist.FirstName = userUpdate.FirstName;

            if (!string.IsNullOrEmpty(userUpdate.LastName))
                UserExist.LastName = userUpdate.LastName;

            if (!string.IsNullOrEmpty(userUpdate.Email))
                UserExist.Email = userUpdate.Email;

            if (!string.IsNullOrEmpty(userUpdate.PhoneNumber))
                UserExist.PhoneNumber = userUpdate.PhoneNumber;
            var IsUpdated = await _userManager.UpdateAsync(UserExist);

            if (!IsUpdated.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }



            return Ok(new UserResponse
            {
                Id = Guid.Parse(UserExist.Id),
                FirstName = UserExist.FirstName!,
                LastName = UserExist.LastName!,
                Email = UserExist.Email,
                Password = UserExist.PasswordHash,
                PhoneNumber = UserExist.PhoneNumber,
                LockoutEnabled = UserExist.LockoutEnabled
            });
        }

        [HttpPut]
        [Route("LockUnlock")]
        public async Task<IActionResult> LockUnlock([FromQuery] string userid)
        {
            var IsValidFormat = Guid.TryParse(userid, out var userId);

            if (!IsValidFormat) return BadRequest("Invalid Format");

            var UserExist = await _unitOfWork.User.GetAsync(userId);

            if (UserExist is null) return NotFound("User Not Found");

            UserExist.LockoutEnabled = !UserExist.LockoutEnabled;
            var IsUpdated = await _userManager.UpdateAsync(UserExist);

            if (!IsUpdated.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }


        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] string userid)
        {
            //return Ok(await _user.FindByIdAsync(userid));
            var IsValidFormat = Guid.TryParse(userid, out var userId);

            if (!IsValidFormat) return BadRequest("Invalid Format");

            var UserExist = await _unitOfWork.User.GetAsync(userId);

            if (UserExist is null) return NotFound("User Not Found");


            //var IsDeleted = await _userManager.DeleteAsync(UserExist);

            UserExist.IsDeleted = true;

            var IsDeleted = await _userManager.UpdateAsync(UserExist);
            if (!IsDeleted.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid Credentials"
                    }
                });
            }

            var ExistUser = await _userManager.FindByEmailAsync(userRegistrationRequest.Email);
            if (ExistUser is not null)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Email already in use"
                    }
                });
            }

            var newUser = new ApplicationUser()
            {
                FirstName = userRegistrationRequest.FirstName,
                LastName = userRegistrationRequest.LastName,
                Email = userRegistrationRequest.Email,
                UserName = userRegistrationRequest.Email,
                PhoneNumber = userRegistrationRequest.PhoneNumber,
                LockoutEnabled = false
            };

            var IsCreated = await _userManager.CreateAsync(newUser, userRegistrationRequest.Password);

            if (!IsCreated.Succeeded)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = IsCreated.Errors.Select(x => x.Description).ToList()
                });
            }

            var GenerateToken = await GenerateJwtToken(newUser);

            var CreatedUser = new UserRegistrationResponse
            {
                Success = true,
                Token = GenerateToken.JwtToken,
                //RefreshToken = GenerateToken.RefreshToken
            };

            //send token as httponly cookie
            Response.Cookies.Append("X-Access-Token", GenerateToken.JwtToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, MaxAge = TimeSpan.FromDays(1) });
            Response.Cookies.Append("X-Refresh-Token", GenerateToken.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, MaxAge = TimeSpan.FromDays(1) });

            //return CreatedAtRoute("Get",IsCreated);
            return Ok(CreatedUser);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UserLoginResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid Credentials"
                    }
                });
            }

            var UserExist = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (UserExist is null)
            {
                return Unauthorized(new UserLoginResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "User Doesn't Exist"
                    }
                });
            }

            if (UserExist.LockoutEnabled)
            {
                //return Unauthorized(new UserLoginResponse
                //{
                //    Success = false,
                //    Errors = new List<string>()
                //    {
                //        "The User is Locked"
                //    }
                //});

                return StatusCode(StatusCodes.Status423Locked);
            }

            var IsCorrect = await _userManager.CheckPasswordAsync(UserExist, loginRequest.Password);
            if (!IsCorrect)
            {
                return BadRequest(new UserLoginResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid Credentials"
                    }
                });
            }

            var generatedToken = await GenerateJwtToken(UserExist);

            Response.Cookies.Append("X-Access-Token", generatedToken.JwtToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, MaxAge = TimeSpan.FromDays(1) });
            Response.Cookies.Append("X-Refresh-Token", generatedToken.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, MaxAge = TimeSpan.FromDays(1) });

            return Ok(new UserLoginResponse
            {
                Success = true,
                Token = generatedToken.JwtToken,
                UserName = UserExist.UserName,
                FullName = $"{UserExist.FirstName} {UserExist.LastName}"

                //RefreshToken = generatedToken.RefreshToken
            });

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                if (!(Request.Cookies.TryGetValue("X-Access-Token", out var accessToken) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                {
                    return BadRequest(new AuthResultResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid Tokens"
                        }
                    });
                }

                var generatedToken = await ValidateToken(new TokenRequest
                {
                    Token = accessToken,
                    RefreshToken = refreshToken
                });

                if (generatedToken is null)
                {
                    return BadRequest(new AuthResultResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                    {
                        "Token validation failed"
                    }
                    });
                }
                if (generatedToken.Errors is not null)
                {
                    if (generatedToken?.Errors[0].ToString() == "Jwt token has not expired")
                    {
                        generatedToken = new TokenResponse
                        {
                            Token = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                }


                Response.Cookies.Append("X-Access-Token", generatedToken.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, MaxAge = TimeSpan.FromDays(1) });
                Response.Cookies.Append("X-Refresh-Token", generatedToken.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, MaxAge = TimeSpan.FromDays(1) });

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(accessToken, _tokenValidationParameters, out var validatedToken);
                //var email = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)!.Value;
                var user = await _userManager.GetUserAsync(principal);

                return Ok(new UserLoginResponse
                {
                    Success = true,
                    Token = generatedToken.Token,
                    UserName = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}"
                    //RefreshToken = generatedToken.RefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResultResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                });
            }

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            //Request.Cookies.TryGetValue("X-Access-Token", out var accessToken) &&
            if (!(Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
            {
                return BadRequest(new AuthResultResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid Tokens"
                    }
                });
            }

            var RefreshTokenExist = await _unitOfWork.RefreshTokens.GetByRefreshToken(refreshToken);

            if (RefreshTokenExist is null)
            {
                return BadRequest(new TokenResponse
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Invalid refresh token"
                        }
                });
            }
            RefreshTokenExist.IsRevoked = true;
            var result = await _unitOfWork.RefreshTokens.MarkRefreshTokenAsRevoked(RefreshTokenExist);

            if (result)
            {
                await _unitOfWork.CompleteAsync();
            }

            //Response.Cookies.Append("X-Access-Token", generatedToken.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, MaxAge = TimeSpan.FromDays(1) });
            Response.Cookies.Delete("X-Refresh-Token", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, MaxAge = TimeSpan.FromDays(1) });


            return NoContent();
        }

        private async Task<TokenResponse?> ValidateToken(TokenRequest tokenRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var algResult = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!algResult) return null;
                }



                var utcExpiryDate = long.Parse(principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);


                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new TokenResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Jwt token has not expired"
                        }
                    };
                }

                var RefreshTokenExist = await _unitOfWork.RefreshTokens.GetByRefreshToken(tokenRequest.RefreshToken);

                if (RefreshTokenExist is null)
                {
                    return new TokenResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid refresh token"
                        }
                    };
                }

                if (RefreshTokenExist.ExpiryDate < DateTime.UtcNow)
                {
                    return new TokenResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token has Expired, please login again"
                        }
                    };
                }

                if (RefreshTokenExist.IsUsed)
                {
                    return new TokenResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token has been used it cannot be reused"
                        }
                    };
                }

                if (RefreshTokenExist.IsRevoked)
                {
                    return new TokenResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token has been revoked it cannot be used"
                        }
                    };
                }

                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
                if (RefreshTokenExist.JwtId != jti)
                {
                    return new TokenResponse
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh token reference does not match jwt token"
                        }
                    };
                }

                RefreshTokenExist.IsUsed = true;
                var result = await _unitOfWork.RefreshTokens.MarkRefreshTokenAsUsed(RefreshTokenExist);
                if (result)
                {
                    await _unitOfWork.CompleteAsync();

                    var dbUser = await _userManager.FindByIdAsync(RefreshTokenExist.UserId);
                    if (dbUser is null)
                    {
                        return new TokenResponse
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Error processing request"
                            }
                        };
                    }

                    var generatedTokens = await GenerateJwtToken(dbUser);


                    return new TokenResponse
                    {
                        Success = true,
                        Token = generatedTokens.JwtToken,
                        RefreshToken = generatedTokens.RefreshToken
                    };
                }

                return new TokenResponse
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Error processing request"
                    }
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixDate)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixDate).ToUniversalTime();
            return dateTime;
        }

        private async Task<AuthenticatedToken> GenerateJwtToken(ApplicationUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame), // TODO: update to minutes
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)// TODO: review if change algorithm to more Complicated one
            };

            var tokenObject = jwtHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtHandler.WriteToken(tokenObject);

            var refreshToken = new RefreshToken
            {
                AddedDate = DateTime.UtcNow,
                Token = $"{RandomString(25)}_{Guid.NewGuid()}",
                UserId = user.Id,
                IsUsed = false,
                IsRevoked = false,
                IsDeleted = false,
                JwtId = tokenObject.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.CompleteAsync();

            var authenticatedToken = new AuthenticatedToken
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
            return authenticatedToken;
        }

        private static Random random = new Random();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //public static string SubjectId(this ClaimsPrincipal user) 
        //{ 
        //    return user?.Claims?.FirstOrDefault(c => c.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))?.Value; 
        //}
    }
}
