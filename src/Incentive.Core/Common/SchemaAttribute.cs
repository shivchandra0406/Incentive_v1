using System;

namespace Incentive.Core.Common
{
    /// <summary>
    /// Attribute to specify the database schema for an entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SchemaAttribute : Attribute
    {
        /// <summary>
        /// Gets the schema name for the entity.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaAttribute"/> class.
        /// </summary>
        /// <param name="name">The schema name to use for the entity.</param>
        public SchemaAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Schema name cannot be null or empty.", nameof(name));
            
            Name = name;
        }
    }
}
