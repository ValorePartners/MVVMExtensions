

	[Preserve(AllMembers=true)]
	public class Person
	{
		public int Id{ get; set; }

		public string FirstName{ get; set; }

		public string LastName{ get; set; }

		public string UserName{ get; set; }

		public string Password{ get; set; }

		public Person ()
		{

		}
	}

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
