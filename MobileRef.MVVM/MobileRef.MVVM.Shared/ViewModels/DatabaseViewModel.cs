using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MobileRef.MVVM.Shared
{
	public class DatabaseViewModel:VMBase
	{
		private PersonRespository respository;
		private ObservableCollection<Person> people;
		private string searchCritiera;

		public string SearchCritiera {
			get {
				return searchCritiera;
			}
			set {
				searchCritiera = value;
				Notify ("SearchCritiera");
			}
		}

		public ICommand LoadData {get;set;}

		private async void LoadDataMethod(){
			LoadMessage = "Getting data...";
			IsLoading = true;
			if (!string.IsNullOrEmpty (SearchCritiera)) {
				var data = await respository.GetByFirstOrLastName (SearchCritiera);
				People = data.ToObservable ();
			} else {
				var data = await respository.GetAll ();
				if(data!=null)
					People = data.ToObservable ();
			}
			IsLoading = false;
		}

		public ObservableCollection<Person> People {
			get {
				return people;
			}
			set {
				people = value;
				Notify ("People");
			}
		}

		public DatabaseViewModel ()
		{
			People = new ObservableCollection<Person> ();
			respository = new PersonRespository ();
			LoadData = new RelayCommand (() => {
				LoadDataMethod();
			});
		}
	}
}

