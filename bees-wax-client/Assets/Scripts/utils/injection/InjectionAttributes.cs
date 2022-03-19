using System;

namespace utils.injection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Inject : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Singleton : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PerNamedObject : Attribute
    {
    }
}