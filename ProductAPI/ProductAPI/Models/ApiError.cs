using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Models
{
	/// <summary>
	/// Custom class to allow API to provide further details when an error occurs
	/// </summary>
	public class ApiError : StatusCodeResult
	{
		#region Properties

		/// <summary>
		/// Human readable message giving further details why the error occurred
		/// </summary>
		public string? Message { get; }

		/// <summary>
		/// Further details regarding error.  If an exception occurred, this will have the exception stack trace
		/// </summary>
		public string? DebugMessage { get; }
		#endregion Properties

		#region Constructors
		/// <summary>
		/// Create new ApiError object with status code
		/// </summary>
		/// <param name="statusCode"></param>
		public ApiError(int statusCode)
			: base(statusCode)
		{ }

		/// <summary>
		/// Create new ApiError object with status code and message
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		public ApiError(int statusCode, string message)
			: base(statusCode)
		{
			Message = message;
		}

		/// <summary>
		/// Create new ApiError object with status code, message and an additional debug message
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		/// <param name="debugMessage"></param>
		public ApiError(int statusCode, string message, string debugMessage)
			: this(statusCode, message)
		{
			DebugMessage = debugMessage;
		}

		/// <summary>
		/// Create new ApiError object with status code and an exception
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="ex"></param>
		public ApiError(int statusCode, Exception ex)
			: this(statusCode)
		{
			Message = "Unknown Exception Occurred.  See DebugMessage for further details";
			DebugMessage = ex.Message;
		}

		/// <summary>
		/// Create ApiError object with status code, message and an exception
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		/// <param name="ex"></param>
		public ApiError(int statusCode, string message, Exception ex)
			: this(statusCode, message)
		{
			DebugMessage = ex.Message;
		}
		#endregion Constructors
	}
}
