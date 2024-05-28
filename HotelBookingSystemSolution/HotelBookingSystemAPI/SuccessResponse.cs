namespace HotelBookingSystemAPI
{
    public class SuccessResponse
    {
        public int StatusCode { get; set; } = 200;
        public string SuccessMessage { get; set; } = string.Empty;
        public object? Data { get; set; }

        public SuccessResponse(string successMessage)
        {
            SuccessMessage = successMessage;
        }

        public SuccessResponse(string successMessage, object data)
        {
            SuccessMessage = successMessage;
            Data = data;
        }

    }
}
