using System;

namespace Coreapi.Common.Utility
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RedisHashFieldAttribute : Attribute
    {
        public RedisHashFieldAttribute(string redisFieldName, bool isArray = false) { }
    }
}
