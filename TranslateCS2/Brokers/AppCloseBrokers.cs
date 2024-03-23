using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;

namespace TranslateCS2.Brokers;
internal class AppCloseBrokers : IAppCloseBrokers {
    private static AppCloseBrokers? _instance;
    private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
    private List<IAppCloseBroker> CloseBrokers { get; } = [];
    public IImmutableList<IAppCloseBroker> Items => this.CloseBrokers.ToImmutableArray();
    private AppCloseBrokers() { }
    public static AppCloseBrokers GetInstance() {
        if (_instance == null) {
            _semaphoreSlim.Wait();
            _instance ??= new AppCloseBrokers();
            _semaphoreSlim.Release();
        }
        return _instance;
    }
    public void Add<T>(T appCloseBroker) where T : IAppCloseBroker {
        this.CloseBrokers.Add(appCloseBroker);
    }
}
