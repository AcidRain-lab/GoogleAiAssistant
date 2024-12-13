namespace BLL.DTO.Countries
{
    /// <summary>
    /// Provide country detail i.e. Id and Name 
    /// </summary>
    public class CountryDTO
    {
        /// <summary>
        /// get or set country Id
        /// </summary>
        public int CountryId { get; set; }
        /// <summary>
        /// get or set country name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
