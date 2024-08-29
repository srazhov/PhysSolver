namespace PhysHelper.Parsers
{
    public abstract class BaseParserHandler<T, K> : IParserHandler<T, K>
    {
        protected IParserHandler<T, K>? NextHandler;

        public void Parse(T result, K query)
        {
            Handle(result, query);
            NextHandler?.Parse(result, query);
        }

        public IParserHandler<T, K> SetNext(IParserHandler<T, K> handler)
        {
            NextHandler = handler;
            return NextHandler;
        }

        protected abstract void Handle(T parsedObj, K query);
    }
}

