using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ServiceStackTextSampleiOS
{
	// The ServiceStack Text serializers support [DataMember] aliases where you 
	// can use the Name parameter to specify what alias each field should be.
	// Once you add [DataContract] / [DataMember] attributes to your DTOs then the 
	// behavior becomes opt-in and you will have add [DataMember] on each of the 
	// properties you want serialized.
	[DataContract]
	public class Item
	{
		[DataMember (Name="title")]
		public string Title { get; set; }

		[DataMember (Name="lat")]
		public string Lat { get; set; }

		[DataMember (Name="long")]
		public string Lon { get; set; }

		[DataMember (Name="link")]
		public string Link { get; set; }

		[DataMember (Name="pubDate")]
		public string PubDate { get; set; }

		[DataMember (Name="condition")]
		public Condition Condition { get; set; }

		[DataMember (Name="description")]
		public string Description { get; set; }

		[DataMember (Name="forecast")]
		public List<Forecast> Forecast { get; set; }

		[DataMember (Name="guid")]
		public ServiceStackTextSampleiOS.Guid Guid { get; set; }
	}
}

