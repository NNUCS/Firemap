namespace FiremapHelper
{
    public class FiremapInputParameters
    {
        public int Humidity { get; set; }
        public int TreeCover { get; set; }
        public string? Model1 { get; set; }
        public string? Model2 { get; set; }
        public string? Model3 { get; set; }
        public string? Model4 { get; set; }
        public string? Model5 { get; set; }
        public string? Model6 { get; set; }
        public string? Model7 { get; set; }
        public string? Model8 { get; set; }

        //IsValid need to allow for the possibility of a null value
     
        public bool IsValid()
        {
            if (Humidity > 0 && TreeCover > 0
                && (Model1 == null || Model1 != string.Empty)
                && (Model2 == null || Model2 != string.Empty)
                && (Model3 == null || Model3 != string.Empty)
                && (Model4 == null || Model4 != string.Empty)
                && (Model5 == null || Model5 != string.Empty)
                && (Model6 == null || Model6 != string.Empty)
                && (Model7 == null || Model7 != string.Empty)
                && (Model8 == null || Model8 != string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }


}
