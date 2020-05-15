using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPetS.Extensions
{
    public static class PetExtensions
    {
		public static async void SafeFireAndForget(this Task pet,
												   bool returnToCallingContext,
												   Action<Exception> onException = null)
		{
			try
			{
				await pet.ConfigureAwait(returnToCallingContext);
			}
			catch (Exception ex) when (onException != null)
			{
				onException(ex);
			}
		}
	}
}
