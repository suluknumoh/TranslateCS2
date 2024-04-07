using System.Collections.Immutable;

namespace TranslateCS2.Core.Brokers;
public interface IAppCloseBrokers {
    public IImmutableList<IAppCloseBroker> Items { get; }
    public void Add<T>(T appCloseBroker) where T : IAppCloseBroker;
}
