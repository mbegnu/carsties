using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);
            
            var result = await DB.Update<Item>()
                .MatchID(context.Message.Id)
                .Modify(a => a.Make, context.Message.Make)
                .Modify(a => a.Model, context.Message.Model)
                .Modify(a => a.Year, context.Message.Year)
                .Modify(a => a.Color, context.Message.Color)
                .Modify(a => a.Mileage, context.Message.Mileage)
                .ExecuteAsync();

            if (!result.IsAcknowledged) throw new MessageException(typeof(AuctionUpdated), "Problem updating auction");
        }
    }
}