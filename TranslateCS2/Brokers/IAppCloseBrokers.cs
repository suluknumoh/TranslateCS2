using System.Collections.Immutable;

namespace TranslateCS2.Brokers;
internal interface IAppCloseBrokers {
    public IImmutableList<IAppCloseBroker> Items { get; }
    public void Add<T>(T appCloseBroker) where T : IAppCloseBroker;
}
