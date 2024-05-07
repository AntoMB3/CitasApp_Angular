using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface ILikeRepository
{
    Task<UserLike> GetUserLikeAsync(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithLikesAsync(int userId);
    Task<IEnumerable<LikeDto>> GetUserLikesAsync(string predicate, int userId);
}