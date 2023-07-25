using System.Collections.Generic;
using System.Threading.Tasks;
using EFCrud.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCrud;

public static class Operations
{
    public static async Task<int> Create(MobileDevice mobileDevice)
    {
        await using var db = new Repository();
        var entry = db.MobileDevices.Add(mobileDevice);
        await db.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public static async Task<ICollection<MobileDevice>> Read()
    {
        await using var db = new Repository();
        db.ChangeTracker.AutoDetectChangesEnabled = false;
        var entities = await db.MobileDevices.ToArrayAsync();
        return entities;
    }

    public static async Task<MobileDevice?> Read(int id)
    {
        await using var db = new Repository();
        db.ChangeTracker.AutoDetectChangesEnabled = false;
        var entity = await db.FindAsync<MobileDevice>(id);
        return entity;
    }

    public static async Task Update(MobileDevice mobileDevice)
    {
        await using var db = new Repository();
        db.MobileDevices.Update(mobileDevice);
        await db.SaveChangesAsync();
    }

    public static async Task Delete(MobileDevice mobileDevice)
    {
        await using var db = new Repository();
        db.MobileDevices.Remove(mobileDevice);
        await db.SaveChangesAsync();
    }
}
