using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;

using TranslateCS2.Core.Brokers;

namespace TranslateCS2.Brokers;
internal class AppCloseBrokers : IAppCloseBrokers {
    private static AppCloseBrokers? instance;
    private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
    private List<IAppCloseBroker> CloseBrokers { get; } = [];
    public IImmutableList<IAppCloseBroker> Items => this.CloseBrokers.ToImmutableArray();
    private AppCloseBrokers() { }
    public static AppCloseBrokers GetInstance() {
        if (instance is null) {
            semaphoreSlim.Wait();
            instance ??= new AppCloseBrokers();
            semaphoreSlim.Release();
        }
        return instance;
    }
    public void Add<T>(T appCloseBroker) where T : IAppCloseBroker {
        this.CloseBrokers.Add(appCloseBroker);
    }
}
