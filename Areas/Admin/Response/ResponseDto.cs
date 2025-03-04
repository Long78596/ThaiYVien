namespace ThaiYVien.Areas.Admin.Response
{
	public class ResponseDto
	{
		public bool Success { get; set; }
		public string Message { get; set; }

		public ResponseDto(bool success, string message)
		{
			Success = success;
			Message = message;
		}
	}

}
