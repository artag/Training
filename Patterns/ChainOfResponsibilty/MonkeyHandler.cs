namespace ChainOfResponsibilty
{
    public class MonkeyHandler : AbstractHandler<string>
    {
        public override string Handle(string request)
        {
            if (request == "Banana")
            {
                return $"Monkey: I'll eat the {request}.\n";
            }

            return base.Handle(request);
        }
    }
}
