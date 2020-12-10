namespace GuessTheNumberGame.Common
{
    /// <summary>
    /// Принтер.
    /// </summary>
    public interface IPrinter<in T>
    {
        /// <summary>
        /// Печатает медийный/информационный материал.
        /// </summary>
        /// <param name="media">Медийный/информационный материал.</param>
        void Print(T media);
    }
}
