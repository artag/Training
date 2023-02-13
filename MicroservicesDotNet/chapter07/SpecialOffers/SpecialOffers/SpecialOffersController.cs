using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using SpecialOffers.Events;

namespace SpecialOffers.SpecialOffers;

public record Offer(string Description, int Id);

[Route("/offers")]
public class SpecialOffersController : ControllerBase
{
    private static readonly IDictionary<int, Offer> Offers = new ConcurrentDictionary<int, Offer>();
    private readonly IEventStore _eventStore;

    public SpecialOffersController(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Offer> GetOffer(int id) =>
        Offers.ContainsKey(id)
            ? Ok(Offers[id])
            : NotFound();

    [HttpPost("")]
    public ActionResult<Offer> CreateOffer([FromBody] Offer offer)
    {
        if (offer == null)
            return BadRequest();
        var newOffer = NewOffer(offer);
        return Created(new Uri($"/offers/{newOffer.Id}", UriKind.Relative), newOffer);
    }

    [HttpPut("{id:int}")]
    public Offer UpdateOffer(int id, [FromBody] Offer offer)
    {
        var offerWithId = offer with { Id = id };
        _eventStore.RaiseEvent("SpecialOfferUpdated", new { OldOffer = Offers[id], NewOffer = offerWithId });
        return Offers[id] = offerWithId;
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteOffer(int id)
    {
        _eventStore.RaiseEvent("SpecialOfferRemoved", new { Offer = Offers[id] });
        Offers.Remove(id);
        return Ok();
    }

    private Offer NewOffer(Offer offer)
    {
        var offerId = Offers.Count;
        var newOffer = offer with { Id = offerId };
        _eventStore.RaiseEvent("SpecialOfferCreated", newOffer);
        return Offers[offerId] = newOffer;
    }
}
