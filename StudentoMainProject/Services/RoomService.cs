using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class RoomService
    {
        private readonly SchoolContext context;

        public RoomService(SchoolContext context)
            => this.context = context;

        public async Task AddRoomAsync(Room room)
        {
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
        }

        public async Task RemoveRoomAsync(Room room)
        {
            context.Rooms.Remove(room);
            await context.SaveChangesAsync();
        }

        public async Task RemoveRoomAsync(int id)
            => await RemoveRoomAsync(await GetRoomById(id));

        public async Task UpdateRoomAsync(Room room)
        {
            context.Attach(room).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Room> GetRoomById(int id)
            => await context.Rooms.Where(r => r.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<Room> GetRoomFullById(int id)
            => await context.Rooms.Where(r => r.Id == id).Include(r => r.TimeFrames).AsNoTracking().FirstOrDefaultAsync();

        public async Task<Room[]> GetAllRooms()
            => await context.Rooms.AsNoTracking().ToArrayAsync();
    }
}
