namespace TranslateCS2.Brokers;
internal interface IAppCloseBroker {
    bool IsCancelClose { get; }
    string Message { get; }
}
