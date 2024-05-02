using System;
using System.Threading.Tasks;

namespace TranslateCS2.Core.Services.LatestVersions;
public interface ILatestVersionCheckService {
    Version Latest { get; }
    Version Current { get; }
    Task<bool> IsNewVersionAvailable();
}
