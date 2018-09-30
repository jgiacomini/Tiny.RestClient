namespace Tiny.RestClient.Json
{
    /// <summary>
    ///  Allow SnakeCase (or name also Spinal case) resolve of property (property-name become PropertyName)
    /// </summary>
    /// <seealso cref="IFormatter" />
    public class KebabCasePropertyNamesContractResolver : SeparatorPropertyNamesContractResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnakeCasePropertyNamesContractResolver"/>
        /// </summary>
        public KebabCasePropertyNamesContractResolver()
            : base('-')
        {
        }
    }
}
