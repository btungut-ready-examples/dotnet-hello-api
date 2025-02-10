using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace HelloAPI;

public class Member
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int Age { get; private set; }


    #region Static members for in-memory Member accesses
    private static readonly Dictionary<int, Member> members = new Dictionary<int, Member>
    {
        { 1, new Member { Id = 1, Name = "John Doe", Age = 30 } },
        { 2, new Member { Id = 2, Name = "Jane Doe", Age = 25 } },
        { 3, new Member { Id = 3, Name = "Sammy Doe", Age = 35 } }
    };

    private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

    public static async Task<Member> CreateAsync(string name, int age)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age));

        await semaphoreSlim.WaitAsync();
        int lastId = members.Count;
        try
        {
            var member = new Member
            {
                Id = lastId + 1,
                Name = name,
                Age = age
            };
            members.Add(member.Id, member);
            return member;
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    public static async Task<Member> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

        await semaphoreSlim.WaitAsync();
        try
        {
            if (!members.TryGetValue(id, out var member))
                return null;
            return member;
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }


    public static async Task<List<Member>> GetAsync(Func<Member, bool> predicate = null)
    {
        await semaphoreSlim.WaitAsync();
        try
        {
            var queryable = members.Values.AsEnumerable();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return queryable.ToList();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    #endregion
}