namespace ChainOfResponsibilty
{
    public class DogHandler : AbstractHandler<string>
    {
        public override string Handle(string request)
        {
            if (request == "MeatBall")
            {
                return $"Dog: I'll eat the {request}.\n";
            }

            return base.Handle(request);
        }
    }
}
