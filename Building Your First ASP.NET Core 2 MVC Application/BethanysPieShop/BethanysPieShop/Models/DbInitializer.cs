using Microsoft.EntityFrameworkCore.Internal;

namespace BethanysPieShop.Models
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Pies.Any())
            {
                return;
            }

            context.AddRange(
                new Pie
                {
                    Name = "Apple Pie",
                    Price = 12.95M,
                    ShortDescription = "Our famous apple pies!",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. " +
                                      "Icing gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant lemon " +
                                      "drops liquorice lemon drops cookie lollipop toffee. " +
                                      "Carrot cake carrot cake liquorice sugar plum topping " +
                                      "bonbon pie muffin jujubes. Jelly pastry wafer tart " +
                                      "caramels bear claw. Tiramisu tart pie cake danish lemon " +
                                      "drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = true,
                    ImageUrl = @"/blob/applepie.jpg",
                    ImageThumbnailUrl = @"/blob/applepiesmall.jpg"
                },
                new Pie
                {
                    Name = "Blueberry Cheese Cake",
                    Price = 18.95M,
                    ShortDescription = "You'll love it!",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/blueberrycheesecake.jpg",
                    ImageThumbnailUrl = @"/blob/blueberrycheesecakesmall.jpg"
                },
                new Pie
                {
                    Name = "Cheese Cake",
                    Price = 18.95M,
                    ShortDescription = "Plain cheese cake. Plain pleasure.", 
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/cheesecake.jpg",
                    ImageThumbnailUrl = @"/blob/cheesecakesmall.jpg"
                },
                new Pie
                {
                    Name = "Cherry Pie",
                    Price = 15.95M,
                    ShortDescription = "A summer classic!",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie " +
                                      "biscuit danish chocolate cake. Danish powder cookie " +
                                      "macaroon chocolate donut tart. Carrot cake dragée " +
                                      "croissant lemon drops liquorice lemon drops cookie " +
                                      "lollipop toffee. Carrot cake carrot cake liquorice " +
                                      "sugar plum topping bonbon pie muffin jujubes. Jelly " +
                                      "pastry wafer tart caramels bear claw. Tiramisu tart " +
                                      "pie cake danish lemon drops. Brownie cupcake dragée " +
                                      "gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/cherrypie.jpg",
                    ImageThumbnailUrl = @"/blob/cherrypiesmall.jpg"
                },
                new Pie
                {
                    Name = "Christmas Apple Pie",
                    Price = 13.95M,
                    ShortDescription = "Happy holidays with this pie!",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/christmasapplepie.jpg",
                    ImageThumbnailUrl = @"/blob/christmasapplepiesmall.jpg"
                },
                new Pie
                {
                    Name = "Cranberry Pie",
                    Price = 17.95M,
                    ShortDescription = "A Christmas favorite",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/cranberrypie.jpg",
                    ImageThumbnailUrl = @"/blob/cranberrypiesmall.jpg"
                },
                new Pie
                {
                    Name = "Peach Pie",
                    Price = 15.95M,
                    ShortDescription = "Sweet as peach",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie " +
                                      "roll oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/peachpie.jpg",
                    ImageThumbnailUrl = @"/blob/peachpiesmall.jpg"
                },
                new Pie
                {
                    Name = "Pumpkin Pie",
                    Price = 12.95M,
                    ShortDescription = "Our Halloween favorite",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet icing " +
                                      "bonbon gummies. Gummies lollipop brownie biscuit danish " +
                                      "chocolate cake. Danish powder cookie macaroon chocolate " +
                                      "donut tart. Carrot cake dragée croissant lemon drops " +
                                      "liquorice lemon drops cookie lollipop toffee. Carrot " +
                                      "cake carrot cake liquorice sugar plum topping bonbon " +
                                      "pie muffin jujubes. Jelly pastry wafer tart caramels " +
                                      "bear claw. Tiramisu tart pie cake danish lemon drops. " +
                                      "Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = true,
                    ImageUrl = @"/blob/pumpkinpie.jpg",
                    ImageThumbnailUrl = @"/blob/pumpkinpiesmall.jpg"
                },
                new Pie
                {
                    Name = "Rhubarb Pie",
                    Price = 15.95M,
                    ShortDescription = "My God, so sweet!",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = true,
                    ImageUrl = @"/blob/rhubarbpie.jpg",
                    ImageThumbnailUrl = @"/blob/rhubarbpiesmall.jpg"
                },
                new Pie
                {
                    Name = "Strawberry Pie",
                    Price = 15.95M,
                    ShortDescription = "Our delicious strawberry pie!",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/strawberrypie.jpg",
                    ImageThumbnailUrl = @"/blob/strawberrypiesmall.jpg"
                },
                new Pie
                {
                    Name = "Strawberry Cheese Cake",
                    Price = 18.95M,
                    ShortDescription = "You'll love it!",
                    LongDescription = "Icing carrot cake jelly-o cheesecake. Sweet roll " +
                                      "marzipan marshmallow toffee brownie brownie candy " +
                                      "tootsie roll. Chocolate cake gingerbread tootsie roll " +
                                      "oat cake pie chocolate bar cookie dragée brownie. " +
                                      "Lollipop cotton candy cake bear claw oat cake. Dragée " +
                                      "candy canes dessert tart. Marzipan dragée gummies " +
                                      "lollipop jujubes chocolate bar candy canes. Icing " +
                                      "gingerbread chupa chups cotton candy cookie sweet " +
                                      "icing bonbon gummies. Gummies lollipop brownie biscuit " +
                                      "danish chocolate cake. Danish powder cookie macaroon " +
                                      "chocolate donut tart. Carrot cake dragée croissant " +
                                      "lemon drops liquorice lemon drops cookie lollipop " +
                                      "toffee. Carrot cake carrot cake liquorice sugar plum " +
                                      "topping bonbon pie muffin jujubes. Jelly pastry wafer " +
                                      "tart caramels bear claw. Tiramisu tart pie cake danish " +
                                      "lemon drops. Brownie cupcake dragée gummies.",
                    IsPieOfTheWeek = false,
                    ImageUrl = @"/blob/strawberrycheesecake.jpg",
                    ImageThumbnailUrl = @"/blob/strawberrycheesecakesmall.jpg"
                }
            );

            context.SaveChanges();
        }
    }
}
