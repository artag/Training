using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportsStore.Infrastructure;

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession Session { get; private set; }

        public static Cart GetCart(IServiceProvider services)
        {
            var contextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            var session = contextAccessor?.HttpContext.Session;

            var sessionCart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            sessionCart.Session = session;

            return sessionCart;
        }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
