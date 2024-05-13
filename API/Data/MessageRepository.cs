using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Message> GetMessageAsync(int id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams mp)
    {
        var query = _context.Messages
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        query = mp.Container.ToLowerInvariant() switch
        {
            "inbox" => query.Where(u => u.RecipientUsername == mp.Username),
            "outbox" => query.Where(u => u.SenderUsername == mp.Username),
            _ => query.Where(u => u.RecipientUsername == mp.Username && u.DateRead == null)
        };

        var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

        return await PagedList<MessageDto>
            .CreateAsync(messages, mp.PageNumber, mp.PageSize);
    }

    public Task<PagedList<MessageDto>> GetMessagesForUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int currentUserId, int recipientId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}