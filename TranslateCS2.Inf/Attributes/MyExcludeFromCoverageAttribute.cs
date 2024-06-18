using System;

namespace TranslateCS2.Inf.Attributes;
/// <summary>
///     to exclude a <see langword="class" /> from code coverage report
///     <br/>
///     should only be used for
///     <br/>
///     'untestable' Unity-related <see langword="class"/>es
///     <br/>
///     <br/>
///     or <see langword="class"/>es that only contain <see langword="const"/>ants
///     <br/>
///     or <see cref="Object.ToString"/>-<see langword="method"/>s
///     <br/>
///     or <see cref="Object.Equals(Object)"/>-<see langword="method"/>s
///     <br/>
///     or <see cref="Object.Equals(Object, Object)"/>-<see langword="method"/>s
///     <br/>
///     or <see cref="Object.GetHashCode"/>-<see langword="method"/>s
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class MyExcludeFromCoverageAttribute : Attribute { }
