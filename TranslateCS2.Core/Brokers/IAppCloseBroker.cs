namespace TranslateCS2.Core.Brokers;
public interface IAppCloseBroker {
    bool IsCancelClose { get; }
    string Message { get; }
}
