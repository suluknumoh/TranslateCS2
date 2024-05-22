using System;

namespace TranslateCS2.Inf.Attributes;
/// <summary>
///     to exclude a <see langword="class" /> from code coverage report
///     <br/>
///     should only be used for
///     <br/>
///     'untestable' Unity-related <see langword="class"/>es
///     <br/>
///     or
///     <br/>
///     <see langword="class"/>es that only contain <see langword="const"/>ants
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class MyExcludeClassFromCoverageAttribute : Attribute { }
