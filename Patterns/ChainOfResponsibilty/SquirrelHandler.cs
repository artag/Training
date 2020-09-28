namespace ChainOfResponsibilty
{
    public class SquirrelHandler : AbstractHandler<string>
    {
        public override string Handle(string request)
        {
            if (request == "Nut")
            {
                return $"Squirrel: I'll eat the {request}.\n";
            }

            return base.Handle(request);
        }
    }
}
