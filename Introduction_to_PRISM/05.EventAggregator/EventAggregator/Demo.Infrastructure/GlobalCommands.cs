using Prism.Commands;

namespace Demo.Infrastructure
{
    public static class GlobalCommands
    {
        public static CompositeCommand SaveAllCommand = new CompositeCommand();
    }
}
