using System;
using Xunit;

public class InMemoryRepositoryTests
{
    private static InMemoryRepository<AccessEvent, int> CreateRepo()
        => new InMemoryRepository<AccessEvent, int>(e => e.Id);

    private static AccessEvent CreateSampleEvent(int id = 1, string badgeId = "BADGE-001")
        => new AccessEvent(
            Id: id,
            BadgeId: badgeId,
            AreaName: "Sala Servidores",
            Timestamp: DateTimeOffset.Now,
            AccessGranted: true
        );

    [Fact]
    public void Add_Then_ListAll_ShouldReturnOneItem()
    {
        var repo = CreateRepo();

        repo.Add(CreateSampleEvent(id: 1));
        var all = repo.ListAll();

        Assert.Single(all);
        Assert.Equal(1, all[0].Id);
    }

    [Fact]
    public void GetById_Existing_ShouldReturnEntity()
    {
        var repo = CreateRepo();

        repo.Add(CreateSampleEvent(id: 1, badgeId: "BADGE-XYZ"));

        var found = repo.GetById(1);

        Assert.NotNull(found);
        Assert.Equal("BADGE-XYZ", found!.BadgeId);
    }

    [Fact]
    public void GetById_Missing_ShouldReturnNull()
    {
        var repo = CreateRepo();

        var found = repo.GetById(99);

        Assert.Null(found);
    }

    [Fact]
    public void Update_Existing_ShouldReturnTrue()
    {
        var repo = CreateRepo();

        repo.Add(CreateSampleEvent(id: 1, badgeId: "BADGE-001"));

        var updatedEvent = new AccessEvent(
            Id: 1,
            BadgeId: "BADGE-001",
            AreaName: "Sala Restrita",
            Timestamp: DateTimeOffset.Now,
            AccessGranted: false
        );

        var updated = repo.Update(updatedEvent);

        Assert.True(updated);

        var reloaded = repo.GetById(1);
        Assert.NotNull(reloaded);
        Assert.Equal("Sala Restrita", reloaded!.AreaName);
        Assert.False(reloaded.AccessGranted);
    }

    [Fact]
    public void Update_Missing_ShouldReturnFalse()
    {
        var repo = CreateRepo();

        var updated = repo.Update(CreateSampleEvent(id: 1));

        Assert.False(updated);
    }

    [Fact]
    public void Remove_Existing_ShouldReturnTrue()
    {
        var repo = CreateRepo();

        repo.Add(CreateSampleEvent(id: 1));
        var removed = repo.Remove(1);

        Assert.True(removed);
        Assert.Empty(repo.ListAll());
    }

    [Fact]
    public void Remove_Missing_ShouldReturnFalse()
    {
        var repo = CreateRepo();

        var removed = repo.Remove(99);

        Assert.False(removed);
    }
}
