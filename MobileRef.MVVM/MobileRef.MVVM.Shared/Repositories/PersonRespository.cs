using System;
using Sterling.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MobileRef.MVVM.Shared
{
	public class PersonRespository: DbRepository<Person,int>
	{
		public PersonRespository(): base(AppDb.Database)
		{

		}
		public async Task<List<Person>> GetByFirstOrLastName(string value)
		{
			return await Task.Run(() =>
				{
					var results = (from x in AppDb.Database.Query<Person, int>()
						where x.LazyValue.Value.FirstName.Contains(value) ||
						x.LazyValue.Value.LastName.Contains(value)
						select x).ToList();

					return results.ToGenericList();
				});
		}
	}
}

