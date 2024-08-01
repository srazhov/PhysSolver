namespace PhysHelper.Parsers
{
    public interface IParserHandler<T, K>
    {
        IParserHandler<T, K> SetNext(IParserHandler<T, K> handler);

        void Parse(T result, K query);
    }
}

