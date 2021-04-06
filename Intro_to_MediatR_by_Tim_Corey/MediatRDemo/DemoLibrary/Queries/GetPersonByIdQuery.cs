using DemoLibrary.Model;
using MediatR;

namespace DemoLibrary.Queries
{
    public record GetPersonByIdQuery(int Id) : IRequest<PersonModel>;

    //public class GetPersonByIdQueryClass : IRequest<PersonModel>
    //{
    //    public int Id { get; }

    //    public GetPersonByIdQueryClass(int id)
    //    {
    //        Id = id;
    //    }
    //}
}
