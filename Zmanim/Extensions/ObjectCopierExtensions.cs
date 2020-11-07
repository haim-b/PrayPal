using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace Zmanim.Extensions
{
    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ObjectCopierExtensions
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!IsSerializable(typeof(T)))
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            XmlObjectSerializer formatter = new DataContractSerializer(typeof(T));

            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.WriteObject(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.ReadObject(stream);
            }
        }

        public static bool IsSerializable(Type type)
        {
            if ((type.Attributes & TypeAttributes.Serializable) != TypeAttributes.NotPublic)
            {
                return true;
            }

            Type underlyingSystemType = type.UnderlyingSystemType;
            return ((underlyingSystemType != null) && IsSpecialSerializableType(underlyingSystemType));
        }

        
        private static bool IsSpecialSerializableType(Type type)
        {
            Type baseType = type;
            do
            {
                if ((baseType is Delegate) || (baseType.IsEnum))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }
            while (baseType != null);
            return false;
        }
    }
}