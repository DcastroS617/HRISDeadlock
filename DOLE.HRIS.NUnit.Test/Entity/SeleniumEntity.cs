namespace DOLE.HRIS.NUnit.Test
{
    public class SeleniumEntity
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SeleniumEntity()
        {
        }
       
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SeleniumEntity(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// Gets or set the code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or set the description
        /// </summary>
        public string Description { get; set; }

   
    }

}