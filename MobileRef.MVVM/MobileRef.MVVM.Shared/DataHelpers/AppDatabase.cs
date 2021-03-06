﻿using System;
using Wintellect.Sterling.Database;
using Wintellect.Sterling;
using Wintellect.Sterling.IsolatedStorage;
using Sterling.Common;
using System.Collections.Generic;

namespace MobileRef.MVVM.Shared
{
	public class DemoDatabase : BaseDatabaseInstance
	{
		public DemoDatabase(){

		}
		public override string Name
		{
			get { return "DemoDatabase"; }
		}

		protected override System.Collections.Generic.List<ITableDefinition> RegisterTables()
		{
			var list = new System.Collections.Generic.List<ITableDefinition>();
			list.Add(CreateTableDefinition<Person ,int>(i => i.Id));
			return list;
		}
	}

	public class AppDb
	{
		private static ISterlingDatabaseInstance _database = null;
		private static SterlingEngine _engine = null;
		private static SterlingDefaultLogger _logger = null;

		public static ISterlingDatabaseInstance Database
		{
			get
			{
				return _database;
			}
		}

		public static void Init(){
			if(_database==null)
				_ActivateEngine();
		}

		public static void Close(){
			_DeactivateEngine();
		}

		private static void _ActivateEngine()
		{
			_engine = new SterlingEngine();
			_logger = new SterlingDefaultLogger(SterlingLogLevel.Information);
			_engine.Activate();

			_database = _engine.SterlingDatabase.RegisterDatabase<DemoDatabase>(new IsolatedStorageDriver());

			var maxPdx = _database.GetIntegerIndex<Person>();
			_database.RegisterTrigger<Person, int>(new IntTrigger<Person>(maxPdx,"Id"));
	

		}

		private static void _DeactivateEngine()
		{
			_logger.Detach();
			_engine.Dispose();
			_database = null;
			_engine = null;
		}
	}
}

