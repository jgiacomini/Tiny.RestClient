namespace Tiny.RestClient.Json
{
    /// <summary>
    ///  Allow snake case resolve of property (property_name become PropertyName)
    /// </summary>
    /// <seealso cref="IFormatter" />
    public class SnakeCasePropertyNamesContractResolver : SeparatorPropertyNamesContractResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnakeCasePropertyNamesContractResolver"/>
        /// </summary>
        public SnakeCasePropertyNamesContractResolver()
            : base('_')
        {
        }
    }
}
