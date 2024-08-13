using System.Security.Cryptography;
using System.Text;

namespace api_dong_ho.General
{
	public class VnPayLibrary
	{
		private readonly SortedList<string, string> _requestData = new SortedList<string, string>();
		private readonly SortedList<string, string> _responseData = new SortedList<string, string>();

		public void AddRequestData(string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				_requestData.Add(key, value);
			}
		}

		public void AddResponseData(string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				_responseData.Add(key, value);
			}
		}

		public string CreateRequestUrl(string baseUrl, string hashSecret)
		{
			var data = new StringBuilder();
			foreach (var kv in _requestData)
			{
				if (data.Length > 0)
				{
					data.Append("&");
				}
				data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
			}

			var rawData = data.ToString();
			var signData = HmacSHA512(hashSecret, rawData);
			var paymentUrl = $"{baseUrl}?{rawData}&vnp_SecureHash={signData}";
			return paymentUrl;
		}

		public bool ValidateSignature(string inputHash, string secretKey)
		{
			var data = new StringBuilder();
			foreach (var kv in _responseData)
			{
				if (kv.Key != "vnp_SecureHash")
				{
					if (data.Length > 0)
					{
						data.Append("&");
					}
					data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
				}
			}

			var rawData = data.ToString();
			var myChecksum = HmacSHA512(secretKey, rawData);
			return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
		}

		private static string HmacSHA512(string key, string inputData)
		{
			var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
			var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
			return BitConverter.ToString(hash).Replace("-", "").ToLower();
		}
	}
}
