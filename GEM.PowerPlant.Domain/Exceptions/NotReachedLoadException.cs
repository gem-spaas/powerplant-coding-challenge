using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEM.PowerPlant.Domain.Exceptions
{
	public class DomainException : Exception
	{
		public DomainException(string errorMessage)
			: base(errorMessage)
		{
		}
	}
	public class NotReachedLoadException : DomainException
	{
		public NotReachedLoadException()
			: base("Couldn't generate the requested load")
		{
		}
	}
}
