using System.Collections.Generic;

namespace RestAndApiControllers.Models
{
    public class MemoryRepository : IRepository
    {
        private Dictionary<int, Reservation> _items;

        public MemoryRepository()
        {
            _items = new Dictionary<int, Reservation>();
            new List<Reservation>()
            {
                new Reservation { ClientName = "Alice", Location = "Board Room" },
                new Reservation { ClientName = "Bob", Location = "Lecture Hall" },
                new Reservation { ClientName = "Joe", Location = "Meeting Room 1" }
            }.ForEach(r => AddReservation(r));
        }

        public Reservation this[int id] => _items.ContainsKey(id) ? _items[id] : null;

        public IEnumerable<Reservation> Reservations => _items.Values;

        public Reservation AddReservation(Reservation reservation)
        {
            if (reservation.ReservationId == 0)
            {
                var key = _items.Count;
                while (_items.ContainsKey(key))
                {
                    key++;
                }

                reservation.ReservationId = key;
            }

            _items[reservation.ReservationId] = reservation;
            return reservation;
        }

        public void DeleteReservation(int id) => _items.Remove(id);

        public Reservation UpdateReservation(Reservation reservation) =>
            AddReservation(reservation);
    }
}
