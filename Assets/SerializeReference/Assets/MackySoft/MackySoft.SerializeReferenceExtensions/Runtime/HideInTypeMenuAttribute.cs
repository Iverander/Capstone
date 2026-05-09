using System;

/// <summary>
///     An attribute that hides the type in the SubclassSelector.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface,
    Inherited = false)]
public sealed class HideInTypeMenuAttribute : Attribute
{
}