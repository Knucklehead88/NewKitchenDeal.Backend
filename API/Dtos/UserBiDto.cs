using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserBiDto: UserDto
    {
        public ResponseBusinessInfoTradesDto BusinessInfo { get; set; }
    }
}