using System;

namespace TranslateCS2.Core.Services.LatestVersions;
public interface ILatestVersionCheckService {
    Version Latest { get; }
    Version Current { get; }
    bool IsNewVersionAvailable();
}
