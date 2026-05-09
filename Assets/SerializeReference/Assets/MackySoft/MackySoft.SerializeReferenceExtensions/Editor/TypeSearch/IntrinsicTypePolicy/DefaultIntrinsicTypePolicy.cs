using System;
using Object = UnityEngine.Object;

namespace MackySoft.SerializeReferenceExtensions.Editor
{
    public sealed class DefaultIntrinsicTypePolicy : IIntrinsicTypePolicy
    {
        public static readonly DefaultIntrinsicTypePolicy Instance = new();

        public bool IsAllowed (Type candiateType)
        {
            return
                (candiateType.IsPublic || candiateType.IsNestedPublic || candiateType.IsNestedPrivate) &&
                !candiateType.IsAbstract &&
                !candiateType.IsGenericType &&
                !candiateType.IsPrimitive &&
                !candiateType.IsEnum &&
                !typeof(Object).IsAssignableFrom(candiateType) &&
                Attribute.IsDefined(candiateType, typeof(SerializableAttribute)) &&
                !Attribute.IsDefined(candiateType, typeof(HideInTypeMenuAttribute));
        }
    }
}