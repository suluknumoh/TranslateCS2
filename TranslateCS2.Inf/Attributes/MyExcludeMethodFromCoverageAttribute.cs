using System;

namespace TranslateCS2.Inf.Attributes;
/// <summary>
///     should only be used for
///     <br/>
///     <see cref="Object.ToString"/>-<see langword="method"/>s
///     <br/>
///     <see cref="Object.Equals(Object)"/>
///     <br/>
///     <see cref="Object.Equals(Object, Object)"/>
///     <br/>
///     <see cref="Object.GetHashCode"/>
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class MyExcludeMethodFromCoverageAttribute : Attribute {
}
