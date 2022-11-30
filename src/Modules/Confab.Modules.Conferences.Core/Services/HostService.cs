using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Exceptions;
using Confab.Modules.Conferences.Core.Policies;
using Confab.Modules.Conferences.Core.Repositories;

namespace Confab.Modules.Conferences.Core.Services;

internal class HostService : IHostService
{
    private readonly IHostRepository _hostRepository;
    private readonly IHostDeletionPolicy _hostDeletionPolicy;

    public HostService(IHostRepository hostRepository, IHostDeletionPolicy hostDeletionPolicy)
    {
        _hostRepository = hostRepository;
        _hostDeletionPolicy = hostDeletionPolicy;
    }
    
    public async Task AddAsync(HostDto dto)
    {
        dto.Id = Guid.NewGuid();
        await _hostRepository.AddAsync(new Host
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        });
    }

    public async Task<HostDetailsDto?> GetAsync(Guid id)
    {
        var host = await _hostRepository.GetAsync(id);

        if (host is null)
        {
            return null;
        }

        var dto = Map<HostDetailsDto>(host);
        dto.Conferences = host.Conferences.Select(x => new ConferenceDto
        {
            Id = x.Id,
            HostId = x.HostId,
            HostName = x.Host.Name,
            From = x.From,
            To = x.To,
            Name = x.Name,
            Location = x.Location,
            LogoUrl = x.LogoUrl,
            ParticipantsLimit = x.ParticipantsLimit
        }).ToList();

        return dto;
    }

    public async Task<IReadOnlyList<HostDto>> BrowserAsync()
    {
        var hosts = await _hostRepository.BrowserAsync();

        return hosts.Select(Map<HostDto>).ToList();
    }
    
    /// <exception cref="HostNotFoundException"></exception>
    public async Task UpdateAsync(HostDetailsDto dto)
    {
        var host = await _hostRepository.GetAsync(dto.Id);

        if (host is null)
        {
            throw new HostNotFoundException(dto.Id);
        }

        host.Name = dto.Name;
        host.Description = dto.Description;

        await _hostRepository.UpdateAsync(host);
    }
    
    /// <exception cref="HostNotFoundException"></exception>
    /// <exception cref="CannotDeleteHostException"></exception>
    public async Task DeleteAsync(Guid id)
    {
        var host = await _hostRepository.GetAsync(id);
        
        if (host is null)
        {
            throw new HostNotFoundException(id);
        }

        if (await _hostDeletionPolicy.CanDeleteAsync(host) is false)
        {
            throw new CannotDeleteHostException(id);
        }

        await _hostRepository.DeleteAsync(host);
    }

    private static T Map<T>(Host host) where T : HostDto, new()
    {
        return new T()
        {
            Id = host.Id,
            Name = host.Name,
            Description = host.Description
        };
    }
}