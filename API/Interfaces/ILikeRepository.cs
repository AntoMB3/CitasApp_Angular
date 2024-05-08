using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface ILikeRepository
{
    Task<UserLike> GetUserLikeAsync(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithLikesAsync(int userId);
    Task<PagedList<LikeDto>> GetUserLikesAsync(LikesParams likesParams);
}