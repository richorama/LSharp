#region About ..
//
// Based on code obtained from Jan Tielens weblog 
// at http://weblogs.asp.net/jan/archive/2003/12/04/41154.aspx
//
#endregion

using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LSharp.Libraries
{
	/// <summary>
	/// Provides a completely open certificate policy that accepts any
	/// certificate ** USE WITH CARE **
	/// </summary>
	public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
	{
		public TrustAllCertificatePolicy() 
		{}

		/// <summary>
		/// Accept any certificate
		/// </summary>
		/// <param name="sp"></param>
		/// <param name="cert"></param>
		/// <param name="req"></param>
		/// <param name="problem"></param>
		/// <returns></returns>
		public bool CheckValidationResult(ServicePoint sp, 
			X509Certificate cert,WebRequest req, int problem)
		{
			return true;
		}
	}

}
